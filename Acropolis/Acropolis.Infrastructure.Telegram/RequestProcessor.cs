using Acropolis.Application.Events.Infrastructure;
using Acropolis.Domain;
using Acropolis.Domain.Messenger;
using Acropolis.Infrastructure.Telegram.Extensions;
using System.Text.Json;
using Telegram.Bot.Types;

namespace Acropolis.Infrastructure.Telegram;

public class RequestProcessor : IRequestProcessor
{
    private readonly IMessagePublisher publisher;

    public RequestProcessor(IMessagePublisher publisher)
    {
        this.publisher = publisher;
    }

    public async ValueTask Process(IncomingRequest request, CancellationToken cancellationToken)
    {
        var originalUpdate = JsonSerializer.Deserialize<Update>(request.RawContent);
        await PublishEvent(request, originalUpdate, cancellationToken);
    }

    private async Task PublishEvent(IncomingRequest incomingRequest, Update update, CancellationToken cancellationToken)
    {
        var @event = new Application.Events.RequestReceived(
            incomingRequest.Id,
            incomingRequest.User.ExternalId,
            incomingRequest.User.Name,
            incomingRequest.Source,
            incomingRequest.Timestamp,
            new(ExtractMessage(update), update.ExtractParams()));

        await publisher.Publish(@event);
    }

    private static string ExtractMessage(Update update)
    {
        var message = update?.Message?.Text ??
            update?.ChannelPost?.Text ??
            update?.CallbackQuery?.Data ??
            "NOP";
        return message;
    }
}
