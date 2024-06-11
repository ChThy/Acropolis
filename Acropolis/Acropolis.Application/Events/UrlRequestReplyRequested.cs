namespace Acropolis.Application.Events;

public record UrlRequestReplyRequested(Guid RequestId, string Url, string Message);