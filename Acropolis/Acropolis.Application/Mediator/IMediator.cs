namespace Acropolis.Application.Mediator;

public interface IMediator
{
    ValueTask<object?> Send(object command, CancellationToken cancellationToken = default);
    ValueTask Send(ICommand command, CancellationToken cancellationToken = default);
    ValueTask<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
}