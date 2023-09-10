using Microsoft.Extensions.DependencyInjection;

namespace Acropolis.Application.Mediator.HandlerWrappers;

internal abstract class ResultCommandHandlerWrapper<TResult> : IHandlerWrapper
{
    public abstract ValueTask<TResult> Handle(IServiceProvider serviceProvider, ICommand<TResult> command, CancellationToken cancellationToken = default);
}

internal class ResultCommandHandlerWrapper<TCommand, TResult> : ResultCommandHandlerWrapper<TResult>
    where TCommand : ICommand<TResult>
{
    public override ValueTask<TResult> Handle(IServiceProvider serviceProvider, ICommand<TResult> command, CancellationToken cancellationToken = default)
        => ResultCommandHandlerWrapper<TCommand, TResult>.Handle(serviceProvider, (TCommand)command, cancellationToken);

    public static async ValueTask<TResult> Handle(IServiceProvider serviceProvider, TCommand command, CancellationToken cancellationToken = default)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        var result = await handler.Handle(command, cancellationToken);
        return result;
    }
}