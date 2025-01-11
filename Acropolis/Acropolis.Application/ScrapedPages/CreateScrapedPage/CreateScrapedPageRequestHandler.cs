using Acropolis.Domain.ScrapedPages;
using MediatR;

namespace Acropolis.Application.ScrapedPages.CreateScrapedPage;

public class CreateScrapedPageRequestHandler : IRequestHandler<CreateScrapedPageRequest, ScrapedPage>
{
    public Task<ScrapedPage> Handle(CreateScrapedPageRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}