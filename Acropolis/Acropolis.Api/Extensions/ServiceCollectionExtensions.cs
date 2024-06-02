using Acropolis.Application.EventHandlers;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Application.Sagas.ExternalMessageRequest;
using Acropolis.Infrastructure.EfCore;
using Acropolis.Infrastructure.Extensions;
using Acropolis.Infrastructure.Telegram.Extensions;
using Acropolis.Infrastructure.Telegram.Messenger;
using Acropolis.Infrastructure.YoutubeDownloader.EventHandlers;
using Acropolis.Infrastructure.YoutubeDownloader.Extensions;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Acropolis.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton(TimeProvider.System);
        services.AddHttpClient();

        services.AddDbContextFactory<AppDbContext>(options =>
        {
            options.UseSqlite("Data Source=Acropolis_Messenger.db;cache=shared");
        });

        services.AddInfrastructure(configuration);
        services.AddTelegramMessenger(configuration);
        services.AddYoutubeDownloaderServices(configuration);

        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(VideoDownloadAlreadyRequestedHandler).Assembly, 
                typeof(VideoDownloadRequestedHandler).Assembly,
                typeof(ExternalMessageReplyRequestedHandler).Assembly);
            x.AddSagaStateMachines(typeof(DownloadVideoSaga).Assembly);

            x.AddSagaRepository<DownloadVideoState>().EntityFrameworkRepository(r =>
            {
                r.ConcurrencyMode = ConcurrencyMode.Optimistic;

                r.ExistingDbContext<AppDbContext>();
                r.LockStatementProvider = new SqliteLockStatementProvider();
            });
            x.AddSagaRepository<ExternalMessageRequestState>().EntityFrameworkRepository(r =>
            {
                r.ConcurrencyMode = ConcurrencyMode.Optimistic;

                r.ExistingDbContext<AppDbContext>();
                r.LockStatementProvider = new SqliteLockStatementProvider();
            });

            x.AddEntityFrameworkOutbox<AppDbContext>(o =>
            {
                o.UseSqlite();
                o.UseBusOutbox();
                o.LockStatementProvider = new SqliteLockStatementProvider();
            });

            x.AddConfigureEndpointsCallback((ctx, name, cfg) =>
            {
                cfg.UseEntityFrameworkOutbox<AppDbContext>(ctx);
            });

            x.UsingRabbitMq((context, config) =>
            {
                var username = configuration.GetValue<string>("RabbitMq:User");
                var password = configuration.GetValue<string>("RabbitMq:Password");

                config.Host("localhost", "/", r =>
                {
                    r.Username(username);
                    r.Password(password);
                });
                config.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}