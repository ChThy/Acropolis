using Acropolis.Domain.Repositories;
using MediatR;

namespace Acropolis.Application.YoutubeDownloader;

public class RetryFailedDownloadsHandler : IRequestHandler<RetryFailedDownloadsCommand>
{
    private readonly IYoutubeService youtubeService;
    private readonly IIncomingRequestRepostory incomingRequestRepostory;

    public RetryFailedDownloadsHandler(IYoutubeService youtubeService, IIncomingRequestRepostory incomingRequestRepostory)
    {
        this.youtubeService = youtubeService;
        this.incomingRequestRepostory = incomingRequestRepostory;
    }

    public async Task Handle(RetryFailedDownloadsCommand command, CancellationToken cancellationToken = default)
    {
        await youtubeService.RetryFailedDownloads();
        await incomingRequestRepostory.Update(command.IncomingRequestId, req => req.MarkAsProcessed(DateTimeOffset.UtcNow));
    }
}
