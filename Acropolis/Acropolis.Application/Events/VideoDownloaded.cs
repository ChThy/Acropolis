using Acropolis.Application.Models;

namespace Acropolis.Application.Events;

public record VideoDownloaded(string Url, DateTimeOffset Timestamp, VideoMetaData VideoMetaData);