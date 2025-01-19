namespace Acropolis.Domain;

public class Resource
{
    public Resource(Guid id, string storageLocation, DateTimeOffset createdTimestamp)
    {
        Id = id;
        StorageLocation = storageLocation;
        CreatedTimestamp = createdTimestamp;
    }
    
    public Guid Id { get; private set; }

    public string StorageLocation { get; private set; }
    public DateTimeOffset CreatedTimestamp { get; private set; }
    public int Views { get; private set; }
    
    public void IncrementViews() => Views++;
    public void ResetViews() => Views = 0;
}