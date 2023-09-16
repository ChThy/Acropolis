namespace Acropolis.Application.YoutubeDownloader;
public interface IYoutubeService
{
    ValueTask<Guid> Download(string url);
    ValueTask RetryFailedDownloads();
}
