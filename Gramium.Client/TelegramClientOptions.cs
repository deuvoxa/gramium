namespace Gramium.Client;

public class TelegramClientOptions
{
    private const string BaseUrl = "https://api.telegram.org";
    public string Token { get; set; } = default!;
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    internal string GetBaseUrl()
    {
        return $"{BaseUrl.TrimEnd('/')}/bot{Token}/";
    }
}