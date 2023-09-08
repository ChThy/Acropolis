namespace Acropolis.Domain.Messenger;

public record IncomingRequest
{
    public Guid Id { get; init; }
    public User User { get; init; } = null!;
    public string Source { get; init; } = null!;
    public DateTimeOffset Timestamp { get; init; }
    public DateTimeOffset? ProcessedOn { get; init; }
    public string RawContent { get; init; } = null!;

    public static IncomingRequest Create(User user, string source, string rawContent)
        => new() { User = user, Source = source, RawContent = rawContent };
}
