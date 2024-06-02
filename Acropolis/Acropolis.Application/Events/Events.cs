using Acropolis.Application.Models;

namespace Acropolis.Application.Events;
public record UrlRequestReceived(Guid RequestId, string Url, DateTimeOffset Timestamp);
public record UrlRequestReplyRequested(Guid RequestId, string Url, string Message);

public record VideoDownloadAlreadyRequested(string Url, DateTimeOffset RequestedOnTimestamp);
public record VideoDownloaded(string Url, DateTimeOffset Timestamp, VideoMetaData VideoMetaData);
public record VideoAlreadyDownloaded(string Url, DateTimeOffset Timestamp, VideoMetaData VideoMetaData);
public record VideoDownloadFailed(string Url, DateTimeOffset Timestamp, string ErrorMessage);
public record VideoDownloadSkipped(string Url, DateTimeOffset Timestamp, string Reason);
public record VideoDownloadRequested(string Url);

public record ExternalMessageRequestReceived(
    Guid MessageId,
    string Channel,
    DateTimeOffset Timestamp,
    string MessageBody,
    Dictionary<string, string> MessageProps);

public record ExternalMessageReplyRequested(
    Guid MessageId,
    string Channel,
    string MessageBody,
    Dictionary<string, string> MessageProps);

