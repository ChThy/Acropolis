
using Acropolis.Api.HostedServices;
using Acropolis.Application.Extensions;
using Acropolis.Domain;
using Acropolis.Infrastructure.EfCore.Extensions;
using Acropolis.Infrastructure.EfCore.Messenger;
using Acropolis.Infrastructure.Telegram.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Acropolis.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHostedService<DatabaseMigrator>();

        builder.Services.AddPersistence(builder.Configuration);
        //builder.Services.AddTelegramMessenger(builder.Configuration);

        builder.Services.AddApplicationServices(builder.Configuration);

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGet("incoming-requests", async ([FromServices] MessengerDbContext context) =>
        {
            var result = await context.IncomingRequests.ToListAsync();
            return Results.Ok(result);
        });

        app.MapGet("unprocessed-requests", async ([FromServices] MessengerDbContext context) =>
        {
            var result = await context.IncomingRequests
                .Where(e => e.ProcessedOn == null)
                .ToListAsync();
            return Results.Ok(result);
        });

        app.MapPost("reprocess-unprocessed", async ([FromServices] MessengerDbContext context, [FromServices] IRequestProcessor requestProcessor, CancellationToken cancel) =>
        {
            var result = await context.IncomingRequests
                .Where(e => e.ProcessedOn == null)
                .ToListAsync();

            foreach (var request in result)
            {
                await requestProcessor.Process(request, cancel);
            }
        });

        app.Run();
    }
}
