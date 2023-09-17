namespace Acropolis.Domain.Exceptions;
public class NotFoundException : Exception
{
    public NotFoundException(string resource, string identifier) : 
        base($"Could not find resource of type {resource} with identifier {identifier}")
    {
        Resource = resource;
        Identifier = identifier;
    }

    public string Resource { get; }
    public string Identifier { get; }
}
