using Gramium.Core.Entities.Markup;

namespace Gramium.Framework.Markup;

public class InlineKeyboardBuilder
{
    private readonly List<List<InlineKeyboardButton>> _rows = [];
    private List<InlineKeyboardButton> _currentRow = [];

    public InlineKeyboardBuilder WithButton((string text, string callbackData) button)
    {
        _currentRow.Add(new InlineKeyboardButton(button.text) { CallbackData = button.callbackData });
        return this;
    }

    public InlineKeyboardBuilder WithButtonRow(string text, string callbackData)
    {
        if (_currentRow.Count > 0)
        {
            _rows.Add(_currentRow);
            _currentRow = [];
        }

        _rows.Add([new InlineKeyboardButton(text) { CallbackData = callbackData }]);
        return this;
    }

    public InlineKeyboardBuilder WithButtons(params (string Text, string CallbackData)[] buttons)
    {
        var row = buttons.Select(b => new InlineKeyboardButton(b.Text) { CallbackData = b.CallbackData }).ToList();
        _rows.Add(row);
        return this;
    }

    public InlineKeyboardBuilder WithRow()
    {
        if (_currentRow.Count <= 0) return this;
        _rows.Add(_currentRow);
        _currentRow = [];
        return this;
    }
    
    public InlineKeyboardBuilder WithButtonGrid(IEnumerable<(string text, string callbackData)> buttons, int buttonsPerRow = 3)
    {
        var buttonList = buttons
            .Select(b => new InlineKeyboardButton(b.text) { CallbackData = b.callbackData })
            .ToList();

        for (int i = 0; i < buttonList.Count; i += buttonsPerRow)
        {
            _rows.Add(buttonList.Skip(i).Take(buttonsPerRow).ToList());
        }

        return this;
    }

    public InlineKeyboardMarkup Build()
    {
        if (_currentRow.Count > 0) _rows.Add(_currentRow);

        return new InlineKeyboardMarkup(_rows.Select(row => row.ToArray()).ToArray());
    }
}