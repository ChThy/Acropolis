using Acropolis.Application.DownloadedVideos.CreateDownloadedVideo;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Application.Sagas.ExternalMessageRequest;
using Acropolis.Application.Sagas.ScrapePage;
using Acropolis.Application.Services;
using Acropolis.Application.Shared;
using Acropolis.Infrastructure.EfCore;
using Acropolis.Infrastructure.EfCore.CommandHandlers;
using Acropolis.Infrastructure.EfCore.QueryHandlers;
using Acropolis.Infrastructure.Extensions;
using Acropolis.Infrastructure.PageScraper.EventHandlers;
using Acropolis.Infrastructure.PageScraper.Extensions;
using Acropolis.Infrastructure.Telegram;
using Acropolis.Infrastructure.Telegram.Extensions;
using Acropolis.Infrastructure.Telegram.Messenger;
using Acropolis.Infrastructure.YoutubeDownloader.EventHandlers;
using Acropolis.Infrastructure.YoutubeDownloader.Extensions;
using Acropolis.Shared.Commands;
using Acropolis.Shared.Extensions;
using Acropolis.Shared.Queries;
using MassTransit;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Acropolis.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.TryAddSingleton(TimeProvider.System);
        services.AddHttpClient();

        services.AddDbContextFactory<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
            // options.UseSqlite(configuration.GetConnectionString("Database"));
        });

        services.AddTransient<ProcessService>();
        
        services.AddInfrastructure(configuration);
        services.AddTelegramMessenger(configuration);
        services.AddYoutubeDownloaderServices(configuration);
        services.AddPageScraper(configuration);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(typeof(CreateDownloadedVideoRequest).Assembly);
            config.AddRequestPostProcessor(typeof(IRequestPostProcessor<,>), typeof(SaveChangesPostProcessor<,>), ServiceLifetime.Scoped);
        });

        services.AddQueryHandling(typeof(DownloadedVideosQueryHandler).Assembly);
        services.AddCommandHandling(typeof(SaveChangesCommandHandler).Assembly);

        var telegramStartupOptions = configuration.GetOptions<TelegramOptions>(TelegramOptions.Name);
        services.AddMassTransit(x =>
        {
            x.AddConsumers(
                typeof(VideoDownloadedConsumer).Assembly,
                typeof(VideoDownloadRequestedHandler).Assembly,
                typeof(PageScrapeRequestedHandler).Assembly);

            if (telegramStartupOptions.Enabled)
            {
                x.AddConsumers(typeof(ExternalMessageReplyRequestedHandler).Assembly);
            }

            x.AddSagaStateMachines(typeof(DownloadVideoSaga).Assembly);

            x.AddSagaRepository<DownloadVideoState>().EntityFrameworkRepository(r =>
            {
                r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                r.ExistingDbContext<AppDbContext>();
            });
            x.AddSagaRepository<ScrapePageState>().EntityFrameworkRepository(r =>
            {
                r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                r.ExistingDbContext<AppDbContext>();
            });
            x.AddSagaRepository<ExternalMessageRequestState>().EntityFrameworkRepository(r =>
            {
                r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                r.ExistingDbContext<AppDbContext>();
            });

            // x.AddEntityFrameworkOutbox<AppDbContextSqlite>(o =>
            // {
            //     o.UseSqlite();
            //     o.UseBusOutbox();
            // });
            //
            // x.AddConfigureEndpointsCallback((ctx, name, cfg) =>
            // {
            //     cfg.UseEntityFrameworkOutbox<AppDbContextSqlite>(ctx);
            // });
            
            x.AddConfigureEndpointsCallback((endpoint,cfg) =>
            {
                cfg.ConcurrentMessageLimit = 1;

                if (environment.IsProduction())
                {
                    cfg.UseMessageRetry(r => r.Immediate(0));
                    // cfg.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(60), TimeSpan.FromSeconds(20)));
                }
                else
                {
                    cfg.UseMessageRetry(r => r.Immediate(0));
                }
            });

            x.UsingRabbitMq((context, config) =>
            {
                var username = configuration.GetValue<string>("RabbitMq:User");
                var password = configuration.GetValue<string>("RabbitMq:Password");
                var host = configuration.GetValue<string>("RabbitMq:Host");
                var virtualHost = configuration.GetValue<string>("RabbitMq:VirtualHost");
                
                config.Host(host, virtualHost, r =>
                {
                    r.Username(username!);
                    r.Password(password!);
                });
                config.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}