public record YoutubeSettings
{
    public const string Name = "YoutubeSettings";
    public string YoutubeDownloaderEndpoint { get; set; } = null!;
    public string[] ValidUrls { get; set; } = Array.Empty<string>();
}