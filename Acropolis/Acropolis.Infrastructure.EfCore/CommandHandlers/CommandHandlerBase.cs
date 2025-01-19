using Acropolis.Shared.Commands;

namespace Acropolis.Infrastructure.EfCore.CommandHandlers;

public abstract class CommandHandlerBase<TCommand, TDbContext>(TDbContext dbContext) : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    protected readonly TDbContext DbContext = dbContext;

    public abstract Task Handle(TCommand command, CancellationToken cancellationToken = default);

    Task ICommandHandler<TCommand>.Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        return Handle(command, cancellationToken);
    }
}