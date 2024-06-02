namespace Acropolis.Application.Models;
public record VideoMetaData(
    string VideoId,
    string VideoTitle,
    string Author,
    DateTimeOffset VideoUploadTimestamp,
    string StorageLocation);
