namespace Acropolis.Application.PageScraper;

public interface IScrapeService
{
    ValueTask<Guid> Download(string url);
}
