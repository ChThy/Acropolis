namespace Acropolis.Application.Events.Infrastructure;

public interface IMessagePublisher
{
    ValueTask Publish(IMessage message);
}
