using Acropolis.Application.Events;
using Acropolis.Infrastructure.Telegram.Extensions;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Acropolis.Infrastructure.Telegram;

public sealed class MessageReceiver(
    TelegramBotClient telegramClient,
    IBus bus,
    IOptions<TelegramOptions> options,
    ILogger<MessageReceiver> logger)
    : BackgroundService
{
    private readonly TelegramOptions options = options.Value;
    private readonly IBus bus = bus;

    private readonly ReceiverOptions receiverOptions = new()
    {
        AllowedUpdates = new[]
        {
            UpdateType.Message,
            UpdateType.ChannelPost,
            UpdateType.CallbackQuery
        }
    };

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!options.Enabled)
        {
            logger.LogWarning("Telegram messaging is disabled. No messages will be received.");
            return Task.CompletedTask;
        }

        telegramClient.StartReceiving(
            ReceiveUpdates,
            OnError,
            receiverOptions,
            stoppingToken);

        logger.LogInformation("Started receiving Telegram messages.");

        return Task.CompletedTask;
    }

    private async Task ReceiveUpdates(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        var @event = new ExternalMessageRequestReceived(
            Guid.NewGuid(),
            Constants.TelegramChannel,
            DateTimeOffset.UtcNow,
            ExtractMessage(update),
            update.ExtractParams());

        await bus.Publish(@event, cancellationToken);
    }

    private Task OnError(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Error while receiving Telegram Updates!");
        return Task.CompletedTask;
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