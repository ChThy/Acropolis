using Acropolis.Application.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Acropolis.Application.EventHandlers;

public class VideoAlreadyDownloadedHandler(ILogger<VideoAlreadyDownloadedHandler> logger)
    : IConsumer<VideoAlreadyDownloaded>
{
    private readonly ILogger<VideoAlreadyDownloadedHandler> logger = logger;

    public Task Consume(ConsumeContext<VideoAlreadyDownloaded> context)
    {
        logger.LogInformation("Video already downloaded: {message}", context.Message.ToString());
        return Task.CompletedTask;
    }
}
