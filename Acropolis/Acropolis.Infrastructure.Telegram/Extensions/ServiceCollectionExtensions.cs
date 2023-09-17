using Acropolis.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Acropolis.Infrastructure.Telegram.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramMessenger(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<TelegramOptions>(configuration, TelegramOptions.Name);
        services.AddSingleton<TelegramBotClient>(sp =>
        {
            var telegramOptions = sp.GetOptions<TelegramOptions>();
            return new TelegramBotClient(telegramOptions.UserToken);
        });

        services.AddHostedService<MessageReceiver>();

        return services;
    }
}
