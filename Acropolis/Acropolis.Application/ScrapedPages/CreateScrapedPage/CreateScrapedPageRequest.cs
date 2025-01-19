using Acropolis.Application.Events.PageScraper;
using Acropolis.Domain.ScrapedPages;
using MediatR;

namespace Acropolis.Application.ScrapedPages.CreateScrapedPage;

public record CreateScrapedPageRequest(PageScraped Page) : IRequest<ScrapedPage>;