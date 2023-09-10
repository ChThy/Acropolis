using Acropolis.Application.Mediator;
using Acropolis.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Acropolis.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<YoutubeSettings>(configuration, YoutubeSettings.Name);

        services.AddTransient<IMediator, Mediator.Mediator>();
        services.AddCommandHandlers();

        return services;
    }

    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.RegisterScopedOpenGenerics(typeof(ICommandHandler<>));
        services.RegisterScopedOpenGenerics(typeof(ICommandHandler<,>));
        return services;
    }

    private static IServiceCollection RegisterScopedOpenGenerics(this IServiceCollection services, Type openGenericType)
    {
        var openGenericTypes = Assembly.GetEntryAssembly()!
            .GetReferencedAssemblies()
            .SelectMany(e => Assembly.Load(e).GetTypes())
            .OpenGenericsFor(openGenericType);

        foreach (var (ServiceType, ImplementationType) in openGenericTypes)
        {
            services.TryAddScoped(ServiceType, ImplementationType);
        }

        return services;
    }
}
