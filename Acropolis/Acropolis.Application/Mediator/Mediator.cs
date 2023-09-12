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

    public async ValueTask<object?> Send(object command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        var handlerWrapper = commandHandlerWrappers.GetOrAdd(command.GetType(), static commandType =>
        {
            Type handlerWrapperType;

            var commandInterfaceType = commandType.GetInterfaces().FirstOrDefault(e => e ==  typeof(ICommand));
            if (commandInterfaceType is not null) 
            {
                handlerWrapperType = typeof(CommandHandlerWrapper<>).MakeGenericType(commandType);
            }
            else
            {
                commandInterfaceType = commandType.GetInterfaces().FirstOrDefault(e => e.IsGenericType && e.GetGenericTypeDefinition() == typeof(ICommand<>));
                if (commandInterfaceType is null ) 
                {
                    throw new InvalidOperationException($"Could not create command handler wrapper for {commandType}");
                }

                var responseType = commandInterfaceType.GetGenericArguments()[0];
                handlerWrapperType = typeof(ResultCommandHandlerWrapper<,>).MakeGenericType(commandType, responseType);

            }

            var handlerWrapper = Activator.CreateInstance(handlerWrapperType) as IHandlerWrapper 
                ?? throw new InvalidOperationException($"Could not create command handler wrapper for {commandType}");
            return handlerWrapper;

        });
        return await handlerWrapper.Handle(serviceProvider, command, cancellationToken);
    }

    public async ValueTask Send(ICommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));

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
        ArgumentNullException.ThrowIfNull(command, nameof(command));

        var handlerWrapper = (ResultCommandHandlerWrapper<TResult>)commandHandlerWrappers.GetOrAdd(command.GetType(), static commandType =>
        {
            var handlerWrapperType = typeof(ResultCommandHandlerWrapper<,>).MakeGenericType(commandType, typeof(TResult));
            var handlerWrapper = Activator.CreateInstance(handlerWrapperType) as ResultCommandHandlerWrapper<TResult>
                ?? throw new InvalidOperationException($"Could not create command handler wrapper for {commandType}");
            return handlerWrapper;
        });

        return await handlerWrapper.Handle(serviceProvider, command, cancellationToken);
    }    
}