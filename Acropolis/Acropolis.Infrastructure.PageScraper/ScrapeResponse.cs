namespace Acropolis.Infrastructure.PageScraper;

public record ScrapeResponse(string PageTitle, string Domain, string DocumentName, Stream Document);