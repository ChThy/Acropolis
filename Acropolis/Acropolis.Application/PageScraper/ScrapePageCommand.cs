using Acropolis.Application.Mediator;

namespace Acropolis.Application.PageScraper;

public record ScrapePageCommand(Guid IncomingRequestId, string Url) : ICommand;
