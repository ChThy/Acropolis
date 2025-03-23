namespace Acropolis.Api.Models;

public record PagedResult<TResult>(int Page, int PageSize, int Total, TResult[] Result);