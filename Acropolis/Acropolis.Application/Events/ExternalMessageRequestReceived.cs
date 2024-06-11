namespace Acropolis.Application.Events;

public record ExternalMessageRequestReceived(
    Guid MessageId,
    string Channel,
    DateTimeOffset Timestamp,
    string MessageBody,
    Dictionary<string, string> MessageProps);