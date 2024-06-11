namespace Acropolis.Application.Events;

public record VideoDownloadFailed(string Url, DateTimeOffset Timestamp, string ErrorMessage);