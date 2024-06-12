using Acropolis.Application.Models;

namespace Acropolis.Application.Events.VideoDownloader;

public record VideoDownloaded(string Url, DateTimeOffset Timestamp, VideoMetaData VideoMetaData);