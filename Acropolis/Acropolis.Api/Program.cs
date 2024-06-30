using Acropolis.Api.Extensions;
using Acropolis.Api.HostedServices;
using Acropolis.Api.MigrateOldData;
using Acropolis.Api.Models;
using Acropolis.Application.Events;
using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Sagas.DownloadVideo;
using Acropolis.Infrastructure.EfCore;
using MassTransit;
using MassTransit.Internals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YoutubeExplode;

namespace Acropolis.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHostedService<DatabaseMigrator>();

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddServices(builder.Configuration);

        builder.Services.AddYoutubeDownloaderDataMigration(builder.Configuration);

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapPost("download", async (
            [FromServices] IBus bus,
            [FromBody] DownloadVideoRequest request,
            CancellationToken cancellationToken) =>
        {
            await bus.Publish(new UrlRequestReceived(Guid.NewGuid(), request.Url, DateTimeOffset.UtcNow),
                ctx => ctx.CorrelationId = NewId.NextGuid(), cancellationToken);
            return Results.Accepted();
        });

        app.MapPost("migratevideos", async (
            int take,
            int skip,
            YoutubeDownloaderDbContext youtubeDownloaderDbContext,
            AppDbContext dbContext,
            YoutubeClient youtubeClient,
            IBus bus,
            ILogger<YoutubeClient> logger) =>
        {
            List<string> notFound = [];

            var videos = await youtubeDownloaderDbContext
                .Set<YoutubeDownload>()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            foreach (var video in videos.Where(e => e.Request is not null && e.Video is not null))
            {
                var videoState = new DownloadVideoState()
                {
                    CorrelationId = video.Id,
                    CurrentState = "Downloaded",
                    RequestedTimestamp =  video.Request!.Timestamp,
                    DownloadedTimestamp = video.Request.Timestamp,
                    Url = $"https://www.youtube.com/watch?v={video.Request.VideoId}",
                    VideoMetaData = new(video.Request.VideoId, video.Video!.Title, video.Video.Author, video.Video.UploadTimeStamp, video.Video.StorageLocation)
                };
                dbContext.Add(videoState);
            }

            await dbContext.SaveChangesAsync();
            
            foreach (var video in videos.Where(e => e.Request is null))
            {
                if (video.Video?.Title is null)
                {
                    throw new InvalidOperationException($"Cannot find author and title for video with id: {video.Id}");
                }

                logger.LogInformation("Searching for {author} {title}", video.Video.Author, video.Video.Title);
                try
                {
                    var results = await youtubeClient.Search
                        .GetVideosAsync($"{video.Video.Author} {video.Video.Title}").ToListAsync();

                    var match = results.FirstOrDefault(e => e.Author.ChannelTitle == video.Video.Author && e.Title == video.Video.Title)?
                        .Url;

                    if (match is null)
                    {
                        notFound.Add(video.Video.ToString());
                        logger.LogWarning("Nothing found for {video}", video.Video.ToString());
                        continue;
                    }

                    logger.LogInformation("Match: {match}", match);
                    await bus.Publish(new UrlRequestReceived(video.Id, match, DateTimeOffset.UtcNow));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "While checking {video}", video.Video.Author + " " + video.Video.Title);
                    notFound.Add(video.Video.ToString());
                    logger.LogWarning("Nothing found for {video}", video.Video.ToString());
                }
            }

            foreach (var video in videos.Where(e => e.Request is not null && e.Video is null))
            {
                await bus.Publish(new UrlRequestReceived(video.Id, $"https://www.youtube.com/watch?v={video.Request.VideoId}", DateTimeOffset.UtcNow));
            }

            return Results.Ok(notFound);
            // http://www.youtube.com/watch?v=<VideoId>
        });

        app.Run();
    }
}