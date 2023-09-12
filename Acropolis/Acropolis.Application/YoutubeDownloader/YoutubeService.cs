using System.Net.Http.Json;

namespace Acropolis.Application.YoutubeDownloader;

public class YoutubeService : IYoutubeService
{
    private readonly HttpClient httpClient;

    public YoutubeService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async ValueTask Download(string url)
    {
        await httpClient.PostAsJsonAsync("download", new { url });
    }

    public async ValueTask RetryFailedDownloads()
    {
        await httpClient.PostAsJsonAsync<object>("retry-failed", new());
    }
}
