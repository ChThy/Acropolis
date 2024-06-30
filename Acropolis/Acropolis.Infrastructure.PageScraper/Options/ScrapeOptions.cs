namespace Acropolis.Infrastructure.PageScraper.Options;

public class ScrapeOptions
{
    public const string Name = "ScrapeOptions";
    public string[] IgnoredDomains { get; set; } = Array.Empty<string>();
}