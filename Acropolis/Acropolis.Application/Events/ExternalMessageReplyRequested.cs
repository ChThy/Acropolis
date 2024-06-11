namespace Acropolis.Application.Events;

public record ExternalMessageReplyRequested(
    Guid MessageId,
    string Channel,
    string MessageBody,
    Dictionary<string, string> MessageProps);