using Acropolis.Application.Mediator;

namespace Acropolis.Application.Events;

public abstract class ApplicationEventHandler<TEvent> : ICommandHandler<TEvent> where TEvent : ICommand
{
    public abstract ValueTask Handle(TEvent command, CancellationToken cancellationToken = default);
}
