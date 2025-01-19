namespace Acropolis.Domain.DownloadedVideos;

public class DownloadedVideo
{
    private readonly HashSet<Resource> resources = [];

    private DownloadedVideo() {}
    
    public DownloadedVideo(Guid id, string url, VideoMetaData metaData) : this()
    {
        Id = id;
        Url = url;
        MetaData = metaData;
    }
    
    public Guid Id { get; private set; }
    public string Url { get; private set; }
    public VideoMetaData MetaData { get; private set; }
    public IEnumerable<Resource> Resources => resources;

    public void AddResource(Resource resource)
    {
        resources.Add(resource);
    }
}