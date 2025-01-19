using Acropolis.Application.Events.VideoDownloader;
using MassTransit;
using MediatR;

namespace Acropolis.Application.DownloadedVideos.CreateDownloadedVideo;

public class VideoDownloadedConsumer(IMediator mediator) : IConsumer<VideoDownloaded>
{
    public async Task Consume(ConsumeContext<VideoDownloaded> context)
    {
        await mediator.Send(new CreateDownloadedVideoRequest(context.Message), context.CancellationToken);
    }
}