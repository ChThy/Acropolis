using MediatR;

namespace Acropolis.Application.PageScraper;

public record ScrapePageCommand(Guid IncomingRequestId, string Url) : IRequest;
