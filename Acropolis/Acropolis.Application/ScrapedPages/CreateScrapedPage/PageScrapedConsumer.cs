using Acropolis.Application.Events.PageScraper;
using MassTransit;
using MediatR;

namespace Acropolis.Application.ScrapedPages.CreateScrapedPage;

public class PageScrapedConsumer(IMediator mediator) : IConsumer<PageScraped>
{
    public Task Consume(ConsumeContext<PageScraped> context)
    {
        return mediator.Send(new CreateScrapedPageRequest(context.Message), context.CancellationToken);
    }
}