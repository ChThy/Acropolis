using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Acropolis.Application.YoutubeDownloader;

public class YoutubeService : IYoutubeService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<YoutubeService> logger;

    public YoutubeService(HttpClient httpClient, ILogger<YoutubeService> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async ValueTask Download(string url)
    {
        try
        {

            var response = await httpClient.PostAsJsonAsync("download", url);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(await response.Content.ReadAsStringAsync());
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to download");
        }
    }

    public async ValueTask RetryFailedDownloads()
    {
        await httpClient.PostAsJsonAsync<object>("retry-failed", new());
    }
}
