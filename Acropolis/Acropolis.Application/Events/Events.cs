namespace Acropolis.Application.Events;
public record VideoDownloadRequested(string Url, DateTimeOffset Timestamp);
