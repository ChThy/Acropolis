namespace Acropolis.Application.Events;

public record Request(string Message, Dictionary<string, string>? Params = null)
{
    public Dictionary<string, string> Params { get; init; } = Params ?? new Dictionary<string, string>();
}
