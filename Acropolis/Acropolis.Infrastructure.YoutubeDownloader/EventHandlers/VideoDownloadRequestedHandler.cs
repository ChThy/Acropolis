using System.Diagnostics;
using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Services;
using Acropolis.Domain.DownloadedVideos;
using Acropolis.Domain.Models;
using Acropolis.Infrastructure.FileStorages;
using Acropolis.Infrastructure.Helpers;
using Acropolis.Infrastructure.YoutubeDownloader.Helpers;
using Acropolis.Infrastructure.YoutubeDownloader.Options;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Acropolis.Infrastructure.YoutubeDownloader.EventHandlers;

public class VideoDownloadRequestedHandler(
    YoutubeClient youtubeClient,
    IOptionsMonitor<YoutubeOptions> optionsMonitor,
    IFileStorage fileStorage,
    TimeProvider timeProvider,
    ProcessService processService,
    ILogger<VideoDownloadRequestedHandler> logger)
    : IConsumer<VideoDownloadRequested>
{
    private readonly IFileStorage fileStorage = fileStorage;
    private readonly ILogger<VideoDownloadRequestedHandler> logger = logger;
    private readonly ProcessService processService = processService;
    private readonly TimeProvider timeProvider = timeProvider;
    private readonly YoutubeClient youtubeClient = youtubeClient;
    private readonly YoutubeOptions youtubeOptions = optionsMonitor.CurrentValue;

    public async Task Consume(ConsumeContext<VideoDownloadRequested> context)
    {
        var url = context.Message.Url;
        logger.LogDebug("Starting downloading video: {url}", url);

        if (!IsYoutubeUrl(url))
        {
            logger.LogWarning("Invalid Youtube URL: {url}", url);
            await context.Publish(new VideoDownloadSkipped(url, timeProvider.GetLocalNow(), "Not a Youtube URL."));
            return;
        }

        try
        {
            var storedVideo = await DownloadVideo(new(url), context.CancellationToken);
            await context.Publish(new VideoDownloaded(url, timeProvider.GetLocalNow(), storedVideo.MetaData, storedVideo.StorageLocation));
            logger.LogInformation("Video downloaded: {video}", storedVideo);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to download video: {url}", url);
            await context.Publish(new VideoDownloadFailed(url, timeProvider.GetLocalNow(), ex.Message));
        }
    }

    private async Task<StoredVideo> DownloadVideo(Uri uri, CancellationToken cancellationToken)
    {
        var videoId = VideoIdExtractor.ExtractVideoId(uri);
        var video = await youtubeClient.Videos.GetAsync(videoId, cancellationToken);
        var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(videoId, cancellationToken);

        var videoOnlyStreamInfos = streamManifest.GetVideoOnlyStreams()
            .Where(e => e.Container == Container.Mp4)
            .ToList();

        var directory = ConstructDirectory(video.Author.ChannelTitle);
        var filename = FileNameWithoutExtension(video.UploadDate, video.Title)
            .Replace("'", "")
            .Replace("-", "_");

        var videoStreamInfo = WithLowestPreferredQuality(videoOnlyStreamInfos) ?? videoOnlyStreamInfos.GetWithHighestVideoQuality();
        var audioStreamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

        var tempDirectory = ConstructDirectory(youtubeOptions.ToMuxDirectory);
        var tempFileName = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        
        var videoOnlyFileName = VideoOnlyFileName(tempFileName);
        var audioOnlyFileName = AudioOnlyFileName(tempFileName);
        var tempVideoFilePath = FullPath(tempDirectory, FileNameWithExtension(videoOnlyFileName, videoStreamInfo.Container.Name));
        var tempAudioFilePath = FullPath(tempDirectory, FileNameWithExtension(audioOnlyFileName, videoStreamInfo.Container.Name));

        await using var videoStream = await youtubeClient.Videos.Streams.GetAsync(videoStreamInfo, cancellationToken);
        await using var audioStream = await youtubeClient.Videos.Streams.GetAsync(audioStreamInfo, cancellationToken);

        var storedVideoPart = await fileStorage.StoreFile(tempVideoFilePath, videoStream, cancellationToken);
        var storedAudioPart = await fileStorage.StoreFile(tempAudioFilePath, audioStream, cancellationToken);

        var fileDirectory = Path.GetDirectoryName(storedVideoPart);
        var videoFullPath = Path.Combine(Directory.GetCurrentDirectory(), storedVideoPart);
        var audioFullPath = Path.Combine(Directory.GetCurrentDirectory(), storedAudioPart);
        var outputPath = Path.Join(Directory.GetCurrentDirectory(), directory, FileNameWithExtension(filename, videoStreamInfo.Container.Name.ToLowerInvariant()));

        var muxResult = await processService.RunProcessAsync("ffmpeg", $"-i \"{videoFullPath}\" -i \"{audioFullPath}\" -c:v copy -c:a aac \"{outputPath}\"");
        if (muxResult.ExitCode != 0)
        {
            throw new($"Failed to mux video {outputPath}");
        }

        fileStorage.DeleteFile(tempVideoFilePath);
        fileStorage.DeleteFile(tempAudioFilePath);

        return new StoredVideo(
            new VideoMetaData(
                videoId,
                video.Title,
                video.Author.ChannelTitle,
                video.UploadDate),
            Path.Join(fileDirectory, FileNameWithExtension(filename, videoStreamInfo.Container.Name.ToLowerInvariant())));
    }

    private VideoOnlyStreamInfo? WithLowestPreferredQuality(ICollection<VideoOnlyStreamInfo> streams) =>
        streams.Any(e => youtubeOptions.PreferredQualityValues.Contains(e.VideoQuality.Label))
            ? streams.OrderBy(e => e.VideoQuality.Label).First()
            : null;

    private bool IsYoutubeUrl(string? url)
    {
        if (url is null)
        {
            return false;
        }

        return youtubeOptions.ValidUrls.Any(e =>
            url.StartsWith(e, StringComparison.InvariantCultureIgnoreCase));
    }

    private static string FileNameWithoutExtension(DateTimeOffset uploaded, string title)
    {
        var timestamp = uploaded.ToString("yyyyMMdd");
        return $"{timestamp}_{title}".RemoveInvalidFileNameChars();
    }

    private static string FileNameWithExtension(string filename, string extension) => $"{filename}.{extension}";
    private static string ConstructDirectory(string author) => Path.Join("youtubedownloader", author).RemoveInvalidDirectoryChars();
    private static string FullPath(string directory, string filename) => Path.Join(directory, filename);
    private static string VideoOnlyFileName(string fileName) => $"VideoPart.{fileName}";
    private static string AudioOnlyFileName(string fileName) => $"AudioPart.{fileName}";
}

public static class VideoService
{
    public static void MuxVideoWithAudio(string videoPath, string audioPath, string outputPath)
    {
        var ffmpegPath = @"ffmpeg";
        var arguments = $"-i {videoPath} -i {audioPath} -c:v copy -c:a aac {outputPath}";

        var process = new Process
        {
            StartInfo = new()
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };


        process.Start();
        process.WaitForExit();
    }
}