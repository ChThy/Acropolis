using Acropolis.Application.Mediator;

namespace Acropolis.Application.YoutubeDownloader;

public record RetryFailedDownloads() : ICommand;

public class RetryFailedDownloadsHandler : ICommandHandler<RetryFailedDownloads>
{
    private readonly IYoutubeService youtubeService;

    public RetryFailedDownloadsHandler(IYoutubeService youtubeService)
    {
        this.youtubeService = youtubeService;
    }

    public async ValueTask Handle(RetryFailedDownloads command, CancellationToken cancellationToken = default)
    {
        await youtubeService.RetryFailedDownloads();
    }
}
