namespace Acropolis.Application.Events.VideoDownloader;

public record VideoDownloadFailed(string Url, DateTimeOffset Timestamp, string ErrorMessage);