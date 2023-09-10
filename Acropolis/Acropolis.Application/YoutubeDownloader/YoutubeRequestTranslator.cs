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
        return youtubeSettings.ValidUrls.Any(e =>
            request.StartsWith(e, StringComparison.InvariantCultureIgnoreCase));
    }

    public ICommandBase Command(string request, Dictionary<string, string>? param = null)
    {
        return new DownloadYoutubeVideo(request);
    }
}