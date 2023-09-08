using Acropolis.Infrastructure.EfCore.Messenger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acropolis.Infrastructure.EfCore.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<MessengerDbContext>((_, options) =>
        {
            options.UseSqlite(configuration.GetConnectionString("MessengerDatabase"));
        });

        

        return services;
    }
}
