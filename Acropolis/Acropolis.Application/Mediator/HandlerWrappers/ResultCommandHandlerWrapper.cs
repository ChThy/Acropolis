using Microsoft.Extensions.DependencyInjection;

namespace Acropolis.Application.Mediator.HandlerWrappers;

internal abstract class ResultCommandHandlerWrapper<TResult> : IHandlerWrapper
{
    public abstract ValueTask<TResult> Handle(IServiceProvider serviceProvider, ICommand<TResult> command, CancellationToken cancellationToken = default);
    public async ValueTask<object?> Handle(IServiceProvider serviceProvider, object command, CancellationToken cancellationToken = default)
        => await Handle(serviceProvider, (ICommand<TResult>)command, cancellationToken);
}

internal class ResultCommandHandlerWrapper<TCommand, TResult> : ResultCommandHandlerWrapper<TResult>
    where TCommand : ICommand<TResult>
{
    public override ValueTask<TResult> Handle(IServiceProvider serviceProvider, ICommand<TResult> command, CancellationToken cancellationToken = default)
        => Handle(serviceProvider, (TCommand)command, cancellationToken);

    public async ValueTask<TResult> Handle(IServiceProvider serviceProvider, TCommand command, CancellationToken cancellationToken = default)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        var result = await handler.Handle(command, cancellationToken);
        return result;
    }
}