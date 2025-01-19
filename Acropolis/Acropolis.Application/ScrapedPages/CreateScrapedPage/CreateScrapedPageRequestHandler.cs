using Acropolis.Application.Shared;
using Acropolis.Domain.ScrapedPages;
using Acropolis.Shared.Commands;
using MediatR;

namespace Acropolis.Application.ScrapedPages.CreateScrapedPage;

public class CreateScrapedPageRequestHandler(ICommandHandler commandHandler) : IRequestHandler<CreateScrapedPageRequest, ScrapedPage>
{
    public async Task<ScrapedPage> Handle(CreateScrapedPageRequest request, CancellationToken cancellationToken)
    {
        var page = new ScrapedPage(
            Guid.CreateVersion7(),
            request.Page.Url,
            new(request.Page.PageTitle, request.Page.Domain));

        page.AddResource(new(Guid.CreateVersion7(), request.Page.StorageLocation, request.Page.Timestamp));

        await commandHandler.Handle(SaveChangesCommand.AddSave(page), cancellationToken);
        return page;
    }
}