using Acropolis.Application.Events;
using Acropolis.Application.Mediator;
using Microsoft.Extensions.Options;

namespace Acropolis.Application.PageScraper;
public class ScrapeRequestTranslator : IRequestCommandTranslator
{
    private readonly ScrapeSettings scrapeSettings;

    public ScrapeRequestTranslator(IOptions<ScrapeSettings> options)
    {
        this.scrapeSettings = options.Value;
    }

    public bool CanHandle(RequestReceived request)
    {
        return Uri.TryCreate(request.Request.Message, new UriCreationOptions(), out Uri? uri) &&
            !scrapeSettings.IgnoredHosts.Contains(uri.Host);
    }

    public ICommandBase CreateCommand(RequestReceived request)
    {
        return new ScrapePageCommand(request.Id, request.Request.Message);
    }
}
