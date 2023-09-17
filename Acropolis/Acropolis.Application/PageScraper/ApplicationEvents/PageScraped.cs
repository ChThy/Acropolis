using Acropolis.Application.Events;
namespace Acropolis.Application.PageScraper.ApplicationEvents;
public record PageScraped(Guid ScrapedPageId, string PageTitle) : ApplicationEvent;
