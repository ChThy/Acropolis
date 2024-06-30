using System.Text;

namespace Acropolis.Infrastructure.Helpers;
public static class PathHelpers
{
    public static char[] InvalidFileNameCharacters { get; } = Path.GetInvalidFileNameChars();
    public static char[] InvalidPathCharacters { get; } = Path.GetInvalidPathChars();

    public static string RemoveInvalidFileNameChars(this string filename)
    {
        var sb = new StringBuilder(filename);
        foreach (var character in InvalidFileNameCharacters)
        {

            sb.Replace(character.ToString(), "");
        }
        return sb.ToString();
    }

    public static string RemoveInvalidDirectoryChars(this string path)
    {
        var sb = new StringBuilder(path);
        foreach (var character in InvalidPathCharacters)
        {
            sb.Replace(character.ToString(), "");
        }
        return sb.ToString();
    }

    public static string RemoveInvalidFilePathChars(this string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        var fileName = Path.GetFileName(filePath);
        return Path.Combine(directory?.RemoveInvalidDirectoryChars() ?? "", fileName?.RemoveInvalidFileNameChars() ?? "");
    }
}
