using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Acropolis.Shared.Queries;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryHandling(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.TryAddScoped<IQueryHandler, QueryHandler>();
        services.AddScoped(typeof(QueryHandlerWrapper<,>));

        var queryHandlers = assemblies.SelectMany(a => a.GetTypes())
            .OpenGenericsFor(typeof(IQueryHandler<,>))
            .ToArray();

        foreach (var queryHandler in queryHandlers)
        {
            services.AddScoped(queryHandler.ServiceType, queryHandler.ImplementationType);
        }

        return services;
    }
}