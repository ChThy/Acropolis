using Sieve.Models;

namespace Acropolis.Api.Models;

public record Sieve
{
    public int? Page { get; init; }
    public int? PageSize { get; init; }
    public string? Filters { get; init; }
    public string? Sorts { get; init; }

    public SieveModel GetSieveModel() => new()
    {
        Page = Page,
        PageSize = PageSize,
        Filters = Filters,
        Sorts = Sorts
    };
}
