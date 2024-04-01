namespace Acropolis.Infrastructure.FileStorages;

public interface IFileStorage
{
    ValueTask<string> StoreFile(string fileName, Stream stream, CancellationToken cancellationToken = default);
}