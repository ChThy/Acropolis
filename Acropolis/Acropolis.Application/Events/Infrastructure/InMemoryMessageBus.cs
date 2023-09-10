using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Acropolis.Application.Events.Infrastructure;

public class InMemoryMessageBus : IMessagePublisher, IMessageSubscriber, IDisposable
{
    private readonly Channel<IMessage> channel;
    private readonly ConcurrentDictionary<string, IMessageSubscription> subscriptions = new();
    private readonly ILogger<InMemoryMessageBus> logger;

    public InMemoryMessageBus(ILogger<InMemoryMessageBus> logger)
    {
        channel = Channel.CreateUnbounded<IMessage>();
        this.logger = logger;
    }

    public ValueTask Publish(IMessage message)
    {
        return channel.Writer.WriteAsync(message);
    }

    public async Task ProcessMessages()
    {
        await foreach (var message in channel.Reader.ReadAllAsync())
        {
            foreach (var subscription in subscriptions.Values) 
            {
                try
                {
                    await subscription.HandleMessage(message);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception while processing message {message} in subscription {subscripton}", 
                        message.GetType().Name, subscription.Name);
                }
            }
        }
    }

    public IMessageSubscription Subscribe(string name, Func<IMessage, ValueTask> onMessage)
    {
        var subscription = new MessageSubscription(this, name, onMessage);
        
        if (!subscriptions.TryAdd(name, subscription)) 
        {
            throw new Exception($"Failed to create subscription {name}");
        }

        return subscription;
    }

    public void Unsubscribe(IMessageSubscription subscription)
    {
        subscriptions.TryRemove(subscription.Name, out _);
    }

    public void Dispose()
    {
        foreach (var subscription in subscriptions.Values) 
        {
            subscription.Dispose();
        }
    }
}
