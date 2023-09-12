using Acropolis.Application.Events.Infrastructure;
using Acropolis.Application.Mediator;
using Acropolis.Application.YoutubeDownloader;
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
        
        services.AddMediator();
        services.AddInMemoryMessageBus();

        services.AddScoped<IRequestCommandTranslator, YoutubeRequestTranslator>();
        services.AddHttpClient<IYoutubeService, YoutubeService>((sp, client) =>
        {
            var options = sp.GetOptions<YoutubeSettings>();
            client.BaseAddress = new Uri(options.YoutubeDownloaderEndpoint);
        });

        return services;
    }
        
    private static IServiceCollection AddInMemoryMessageBus(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryMessageBus>();
        services.AddSingleton<IMessagePublisher, InMemoryMessageBus>(sp => sp.GetRequiredService<InMemoryMessageBus>());
        services.AddSingleton<IMessageSubscriber, InMemoryMessageBus>(sp => sp.GetRequiredService<InMemoryMessageBus>());

        return services;
    }

    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddTransient<IMediator, Mediator.Mediator>();
        services.AddCommandHandlers();
        return services;
    }

    private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
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
