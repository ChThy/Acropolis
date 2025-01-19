namespace Acropolis.Domain.DownloadedVideos;

public record VideoMetaData(
    string VideoId,
    string VideoTitle,
    string Author,
    DateTimeOffset VideoUploadTimestamp);
