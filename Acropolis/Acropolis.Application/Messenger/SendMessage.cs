using Acropolis.Application.Mediator;

namespace Acropolis.Application.Messenger;

public record SendMessage(string Message, string Target) : ICommand;

public class SendMessageHandler : ICommandHandler<SendMessage>
{
    public ValueTask Handle(SendMessage command, CancellationToken cancellationToken = default)
    {
        return ValueTask.CompletedTask;
    }
}
