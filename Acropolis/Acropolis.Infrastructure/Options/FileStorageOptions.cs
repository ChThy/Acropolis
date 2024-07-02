namespace Acropolis.Infrastructure.Options;
public class FileStorageOptions
{
    public const string Name = "FileStorageOptions";

    public string BaseDirectory { get; set; } = string.Empty;
    public int MaxFilenameLength { get; set; } = 200;
}
