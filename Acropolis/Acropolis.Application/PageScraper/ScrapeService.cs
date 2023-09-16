using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Acropolis.Application.PageScraper;

public class ScrapeService : IScrapeService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<ScrapeService> logger;

    public ScrapeService(HttpClient httpClient, ILogger<ScrapeService> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async ValueTask<Guid> Download(string url)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("scrape-page", new { Url = url, ResourceTypes = new string[] { "png" } });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to download");
            throw;
        }
    }
}
