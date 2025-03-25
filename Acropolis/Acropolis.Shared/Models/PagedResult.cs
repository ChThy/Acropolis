namespace Acropolis.Shared.Models;

public record PagedResult<TResult>(int Page, int PageSize, int Total, TResult[] Result);