using Acropolis.Shared.Queries;

namespace Acropolis.Infrastructure.EfCore.QueryHandlers;

public abstract class QueryHandlerBase<TQuery, TResult, TDbContext>(TDbContext dbContext) : 
    IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    protected readonly TDbContext DbContext = dbContext;

    public abstract Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default);
    
    Task<TResult> IQueryHandler<TQuery, TResult>.Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        return Handle(query, cancellationToken);
    }
}