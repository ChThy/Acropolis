namespace Acropolis.Shared.Queries;

public interface IQueryHandler
{
    Task<TResult> Handle<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default);
}