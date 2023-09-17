using Acropolis.Application.Mediator;

namespace Acropolis.Application.Events;

public abstract record ApplicationEvent : Message, ICommand;
