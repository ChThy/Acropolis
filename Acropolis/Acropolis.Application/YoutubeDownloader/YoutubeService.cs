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

    public async ValueTask<Guid> Download(string url)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("download", url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to download");
            throw;
        }
    }

    public async ValueTask RetryFailedDownloads()
    {
        try
        {
            await httpClient.PostAsJsonAsync<object>("retry-failed", new());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to download");
        }
    }
}
