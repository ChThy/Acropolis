using Acropolis.Application.Mediator;
using Microsoft.Extensions.Options;

namespace Acropolis.Application.YoutubeDownloader;

public class YoutubeRequestTranslator : IRequestCommandTranslator
{
    private readonly YoutubeSettings youtubeSettings;

    public YoutubeRequestTranslator(IOptions<YoutubeSettings> options)
    {
        youtubeSettings = options.Value;
    }

    public bool CanHandle(string request, Dictionary<string, string>? param = null)
    {
        if (request is null)
            return false;
        return IsRetryRequest(request) || youtubeSettings.ValidUrls.Any(e =>
            request.StartsWith(e, StringComparison.InvariantCultureIgnoreCase));
    }

    private static bool IsRetryRequest(string request) => request.Equals("retry", StringComparison.OrdinalIgnoreCase);

    public ICommandBase CreateCommand(string request, Dictionary<string, string>? param = null)
    {
        if (IsRetryRequest(request))
        {
            return new RetryFailedDownloads();
        }
        else
        {
            return new DownloadYoutubeVideo(request);
        }
    }
}