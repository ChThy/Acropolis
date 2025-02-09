namespace Acropolis.Infrastructure.Telegram;

public class TelegramOptions
{
    public const string Name = "Telegram";

    public bool Enabled { get; set; } = true;
    public string UserToken { get; set; } = "";
    public string[] ValidChannelTitles { get; set; } = [];
}
