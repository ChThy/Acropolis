namespace Acropolis.Application.Mediator;

public interface IMediator
{
    ValueTask Send(ICommand command, CancellationToken cancellationToken = default);
    ValueTask<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
}