using Acropolis.Application.Models;

namespace Acropolis.Application.Events;

public record VideoAlreadyDownloaded(string Url, DateTimeOffset Timestamp, VideoMetaData VideoMetaData);