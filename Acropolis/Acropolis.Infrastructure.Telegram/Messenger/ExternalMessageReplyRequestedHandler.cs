using Acropolis.Application.Events;
using MassTransit;
using Telegram.Bot;

namespace Acropolis.Infrastructure.Telegram.Messenger;

public class ExternalMessageReplyRequestedHandler(TelegramBotClient telegramBotClient) : IConsumer<ExternalMessageReplyRequested>
{
    public async Task Consume(ConsumeContext<ExternalMessageReplyRequested> context)
    {
        await telegramBotClient.SendMessage(
            context.Message.MessageProps["ChatId"],
            context.Message.MessageBody,
            replyParameters: new()
            {
                MessageId = int.Parse(context.Message.MessageProps["MessageId"]) 
            },
            cancellationToken: context.CancellationToken);
    }
}