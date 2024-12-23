using Microsoft.Extensions.DependencyInjection;

namespace Acropolis.Shared.Commands;

public class CommandHandler(IServiceProvider serviceProvider) : ICommandHandler
{
    public Task Handle<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        return handler.Handle(command, cancellationToken);
    }
}