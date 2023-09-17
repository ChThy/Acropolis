namespace Acropolis.Application.Mediator;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    ValueTask Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    ValueTask<TResult> Handle(TCommand command, CancellationToken cancellationToken = default);
}
