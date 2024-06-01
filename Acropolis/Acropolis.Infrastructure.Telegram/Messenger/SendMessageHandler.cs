using Acropolis.Application.Events;
using MassTransit;
using Telegram.Bot;

namespace Acropolis.Infrastructure.Telegram.Messenger;

public class SendMessageHandler(TelegramBotClient telegramBotClient) : IConsumer<ExternalMessageReplyRequested>
{
    public async Task Consume(ConsumeContext<ExternalMessageReplyRequested> context)
    {
        await telegramBotClient.SendTextMessageAsync(
            context.Message.MessageProps["ChatId"],
            context.Message.MessageBody,
            replyToMessageId: int.Parse(context.Message.MessageProps["MessageId"]),
            cancellationToken: context.CancellationToken);
    }
}