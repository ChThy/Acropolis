namespace Acropolis.Application.Events;
public record UrlRequestReceived(Guid RequestId, string Url, DateTimeOffset Timestamp);