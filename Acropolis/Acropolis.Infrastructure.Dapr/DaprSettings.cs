using System.ComponentModel.DataAnnotations;

namespace Acropolis.Infrastructure.Dapr;
public class DaprSettings
{
    public const string Name = "DaprSettings";

    [Required]
    public string PubSubName { get; set; } = null!;

    [Required]
    public string ScraperTopicName { get; set; } = null!;

    [Required]
    public string YoutubeDownloaderTopicName { get; set; } = null!;
}
