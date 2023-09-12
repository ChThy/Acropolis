
using Acropolis.Api.HostedServices;
using Acropolis.Application.Extensions;
using Acropolis.Application.Mediator;
using Acropolis.Application.Messenger;
using Acropolis.Application.YoutubeDownloader;
using Acropolis.Infrastructure.EfCore.Extensions;
using Acropolis.Infrastructure.Telegram.Extensions;
using Microsoft.AspNetCore.Mvc;

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

        builder.Services.AddApplicationServices(builder.Configuration);

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGet("youtube-download", async ([FromServices] IMediator mediator, string url) =>
        {
            var command = new DownloadYoutubeVideo(url);
            await mediator.Send(command);
        });

        app.MapPost("send-message", async ([FromServices] IMediator mediator, string message, string target) =>
        {
            await mediator.Send(new SendMessage(message, target));
        });

        app.Run();
    }
}
