using System.Web;

namespace Acropolis.Infrastructure.YoutubeDownloader.Helpers;
public static class VideoIdExtractor
{
    private const string VideoIdQueryParameterName = "v";
    public static string ExtractVideoId(string url)
    {
        var uri = new Uri(url);
        string? videoId = HasVideoIdQueryParameter(uri)
            ? ExtractVideoIdFromQueryParameter(uri)
            : ExtractVideoIdFromPath(uri);

        return string.IsNullOrWhiteSpace(videoId)
            ? throw new ArgumentException($"Url {url} does not contain a valid videoId")
            : videoId;
    }

    private static bool HasVideoIdQueryParameter(Uri uri)
    {
        return uri.Query.Contains($"?{VideoIdQueryParameterName}=", StringComparison.InvariantCultureIgnoreCase) ||
            uri.Query.Contains($"&{VideoIdQueryParameterName}=", StringComparison.InvariantCultureIgnoreCase);
    }

    private static string? ExtractVideoIdFromQueryParameter(Uri uri) =>
        HttpUtility.ParseQueryString(uri.Query).Get(VideoIdQueryParameterName);

    private static string? ExtractVideoIdFromPath(Uri uri) => uri.Segments.LastOrDefault();
}
