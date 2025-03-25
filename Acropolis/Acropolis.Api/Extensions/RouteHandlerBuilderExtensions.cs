using Microsoft.AspNetCore.Mvc;

namespace Acropolis.Api.Extensions;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder ProducesCommonResponses(this RouteHandlerBuilder builder)
    {
        builder.Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        builder.Produces<ProblemDetails>(StatusCodes.Status404NotFound);
        builder.Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        
        return builder;
    }
}