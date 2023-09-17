namespace Acropolis.Application.Mediator;

public interface ICommandBase { };
public interface ICommand : ICommandBase { };
public interface ICommand<TResult> : ICommandBase { };

