using Acropolis.Application.Events.PageScraper;
using MassTransit;

namespace Acropolis.Application.ScrapedPages.CreateScrapedPage;

public class PageScrapedConsumer : IConsumer<PageScraped>
{
    public Task Consume(ConsumeContext<PageScraped> context)
    {
        throw new NotImplementedException();
    }
}