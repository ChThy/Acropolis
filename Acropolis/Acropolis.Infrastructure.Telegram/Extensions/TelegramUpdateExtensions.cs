using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Acropolis.Infrastructure.Telegram.Extensions;
public static class TelegramUpdateExtensions
{
    public static Dictionary<string, string> ExtractParams(this Update update)
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

    private static string GetUserName(string? firstName, string? lastName) => $"{firstName} {lastName}".Trim();

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
}
