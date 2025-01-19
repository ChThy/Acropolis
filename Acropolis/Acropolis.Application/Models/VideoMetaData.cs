namespace Acropolis.Application.Models;

[Obsolete]
public record VideoMetaData(
    string VideoId,
    string VideoTitle,
    string Author,
    DateTimeOffset VideoUploadTimestamp,
    string StorageLocation);
