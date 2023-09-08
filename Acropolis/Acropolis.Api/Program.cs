
using Acropolis.Api.HostedServices;
using Acropolis.Infrastructure.EfCore.Extensions;
using Acropolis.Infrastructure.Telegram.Extensions;

namespace Acropolis.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHostedService<DatabaseMigrator>();

        builder.Services.AddPersistence(builder.Configuration);
        builder.Services.AddTelegramMessenger(builder.Configuration);

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.Run();
    }
}
