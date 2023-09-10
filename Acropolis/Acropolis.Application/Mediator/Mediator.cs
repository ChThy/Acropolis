using Acropolis.Application.Mediator.HandlerWrappers;
using System.Collections.Concurrent;

namespace Acropolis.Application.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider serviceProvider;
    private static readonly ConcurrentDictionary<Type, IHandlerWrapper> commandHandlerWrappers = new();

    public Mediator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async ValueTask Send(ICommand command, CancellationToken cancellationToken = default)
    {
        var handlerWrapper = (CommandHandlerWrapper)commandHandlerWrappers.GetOrAdd(command.GetType(), static commandType =>
        {
            var handlerWrapperType = typeof(CommandHandlerWrapper<>).MakeGenericType(commandType);
            var handlerWrapper = Activator.CreateInstance(handlerWrapperType) as CommandHandlerWrapper
                ?? throw new InvalidOperationException($"Could not create command handler wrapper for {commandType}");
            return handlerWrapper;
        });
        await handlerWrapper.Handle(serviceProvider, command, cancellationToken);
    }

    public async ValueTask<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var handlerWrapper = (ResultCommandHandlerWrapper<TResult>)commandHandlerWrappers.GetOrAdd(command.GetType(), static commandType =>
        {
            var handlerWrapperType = typeof(ResultCommandHandlerWrapper<,>).MakeGenericType(commandType, typeof(TResult));
            var handlerWrapper = Activator.CreateInstance(handlerWrapperType) as ResultCommandHandlerWrapper<TResult>
                ?? throw new InvalidOperationException($"Could not create command handler wrapper for {commandType}");
            return handlerWrapper;
        });

        var result = await handlerWrapper.Handle(serviceProvider, command, cancellationToken);
        return result;
    }
}