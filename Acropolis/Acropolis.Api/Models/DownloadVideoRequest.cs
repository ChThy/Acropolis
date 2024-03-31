namespace Acropolis.Api.Models;

public record DownloadVideoRequest
{
    public string Url { get; init; } = null!;
}
