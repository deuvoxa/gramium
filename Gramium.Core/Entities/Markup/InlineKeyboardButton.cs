using System.Text.Json.Serialization;

namespace Gramium.Core.Entities.Markup;

public class InlineKeyboardButton(string text)
{
    [JsonPropertyName("text")] public string Text { get; set; } = text;

    [JsonPropertyName("callback_data")] public string? CallbackData { get; set; }

    [JsonPropertyName("url")] public string? Url { get; set; }

    public static InlineKeyboardButton WithCallbackData(string text, string callbackData)
    {
        return new InlineKeyboardButton(text) { CallbackData = callbackData };
    }

    public static InlineKeyboardButton WithUrl(string text, string url)
    {
        return new InlineKeyboardButton(text) { Url = url };
    }
}