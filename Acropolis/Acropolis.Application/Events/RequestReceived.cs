namespace Acropolis.Application.Events;

public record RequestReceived(Guid Id,
    string UserExternalId,
    string UserName,
    string Source,
    DateTimeOffset Timestamp,
    Request Request);
