namespace Acropolis.Application.Events.PageScraper;

public record PageScrapeFailed(string Url, DateTimeOffset Timestamp, string ErrorMessage);