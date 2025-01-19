using Microsoft.Extensions.DependencyInjection;

namespace Acropolis.Shared.Queries;

internal abstract class QueryHandlerWrapper<TResult>
{
    public abstract Task<TResult> Handle(IQuery<TResult> query, CancellationToken cancellationToken = default);
}

internal class QueryHandlerWrapper<TQuery, TResult>(IServiceProvider serviceProvider) : QueryHandlerWrapper<TResult>
    where TQuery : IQuery<TResult>
{
    public override Task<TResult> Handle(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var queryHandler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return queryHandler.Handle((TQuery)query, cancellationToken);
    }
}