using Acropolis.Application.Events;
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
    private readonly YoutubeClient youtubeClient = youtubeClient;
    private readonly IFileStorage fileStorage = fileStorage;
    private readonly TimeProvider timeProvider = timeProvider;
    private readonly YoutubeOptions youtubeOptions = optionsMonitor.CurrentValue;
    private readonly ILogger<VideoDownloadRequestedHandler> logger = logger;

    public async Task Consume(ConsumeContext<VideoDownloadRequested> context)
    {
        var url = context.Message.Url;
        logger.LogDebug("Starting downloading video: {url}", url);

        if (!IsYoutubeUrl(url))
        {
            logger.LogWarning("Invalid Youtube URL: {url}", url);
            await context.Publish(new VideoDownloadFailed(url, timeProvider.GetLocalNow(), "Invalid Youtube URL."));
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

        var streamInfo = streamManifest.GetMuxedStreams()
            .Where(e => e.Container == Container.Mp4)
            .GetWithHighestVideoQuality();

        var stream = await youtubeClient.Videos.Streams.GetAsync(streamInfo, cancellationToken);

        var filename = ConstructFileName(video.Author.ChannelTitle, video.UploadDate,
            video.Title, Container.Mp4.ToString().ToLowerInvariant());
        var storedLocation = await fileStorage.StoreFile(filename, stream, cancellationToken);

        return (
            new VideoMetaData(
                videoId,
                video.Title,
                video.Author.ChannelTitle,
                video.UploadDate,
                storedLocation),
            stream);
    }

    private bool IsYoutubeUrl(string? url)
    {
        if (url is null)
            return false;
        return youtubeOptions.ValidUrls.Any(e =>
            url.StartsWith(e, StringComparison.InvariantCultureIgnoreCase));
    }

    private static string ConstructFileName(string author, DateTimeOffset uploaded, string title, string extension)
    {
        var timestamp = uploaded.ToString("yyyyMMdd");
        return Path.Join(author, $"{timestamp}_{title}.{extension}");
    }
}
