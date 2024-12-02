using System.Text.Json.Serialization;

namespace Gramium.Core.Entities.Markup;

public class InlineKeyboardMarkup(InlineKeyboardButton[][] inlineKeyboard) : IReplyMarkup
{
    [JsonPropertyName("inline_keyboard")]
    [JsonInclude]
    public InlineKeyboardButton[][] InlineKeyboard { get; init; } = inlineKeyboard ?? throw new ArgumentNullException(nameof(inlineKeyboard));

    public static InlineKeyboardMarkup Create(params InlineKeyboardButton[] buttons)
    {
        return new InlineKeyboardMarkup([buttons]);
    }
}