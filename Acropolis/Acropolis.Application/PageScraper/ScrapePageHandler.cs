using Acropolis.Domain.Repositories;
using MediatR;

namespace Acropolis.Application.PageScraper;

public class ScrapePageHandler : IRequestHandler<ScrapePageCommand>
{
    private readonly IScrapeService scrapeService;
    private readonly IIncomingRequestRepostory incomingRequestRepostory;

    public ScrapePageHandler(IScrapeService webPageService, IIncomingRequestRepostory incomingRequestRepostory)
    {
        this.scrapeService = webPageService;
        this.incomingRequestRepostory = incomingRequestRepostory;
    }

    public async Task Handle(ScrapePageCommand command, CancellationToken cancellationToken = default)
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
