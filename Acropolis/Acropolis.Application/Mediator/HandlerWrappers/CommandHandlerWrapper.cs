using Microsoft.Extensions.DependencyInjection;

namespace Acropolis.Application.Mediator.HandlerWrappers;

internal abstract class CommandHandlerWrapper : IHandlerWrapper
{
    public abstract ValueTask Handle(IServiceProvider serviceProvider, ICommand command, CancellationToken cancellationToken = default);

    public async ValueTask<object?> Handle(IServiceProvider serviceProvider, object command, CancellationToken cancellationToken = default)
    {
        await Handle(serviceProvider, (ICommand)command, cancellationToken);
        return Nop.Instance;
    }

    public readonly struct Nop
    {
        public static readonly Nop Instance = new();
    }
}

internal class CommandHandlerWrapper<TCommand> : CommandHandlerWrapper where TCommand : ICommand
{
    public override ValueTask Handle(IServiceProvider serviceProvider, ICommand command, CancellationToken cancellationToken = default)
        => CommandHandlerWrapper<TCommand>.Handle(serviceProvider, (TCommand)command, cancellationToken);

    public static async ValueTask Handle(IServiceProvider serviceProvider, TCommand command, CancellationToken cancellationToken = default)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.Handle(command, cancellationToken);
    }
}
