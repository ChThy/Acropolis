namespace Acropolis.Application.Events.PageScraper;

public record RetryFailedPageScrapeRequested(string Url, DateTimeOffset Timestamp);