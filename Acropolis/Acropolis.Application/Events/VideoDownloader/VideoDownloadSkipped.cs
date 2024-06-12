namespace Acropolis.Application.Events.VideoDownloader;

public record VideoDownloadSkipped(string Url, DateTimeOffset Timestamp, string Reason);