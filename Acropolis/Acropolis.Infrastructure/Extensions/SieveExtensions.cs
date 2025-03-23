using Sieve.Models;

namespace Acropolis.Infrastructure.Extensions;

public static class SieveExtensions
{
    public static SieveModel GetSieveModel(this Shared.Models.Sieve sieve) => new()
    {
        Page = sieve.Page,
        PageSize = sieve.PageSize,
        Filters = sieve.Filters,
        Sorts = sieve.Sorts
    };
}