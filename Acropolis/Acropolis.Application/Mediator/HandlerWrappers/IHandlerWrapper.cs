namespace Acropolis.Application.Mediator.HandlerWrappers;

public interface IHandlerWrapper 
{
    ValueTask<object?> Handle(IServiceProvider serviceProvider, object command, CancellationToken cancellationToken = default);
}