namespace Gramium.Client;

public class TelegramClientOptions
{
    public string Token { get; set; } = default!;
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
    private string BaseUrl { get; set; } = "https://api.telegram.org";

    internal string GetBaseUrl() => 
        $"{BaseUrl.TrimEnd('/')}/bot{Token}/";
}