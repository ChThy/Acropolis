using Acropolis.Domain.DownloadedVideos;

namespace Acropolis.Domain.Models;

public record StoredVideo
{
    private StoredVideo()
    {
    }

    public StoredVideo(VideoMetaData metaData, string storageLocation) : this()
    {
        MetaData = metaData;
        StorageLocation = storageLocation;
    }

    public VideoMetaData MetaData { get; init; } = null!;
    public string StorageLocation { get; init; } = null!;
}