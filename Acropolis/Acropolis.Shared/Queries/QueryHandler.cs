using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Acropolis.Shared.Queries;

public sealed class QueryHandler(IServiceProvider serviceProvider) : IQueryHandler
{
    private static readonly ConcurrentDictionary<Type, Type> TypeCache = [];

    public Task<TResult> Handle<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var wrapperType = TypeCache.GetOrAdd(query.GetType(), queryType => typeof(QueryHandlerWrapper<,>).MakeGenericType(queryType, typeof(TResult)));

        var handler = serviceProvider.GetRequiredService(wrapperType) as QueryHandlerWrapper<TResult>;
        return handler!.Handle(query, cancellationToken);
    }
}