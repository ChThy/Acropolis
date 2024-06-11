using Acropolis.Application.Events;
using MassTransit;

namespace Acropolis.Infrastructure.PageScraper.EventHandlers;

public class PageScrapeRequestedHandler : IConsumer<PageScrapeRequested>
{
    public async Task Consume(ConsumeContext<PageScrapeRequested> context)
    {
        
    }
}