using System.Text;

namespace Acropolis.Infrastructure.Helpers;

public static class PathHelpers
{
    public static char[] InvalidFileNameCharacters { get; } =
    [
        '\'', '<', '>', '|', '\u0000', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\u0007', '\b', '\t', '\n', '\u000b', '\f', '\r', '\u000e', '\u000f', '\u0010', '\u0011', '\u0012',
        '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019', '\u001a', '\u001b', '\u001c', '\u001d', '\u001e', '\u001f', ':', '*', '?', '\\', '/'
    ];

    public static char[] InvalidPathCharacters { get; } =
    [
        '|', '\u0000', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\u0007', '\b', '\t', '\n', '\u000b', '\f', '\r', '\u000e', '\u000f', '\u0010', '\u0011', '\u0012', '\u0013',
        '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019', '\u001a', '\u001b', '\u001c', '\u001d', '\u001e', '\u001f'
    ];

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