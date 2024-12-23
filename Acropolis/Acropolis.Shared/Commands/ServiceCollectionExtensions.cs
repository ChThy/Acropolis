using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Acropolis.Shared.Commands;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandHandling(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.TryAddScoped<ICommandHandler, CommandHandler>();

        var commandHandlers = assemblies.SelectMany(a => a.GetTypes())
            .OpenGenericsFor(typeof(ICommandHandler<>))
            .ToArray();

        foreach (var commandHandler in commandHandlers)
        {
            services.AddScoped(commandHandler.ServiceType, commandHandler.ImplementationType);
        }

        return services;
    }
}