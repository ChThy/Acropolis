namespace Acropolis.Application.Events.Infrastructure;

public interface IMessageSubscription : IDisposable
{
    string Name { get; }
    ValueTask HandleMessage(IMessage message);
}
