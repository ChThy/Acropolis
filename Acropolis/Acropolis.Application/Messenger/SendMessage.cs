using Acropolis.Application.Mediator;

namespace Acropolis.Application.Messenger;

public record SendMessage(string Message, Dictionary<string, string> Params) : ICommand;
