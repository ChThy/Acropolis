using Acropolis.Application.Models;
using Acropolis.Domain;

namespace Acropolis.Application.Events;
public record VideoDownloadRequestReceived(string Url, DateTimeOffset Timestamp);
public record VideoDownloadAlreadyRequested(string Url, DateTimeOffset RequestedOnTimestamp);
public record VideoDownloaded(string Url, DateTimeOffset Timestamp, VideoMetaData VideoMetaData);
public record VideoAlreadyDownloaded(string Url, DateTimeOffset Timestamp, VideoMetaData VideoMetaData);
public record VideoDownloadFailed(string Url, DateTimeOffset Timestamp, string ErrorMessage);
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
