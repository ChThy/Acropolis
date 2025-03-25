using Acropolis.Domain.DownloadedVideos;

namespace Acropolis.Application.Events.VideoDownloader;

public record VideoDownloaded(string Url, DateTimeOffset Timestamp, VideoMetaData VideoMetaData, string StorageLocation);