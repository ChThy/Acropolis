namespace Acropolis.Application.Events.Infrastructure;

public interface IMessageSubscriber
{
    IMessageSubscription Subscribe(string name, Func<IMessage, ValueTask> onMessage);
    void Unsubscribe(IMessageSubscription subscription);
}
