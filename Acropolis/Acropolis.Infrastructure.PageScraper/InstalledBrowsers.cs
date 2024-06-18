namespace Acropolis.Infrastructure.PageScraper;

public sealed class InstalledBrowsers
{
    private readonly HashSet<string> executablePaths = [];

    public void AddBrowserPath(string path)
    {
        executablePaths.Add(path);
    }

    public string[] GetExecutablePaths()
    {
        return executablePaths.ToArray();
    }
}