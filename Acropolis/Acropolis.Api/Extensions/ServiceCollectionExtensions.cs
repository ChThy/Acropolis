using Acropolis.Application.Events;
using Acropolis.Application.Sagas;
using Acropolis.Infrastructure.EfCore;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite("Data Source=Acropolis_Messenger.db;cache=shared");
        });

        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(VideoDownloadRequested).Assembly);
            x.AddSagaStateMachines(typeof(DownloadVideoSaga).Assembly);

            x.AddSagaRepository<DownloadVideoState>().EntityFrameworkRepository(r =>
            {
                r.ExistingDbContext<AppDbContext>();
                r.LockStatementProvider = new SqliteLockStatementProvider();
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
