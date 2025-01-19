namespace Acropolis.Domain.ScrapedPages;

public class ScrapedPage
{
    private readonly HashSet<Resource> resources = [];

    private ScrapedPage() {}
    
    public ScrapedPage(Guid id, string url, PageMetaData metaData) : this()
    {
        Id = id;
        Url = url;
        MetaData = metaData;
    }

    public Guid Id { get; private set; }
    public string Url { get; private set; }
    public PageMetaData MetaData { get; private set; }
    public IEnumerable<Resource> Resources => resources;
    
    public void AddResource(Resource resource)
    {
        resources.Add(resource);
    }
}