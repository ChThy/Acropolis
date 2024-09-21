namespace Acropolis.Application.Events.VideoDownloader;

public record RetryFailedVideoDownloadRequested(string Url, DateTimeOffset Timestamp);