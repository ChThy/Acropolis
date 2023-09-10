using Acropolis.Application.Mediator;

namespace Acropolis.Application.YoutubeDownloader;

public record DownloadYoutubeVideo(string Url) : ICommand<bool>;

public class DownloadYoutubeVideoHandler : ICommandHandler<DownloadYoutubeVideo, bool>
{
    public ValueTask<bool> Handle(DownloadYoutubeVideo command, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(true);
    }
}
