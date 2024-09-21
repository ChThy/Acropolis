using MassTransit;

namespace Acropolis.Application.Extensions.MassTransitExtensions;
public static class BehaviourContextExtensions
{
    public static (TSaga saga, TMessage message) Deconstruct<TSaga, TMessage>(this BehaviorContext<TSaga, TMessage> ctx) 
        where TSaga : class, SagaStateMachineInstance
        where TMessage : class
    {
        return (ctx.Saga, ctx.Message);
    }
}
