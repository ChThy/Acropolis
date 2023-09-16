using Acropolis.Application.Events;
using Acropolis.Application.Mediator;

namespace Acropolis.Application.PageScraper;
public class ScrapeRequestTranslator : IRequestCommandTranslator
{
    public bool CanHandle(RequestReceived request)
    {
        return Uri.TryCreate(request.Request.Message, new UriCreationOptions(), out Uri? _);
    }

    public ICommandBase CreateCommand(RequestReceived request)
    {
        return new ScrapePageCommand(request.Id, request.Request.Message);
    }
}
