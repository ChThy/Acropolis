using Acropolis.Application.Models;

namespace Acropolis.Application.Events;
public record VideoDownloadRequestReceived(string Url, DateTimeOffset Timestamp);
public record VideoDownloadAlreadyRequested(string Url, DateTimeOffset RequestedOnTimestamp);
public record VideoDownloaded(string Url, DateTimeOffset Timestamp, VideoMetaData VideoMetaData);
public record VideoAlreadyDownloaded(string Url, DateTimeOffset Timestamp, VideoMetaData VideoMetaData);
public record VideoDownloadFailed(string Url, DateTimeOffset Timestamp, string ErrorMessage);


public record VideoDownloadRequested(string Url);
