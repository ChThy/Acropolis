﻿namespace Acropolis.Domain.Messenger;

public record IncomingRequest
{
    public Guid Id { get; init; }
    public User User { get; init; } = null!;
    public string Source { get; init; } = null!;
    public DateTimeOffset Timestamp { get; init; }
    public DateTimeOffset? ProcessedOn { get; private set; }
    public string RawContent { get; init; } = null!;

    public static IncomingRequest Create(Guid id, DateTimeOffset timestamp, User user, string source, string rawContent)
        => new() { Id = id, Timestamp = timestamp, User = user, Source = source, RawContent = rawContent };

    public void MarkAsProcessed(DateTimeOffset dateTimeOffset) => ProcessedOn = dateTimeOffset;
}
