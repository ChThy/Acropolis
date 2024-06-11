namespace Acropolis.Application.Events;

public record VideoDownloadAlreadyRequested(string Url, DateTimeOffset RequestedOnTimestamp);