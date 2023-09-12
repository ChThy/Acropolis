namespace Acropolis.Application.YoutubeDownloader;
public interface IYoutubeService
{
    ValueTask Download(string url);
    ValueTask RetryFailedDownloads();
}
