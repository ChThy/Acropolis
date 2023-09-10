public record YoutubeSettings
{
    public const string Name = "YoutubeSettings";
    public string[] ValidUrls { get; set; } = Array.Empty<string>();
}