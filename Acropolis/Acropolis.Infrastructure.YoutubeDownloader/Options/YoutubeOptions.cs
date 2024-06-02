namespace Acropolis.Infrastructure.YoutubeDownloader.Options;
public record YoutubeOptions
{
    public const string Name = "YoutubeOptions";
    public string[] ValidUrls { get; set; } = Array.Empty<string>();
}
