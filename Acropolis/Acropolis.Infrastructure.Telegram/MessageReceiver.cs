using Acropolis.Domain.Messenger;
using Acropolis.Domain.Repositories;
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
    private readonly IIncomingRequestRepostiory repostiory;
    private readonly TelegramOptions options;
    private readonly ILogger<MessageReceiver> logger;

    private readonly ReceiverOptions receiverOptions = new()
    {
        AllowedUpdates = new[]
        {
           UpdateType.ChannelPost,
           UpdateType.CallbackQuery
        }
    };

    public MessageReceiver(
        TelegramBotClient client,
        IIncomingRequestRepostiory repostiory,
        IOptions<TelegramOptions> options,
        ILogger<MessageReceiver> logger)
    {
        this.TelegramClient = client;
        this.repostiory = repostiory;
        this.options = options.Value;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!options.Enabled)
        {
            logger.LogWarning("Telegram messaging is disabled. No messages will be received.");
            return;
        }

        TelegramClient.StartReceiving(
            updateHandler: ReceiveUpdates,
            pollingErrorHandler: OnError,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken);

        logger.LogInformation("Started receiving Telegram messages.");
    }

    private async Task ReceiveUpdates(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        var rawContent = JsonSerializer.Serialize(update);
        var incomingRequest = IncomingRequest.Create(Domain.User.System, $"TELEGRAM_{update.Id}", rawContent);

        await repostiory.Add(incomingRequest);
    }

    private Task OnError(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Error while receiving Telegram Updates!");
        return Task.CompletedTask;
    }
}
