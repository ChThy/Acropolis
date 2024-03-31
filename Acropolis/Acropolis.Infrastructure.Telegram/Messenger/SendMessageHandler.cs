//using Acropolis.Application.Mediator;
//using Acropolis.Application.Messenger;
//using Telegram.Bot;

//namespace Acropolis.Infrastructure.Telegram.Messenger;
//public class SendMessageHandler : ICommandHandler<SendMessage>
//{
//    private readonly TelegramBotClient telegramBotClient;

//    public SendMessageHandler(TelegramBotClient telegramBotClient)
//    {
//        this.telegramBotClient = telegramBotClient;
//    }

//    public async ValueTask Handle(SendMessage command, CancellationToken cancellationToken = default)
//    {
//        await telegramBotClient.SendTextMessageAsync(
//            chatId: command.Params["ChatId"],
//            text: command.Message,
//            replyToMessageId: int.Parse(command.Params["MessageId"]),
//            cancellationToken: cancellationToken);
//    }
//}
