using Acropolis.Api.Extensions;
using Acropolis.Api.HostedServices;
using Acropolis.Api.Models;
using Acropolis.Application.Events;
using Acropolis.Application.Extensions;
using Acropolis.Infrastructure.Telegram.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Acropolis.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHostedService<DatabaseMigrator>();

        //builder.Services.AddPersistence(builder.Configuration);

        builder.Services.AddServices(builder.Configuration);
        builder.Services.AddApplicationServices(builder.Configuration);

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapPost("videos/download", async (
            [FromServices] IBus bus,
            [FromBody] DownloadVideoRequest request,
            CancellationToken cancellationToken) =>
        {
            await bus.Publish(new UrlRequestReceived(Guid.NewGuid(), request.Url, DateTimeOffset.UtcNow),
                ctx => ctx.CorrelationId = NewId.NextGuid(), cancellationToken);
            return Results.Accepted();
        });

        app.Run();
    }
}
