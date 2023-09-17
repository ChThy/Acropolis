using Acropolis.Application.Events;
using Acropolis.Application.Mediator;
using Acropolis.Application.Messenger;
using Acropolis.Application.PageScraper.ApplicationEvents;
using Acropolis.Domain.Messenger;
using Acropolis.Domain.Repositories;
using Acropolis.Infrastructure.Telegram.Extensions;
using System.Text.Json;
using Telegram.Bot.Types;

namespace Acropolis.Infrastructure.Telegram.PageScraper;
public class PageScrapedHandler : ApplicationEventHandler<PageScraped>
{
    private readonly IIncomingRequestRepostory incomingRequestRepostory;
    private readonly IMediator mediator;

    public PageScrapedHandler(IIncomingRequestRepostory incomingRequestRepostory, IMediator mediator)
    {
        this.incomingRequestRepostory = incomingRequestRepostory;
        this.mediator = mediator;
    }

    public override async ValueTask Handle(PageScraped command, CancellationToken cancellationToken = default)
    {
        var originatingRequest = await incomingRequestRepostory.GetByExternalId(command.ScrapedPageId);

        Dictionary<string, string> metaData = originatingRequest is not null ? ExtractMetaData(originatingRequest) : new();

        await mediator.Send(new SendMessage($"Done downloading {command.PageTitle}.", metaData), cancellationToken);
    }

    private static Dictionary<string, string> ExtractMetaData(IncomingRequest originatingRequest)
    {
        var update = JsonSerializer.Deserialize<Update>(originatingRequest.RawContent);
        return update is null ? new() : update.ExtractParams();
    }
}
