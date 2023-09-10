namespace Acropolis.Application.Events;

public record DownloadYoutubeVideoRequestReceived(string Url) : Message;