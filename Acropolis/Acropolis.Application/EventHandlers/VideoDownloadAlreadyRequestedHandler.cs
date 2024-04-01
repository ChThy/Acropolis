using Acropolis.Application.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Acropolis.Application.EventHandlers;
public class VideoDownloadAlreadyRequestedHandler(ILogger<VideoDownloadAlreadyRequestedHandler> logger) : 
    IConsumer<VideoDownloadAlreadyRequested>
{
    private readonly ILogger<VideoDownloadAlreadyRequestedHandler> logger = logger;

    public Task Consume(ConsumeContext<VideoDownloadAlreadyRequested> context)
    {
        logger.LogInformation("Video already requested to download: {message}", context.Message.ToString());
        return Task.CompletedTask;
    }
}
