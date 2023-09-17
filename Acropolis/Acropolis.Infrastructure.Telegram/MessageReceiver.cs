using Acropolis.Application.Events.Infrastructure;
using Acropolis.Domain.Messenger;
using Acropolis.Domain.Repositories;
using Acropolis.Infrastructure.Telegram.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Acropolis.Infrastructure.Telegram;

public sealed class MessageReceiver : BackgroundService
{
    private readonly TelegramBotClient TelegramClient;
    private readonly IIncomingRequestRepostory repostiory;
    private readonly IMessagePublisher publisher;
    private readonly TelegramOptions options;
    private readonly ILogger<MessageReceiver> logger;

    private readonly ReceiverOptions receiverOptions = new()
    {
        AllowedUpdates = new[]
        {
            UpdateType.Message,
            UpdateType.ChannelPost,
            UpdateType.CallbackQuery
        }
    };

    public MessageReceiver(
        TelegramBotClient telegramClient,
        IIncomingRequestRepostory repostiory,
        IMessagePublisher publisher,
        IOptions<TelegramOptions> options,
        ILogger<MessageReceiver> logger)
    {
        this.TelegramClient = telegramClient;
        this.repostiory = repostiory;
        this.publisher = publisher;
        this.options = options.Value;
        this.logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!options.Enabled)
        {
            logger.LogWarning("Telegram messaging is disabled. No messages will be received.");
            return Task.CompletedTask;
        }

        TelegramClient.StartReceiving(
            updateHandler: ReceiveUpdates,
            pollingErrorHandler: OnError,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken);

        logger.LogInformation("Started receiving Telegram messages.");

        return Task.CompletedTask;
    }

    private async Task ReceiveUpdates(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        var rawContent = JsonSerializer.Serialize(update);
        var incomingRequest = IncomingRequest.Create(Guid.NewGuid(), DateTimeOffset.UtcNow, Domain.User.System, $"TELEGRAM_{update.Id}", rawContent);

        await repostiory.Add(incomingRequest);
        await PublishEvent(incomingRequest, update, cancellationToken);
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

    private Task OnError(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Error while receiving Telegram Updates!");
        return Task.CompletedTask;
    }
}
