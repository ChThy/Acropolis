namespace Acropolis.Application.Events.Infrastructure;

public class MessageSubscription : IMessageSubscription
{
    private readonly IMessageSubscriber subscriber;
    private readonly Func<IMessage, ValueTask> onMessage;

    public string Name { get; }

    public MessageSubscription(IMessageSubscriber subscriber, string name, Func<IMessage, ValueTask> onMessage)
    {
        this.subscriber = subscriber;
        Name = name;
        this.onMessage = onMessage;
    }
    public ValueTask HandleMessage(IMessage message) => onMessage(message);

    public void Dispose() => subscriber.Unsubscribe(this);
}
