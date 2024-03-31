using Acropolis.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Acropolis.Application.YoutubeDownloader;

public class DownloadYoutubeVideoHandler : IRequestHandler<DownloadYoutubeVideoCommand>
{
    private readonly IYoutubeService youtubeService;
    private readonly IIncomingRequestRepostory incomingRequestRepostory;
    private readonly ILogger<DownloadYoutubeVideoHandler> logger;

    public DownloadYoutubeVideoHandler(
        IYoutubeService youtubeService,
        IIncomingRequestRepostory incomingRequestRepostory,
        ILogger<DownloadYoutubeVideoHandler> logger)
    {
        this.youtubeService = youtubeService;
        this.incomingRequestRepostory = incomingRequestRepostory;
        this.logger = logger;
    }

    public async Task Handle(DownloadYoutubeVideoCommand command, CancellationToken cancellationToken = default)
    {
        var externalId = await youtubeService.Download(command.Url);
        logger.LogDebug("Download video external Id: {id}", externalId);

        await incomingRequestRepostory.Update(
            command.IncomingRequestId,
            req =>
            {
                req.AcceptedByExternalSystem(externalId, DateTimeOffset.UtcNow);
            });
    }
}
