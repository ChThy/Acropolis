namespace Acropolis.Shared.Commands;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler
{
    Task Handle<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;
}