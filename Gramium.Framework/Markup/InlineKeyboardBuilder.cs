using Gramium.Core.Entities.Markup;

namespace Gramium.Framework.Markup;

public class InlineKeyboardBuilder
{
    private readonly List<List<InlineKeyboardButton>> _rows = new();

    public InlineKeyboardBuilder AddButton(string text, string callbackData)
    {
        _rows.Add([new InlineKeyboardButton(text) { CallbackData = callbackData }]);
        return this;
    }

    public InlineKeyboardBuilder AddButtons(params (string Text, string CallbackData)[] buttons)
    {
        var row = buttons.Select(b => new InlineKeyboardButton(b.Text) { CallbackData = b.CallbackData }).ToList();
        _rows.Add(row);
        return this;
    }

    public InlineKeyboardMarkup Build()
        => new InlineKeyboardMarkup(_rows.Select(row => row.ToArray()).ToArray());
}