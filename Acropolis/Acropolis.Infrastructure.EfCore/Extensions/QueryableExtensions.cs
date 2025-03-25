using Acropolis.Infrastructure.Extensions;
using Acropolis.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace Acropolis.Infrastructure.EfCore.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<TResult>> AsPagedResult<TResult>(
        this IQueryable<TResult> source,
        ISieveProcessor sieveProcessor,
        Shared.Models.Sieve sieve,
        CancellationToken cancellationToken = default)
    {
        var sieveModel = sieve.GetSieveModel();
        var query = source;
        query = sieveProcessor.Apply(sieveModel, query, applyPagination: false);
        var total = await query.CountAsync(cancellationToken);
        query = sieveProcessor.Apply(sieveModel, query, applyFiltering: false, applySorting: false);
        var result = await query.ToArrayAsync(cancellationToken);

        return new(sieveModel.Page ?? 1, sieveModel.PageSize ?? int.MaxValue, total, result);
    }
}