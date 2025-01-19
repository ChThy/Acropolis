using System.Web;

namespace Acropolis.Infrastructure.YoutubeDownloader.Helpers;
public static class VideoIdExtractor
{
    private const string VideoIdQueryParameterName = "v";
    public static string ExtractVideoId(Uri uri)
    {
        string? videoId = HasVideoIdQueryParameter(uri)
            ? ExtractVideoIdFromQueryParameter(uri)
            : ExtractVideoIdFromPath(uri);

        return string.IsNullOrWhiteSpace(videoId)
            ? throw new ArgumentException($"Url {uri} does not contain a valid videoId")
            : videoId;
    }

    private static bool HasVideoIdQueryParameter(Uri uri)
    {
        return uri.Query.Contains($"?{VideoIdQueryParameterName}=", StringComparison.InvariantCultureIgnoreCase) ||
            uri.Query.Contains($"&{VideoIdQueryParameterName}=", StringComparison.InvariantCultureIgnoreCase);
    }

    private static string? ExtractVideoIdFromQueryParameter(Uri uri)
    {
        var queryStrings = uri.Query.Split('?', StringSplitOptions.RemoveEmptyEntries);

        foreach (var queryStringPart in queryStrings)
        {
            var videoId = HttpUtility.ParseQueryString(queryStringPart)[VideoIdQueryParameterName];

            if (!string.IsNullOrWhiteSpace(videoId))
            {
                return videoId;
            }          
        }

        throw new InvalidOperationException($"Failed to extract videoId from query part for {uri}");
    }

    private static string? ExtractVideoIdFromPath(Uri uri) => uri.Segments.LastOrDefault();
}
