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
            UpdateType.Message,
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
            new(ExtractMessage(update), ExtractParams(update)));

        //TODO publish event
    }

    private static string ExtractMessage(Update update)
    {
        var message = update?.Message?.Text ??
            update?.ChannelPost?.Text ??
            update?.CallbackQuery?.Data ??
            "NOP";
        return message;
    }

    private Dictionary<string, string>? ExtractParams(Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => MessageParams(update.Message!),
            UpdateType.ChannelPost => ChannelPostParams(update.ChannelPost!),
            UpdateType.CallbackQuery => CallBackQueryParams(update.CallbackQuery!),
            _ => new Dictionary<string, string>()
        };
    }

    private static Dictionary<string, string> MessageParams(Message message)
    {
        return new Dictionary<string, string>
        {
            ["ChatId"] = message.Chat.Id.ToString(),
            ["ChatName"] = message.Chat.Title ?? "",
            ["MessageId"] = message.MessageId.ToString(),
            ["UserId"] = message.From!.Id.ToString(),
            ["UserName"] = GetUserName(message.From.FirstName, message.From.LastName),
            ["IsBot"] = message.From.IsBot.ToString()
        };
    }

    private static Dictionary<string, string> ChannelPostParams(Message channelPost)
    {   
        // No user info available in ChannelPosts
        return new Dictionary<string, string>
        {
            ["ChatId"] = channelPost.Chat.Id.ToString(),
            ["ChatName"] = channelPost.Chat.Title ?? "",
            ["MessageId"] = channelPost.MessageId.ToString()
        };
    }

    private static Dictionary<string, string> CallBackQueryParams(CallbackQuery callbackQuery)
    {
        return new Dictionary<string, string>
        {
            ["ChatId"] = callbackQuery.Message?.Chat?.Id.ToString() ?? "",
            ["ChatName"] = callbackQuery.Message?.Chat?.Title ?? "",
            ["MessageId"] = callbackQuery.Message?.MessageId.ToString() ?? "",
            ["UserId"] = callbackQuery.From!.Id.ToString(),
            ["UserName"] = GetUserName(callbackQuery.From.FirstName, callbackQuery.From.LastName),
            ["IsBot"] = callbackQuery.From.IsBot.ToString()
        };
    }       

    private static string GetUserName(string? firstName, string? lastName) => $"{firstName} {lastName}".Trim();

    private Task OnError(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Error while receiving Telegram Updates!");
        return Task.CompletedTask;
    }
}
