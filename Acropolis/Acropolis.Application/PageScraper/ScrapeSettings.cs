namespace Acropolis.Application.PageScraper;
public class ScrapeSettings
{
    public const string Name = "ScrapeSettings";
    public string ScraperEndpoint { get; set; } = null!;
    public string[] ResourceTypes { get; set; } = Array.Empty<string>();
}
