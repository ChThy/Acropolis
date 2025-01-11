using Acropolis.Domain.ScrapedPages;
using MediatR;

namespace Acropolis.Application.ScrapedPages.CreateScrapedPage;

public record CreateScrapedPageRequest : IRequest<ScrapedPage>
{
    
}