using Acropolis.Application.Events;
using Acropolis.Application.Mediator;

namespace Acropolis.Application;

public interface IRequestCommandTranslator
{
    bool CanHandle(RequestReceived request);

    ICommandBase CreateCommand(RequestReceived request);
}
