namespace Acropolis.Domain;

public record User
{
    public static readonly User System = new() { ExternalId = Guid.Empty.ToString(), Name = "SYSTEM" };

    public string ExternalId { get; set; } = null!;
    public string Name { get; set; } = null!;
}
