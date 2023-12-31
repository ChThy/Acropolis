
using Acropolis.Api.HostedServices;
using Acropolis.Application.Extensions;
using Acropolis.Infrastructure.Dapr;
using Acropolis.Infrastructure.EfCore.Extensions;
using Acropolis.Infrastructure.EfCore.Messenger;
using Acropolis.Infrastructure.Telegram.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Acropolis.Infrastructure.Dapr.Extensions;

namespace Acropolis.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHostedService<DatabaseMigrator>();
        builder.Services.AddHostedService<MessageConsumer>();

        builder.Services.AddPersistence(builder.Configuration);
        builder.Services.AddTelegramMessenger(builder.Configuration);
        builder.Services.AddDaprInfrastructure(builder.Configuration);

        builder.Services.AddApplicationServices(builder.Configuration);

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseEventSupscriptionEndpoints();

        app.UseAuthorization();

        app.MapGet("incoming-requests", async ([FromServices] MessengerDbContext context) =>
        {
            var result = await context.IncomingRequests.ToListAsync();
            return Results.Ok(result);
        });

        app.Run();
    }
}
