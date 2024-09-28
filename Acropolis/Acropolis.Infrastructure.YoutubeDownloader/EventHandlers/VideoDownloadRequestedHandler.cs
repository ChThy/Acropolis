using System.Diagnostics;
using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Models;
using Acropolis.Infrastructure.FileStorages;
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
    ILogger<VideoDownloadRequestedHandler> logger)
    : IConsumer<VideoDownloadRequested>
{
    private readonly IFileStorage fileStorage = fileStorage;
    private readonly ILogger<VideoDownloadRequestedHandler> logger = logger;
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
            var (videoMetaData, videoStream) = await DownloadVideo(url, context.CancellationToken);
            videoStream.Dispose();
            await context.Publish(new VideoDownloaded(url, timeProvider.GetLocalNow(), videoMetaData));
            logger.LogInformation("Video downloaded: {video}", videoMetaData);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to download video: {url}", url);
            await context.Publish(new VideoDownloadFailed(url, timeProvider.GetLocalNow(), ex.Message));
        }
    }

    private async Task<(VideoMetaData videoMetaData, Stream videoStream)> DownloadVideo(string url, CancellationToken cancellationToken)
    {
        var videoId = VideoIdExtractor.ExtractVideoId(url);
        var video = await youtubeClient.Videos.GetAsync(videoId, cancellationToken);
        var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(videoId, cancellationToken);

        var videoOnlyStreamInfos = streamManifest.GetVideoOnlyStreams()
            .Where(e => e.Container == Container.Mp4)
            .ToList();

        var directory = ConstructDirectory(video.Author.ChannelTitle);
        var filename = FileNameWithoutExtension(video.UploadDate, video.Title);
        
        var videoStreamInfo = WithLowestPreferredQuality(videoOnlyStreamInfos) ?? videoOnlyStreamInfos.GetWithHighestVideoQuality();
        var audioStreamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

        // IStreamInfo[] streams = [videoStreamInfo, audioStreamInfo];
        // var conversionRequest = new ConversionRequestBuilder(FileNameWithExtension(FullPath(directory, filename), videoStreamInfo.Container.Name.ToLowerInvariant()))
        //     .SetFFmpegPath(@"C:\ProgramData\chocolatey\bin")
        //     .Build();
        // await youtubeClient.Videos.DownloadAsync(streams, conversionRequest, cancellationToken: cancellationToken);
        
        //TODO dispose streams etc
        
        var videoOnlyFileName = VideoOnlyFileName(filename);
        var audioOnlyFileName = AudioOnlyFileName(filename);
        var videoOnlyFullPath = FullPath(directory, FileNameWithExtension(videoOnlyFileName, videoStreamInfo.Container.Name));
        var audioOnlyFullPath = FullPath(directory, FileNameWithExtension(audioOnlyFileName, videoStreamInfo.Container.Name));
        
        var videoStream = await youtubeClient.Videos.Streams.GetAsync(videoStreamInfo, cancellationToken);
        var audioStream = await youtubeClient.Videos.Streams.GetAsync(audioStreamInfo, cancellationToken);
        
        var storedVideoPart = await fileStorage.StoreFile(videoOnlyFullPath, videoStream, cancellationToken);
        var storedAudioPart = await fileStorage.StoreFile(audioOnlyFullPath, audioStream, cancellationToken);
        
        VideoService.MuxVideoWithAudio(videoOnlyFullPath, audioOnlyFullPath, 
            FullPath(directory, FileNameWithExtension(filename, videoStreamInfo.Container.Name.ToLowerInvariant())));
        
        return (
            new VideoMetaData(
                videoId,
                video.Title,
                video.Author.ChannelTitle,
                video.UploadDate,
                "storedLocation"),
            null);
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
        return Path.Join($"{timestamp}_{title}");
    }

    private static string FileNameWithExtension(string filename, string extension) => $"{filename}.{extension}";
    private static string ConstructDirectory(string author) => Path.Join("youtubedownloader", author); 
    private static string FullPath(string directory, string filename) => Path.Join(directory, filename);
    private static string VideoOnlyFileName(string fileName) => $"VideoPart.{fileName}";
    private static string AudioOnlyFileName(string fileName) => $"AudioPart.{fileName}";
}

public static class VideoService
{
    
    // See https://gist.github.com/AlexMAS/276eed492bc989e13dcce7c78b9e179d
    // https://gist.github.com/georg-jung/3a8703946075d56423e418ea76212745
    public static void MuxVideoWithAudio(string videoPath, string audioPath, string outputPath)
    {
        var ffmpegPath = @"C:\ProgramData\chocolatey\bin\ffmpeg.exe";
        var arguments = $"-i {videoPath} -i {audioPath} -c:v copy -c:a aac {outputPath}";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        
        process.Start();
        process.WaitForExit();
    }
}