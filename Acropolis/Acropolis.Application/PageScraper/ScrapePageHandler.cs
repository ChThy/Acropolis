using Acropolis.Application.Mediator;
using Acropolis.Domain.Repositories;

namespace Acropolis.Application.PageScraper;

public class ScrapePageHandler : ICommandHandler<ScrapePageCommand>
{
    private readonly IScrapeService scrapeService;
    private readonly IIncomingRequestRepostory incomingRequestRepostory;

    public ScrapePageHandler(IScrapeService webPageService, IIncomingRequestRepostory incomingRequestRepostory)
    {
        this.scrapeService = webPageService;
        this.incomingRequestRepostory = incomingRequestRepostory;
    }

    public async ValueTask Handle(ScrapePageCommand command, CancellationToken cancellationToken = default)
    {
        var externalId = await scrapeService.Download(command.Url);

        await incomingRequestRepostory.Update(
            command.IncomingRequestId,
            req =>
            {
                req.AcceptedByExternalSystem(externalId, DateTimeOffset.UtcNow);
            });
    }
}
