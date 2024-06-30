namespace Acropolis.Application.Events.PageScraper;

public record PageScraped(string Url, DateTimeOffset Timestamp, string PageTitle, string Domain, string StorageLocation);