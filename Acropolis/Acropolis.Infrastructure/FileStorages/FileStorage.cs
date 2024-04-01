using Acropolis.Infrastructure.Helpers;
using Acropolis.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acropolis.Infrastructure.FileStorages;
public sealed class FileStorage(IOptionsMonitor<FileStorageOptions> optionsMonitor, ILogger<FileStorage> logger) 
    : IFileStorage
{
    private readonly FileStorageOptions fileStorageOptions = optionsMonitor.CurrentValue;
    private readonly ILogger<FileStorage> logger = logger;

    public async ValueTask<string> StoreFile(string fileName, Stream stream, CancellationToken cancellationToken = default)
    {
        var safeFileName = fileName.RemoveInvalidFilePathChars();
        var filePath = Path.Combine(fileStorageOptions.BaseDirectory, safeFileName);
        logger.LogDebug("Storing file {file}", filePath);

        var directory = Path.GetDirectoryName(filePath);
        CreateDirectoryIfNeeded(directory!);

        var file = File.OpenWrite(filePath);
        await stream.CopyToAsync(file, cancellationToken);
        file.Close();

        return safeFileName;
    }

    private void CreateDirectoryIfNeeded(string directory)
    {
        if (!Directory.Exists(directory))
        {
            logger.LogDebug("Creating Directory {directory}", directory);
            Directory.CreateDirectory(directory);
        }
    }
}