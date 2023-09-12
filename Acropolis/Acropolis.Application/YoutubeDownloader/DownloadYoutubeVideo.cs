using Acropolis.Application.Mediator;

namespace Acropolis.Application.YoutubeDownloader;

public record DownloadYoutubeVideo(string Url) : ICommand;

public class DownloadYoutubeVideoHandler : ICommandHandler<DownloadYoutubeVideo>
{
    private readonly IYoutubeService youtubeService;

    public DownloadYoutubeVideoHandler(IYoutubeService youtubeService)
    {
        this.youtubeService = youtubeService;
    }

    public async ValueTask Handle(DownloadYoutubeVideo command, CancellationToken cancellationToken = default)
    {
        await youtubeService.Download(command.Url);
    }
}
