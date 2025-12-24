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
        var safeFileName = LimitFilenameLength(fileName).RemoveInvalidFilePathChars();
        var filePath = Path.Combine(fileStorageOptions.BaseDirectory, safeFileName);
        logger.LogDebug("Storing file {file}", filePath);

        var directory = Path.GetDirectoryName(filePath);
        CreateDirectoryIfNeeded(directory!);

        await using var file = File.OpenWrite(filePath);
        await stream.CopyToAsync(file, cancellationToken);
        file.Close();

        return filePath;
    }

    public void DeleteFile(string fileName)
    {
        var filePath = Path.Combine(fileStorageOptions.BaseDirectory, fileName);
        logger.LogDebug("Deleting file {file}", filePath);

        if (!File.Exists(filePath))
        {
            logger.LogWarning("File {file} does nog exist at path {path}", filePath, filePath);
            return;
        }
        File.Delete(filePath);
        
    }

    private string LimitFilenameLength(string filename)
    {
        if (filename.Length <= fileStorageOptions.MaxFilenameLength)
        {
            return filename;
        }
        
        var extension = Path.GetExtension(filename);
        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
        return $"{filenameWithoutExtension.Substring(0, fileStorageOptions.MaxFilenameLength-extension.Length)}{extension}";
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