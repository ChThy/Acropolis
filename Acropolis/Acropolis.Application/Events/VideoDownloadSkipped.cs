namespace Acropolis.Application.Events;

public record VideoDownloadSkipped(string Url, DateTimeOffset Timestamp, string Reason);