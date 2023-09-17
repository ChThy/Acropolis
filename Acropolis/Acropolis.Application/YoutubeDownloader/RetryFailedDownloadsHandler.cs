using Acropolis.Application.Mediator;
using Acropolis.Domain.Repositories;

namespace Acropolis.Application.YoutubeDownloader;

public class RetryFailedDownloadsHandler : ICommandHandler<RetryFailedDownloadsCommand>
{
    private readonly IYoutubeService youtubeService;
    private readonly IIncomingRequestRepostory incomingRequestRepostory;

    public RetryFailedDownloadsHandler(IYoutubeService youtubeService, IIncomingRequestRepostory incomingRequestRepostory)
    {
        this.youtubeService = youtubeService;
        this.incomingRequestRepostory = incomingRequestRepostory;
    }

    public async ValueTask Handle(RetryFailedDownloadsCommand command, CancellationToken cancellationToken = default)
    {
        await youtubeService.RetryFailedDownloads();
        await incomingRequestRepostory.Update(command.IncomingRequestId, req => req.MarkAsProcessed(DateTimeOffset.UtcNow));
    }
}
