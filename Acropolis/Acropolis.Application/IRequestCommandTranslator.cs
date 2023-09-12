using Acropolis.Application.Mediator;

namespace Acropolis.Application;

public interface IRequestCommandTranslator
{
    bool CanHandle(string request, Dictionary<string, string>? param = null);

    ICommandBase CreateCommand(string request, Dictionary<string, string>? param = null);
}
