using Gramium.Core.Entities.Markup;

namespace Gramium.Framework.Markup;

public class InlineKeyboardBuilder
{
    private readonly List<List<InlineKeyboardButton>> _rows = [];
    private List<InlineKeyboardButton> _currentRow = [];

    public InlineKeyboardBuilder AddButton(string text, string callbackData)
    {
        _currentRow.Add(new InlineKeyboardButton(text) { CallbackData = callbackData });
        return this;
    }

    public InlineKeyboardBuilder AddButtonRow(string text, string callbackData)
    {
        if (_currentRow.Count > 0)
        {
            _rows.Add(_currentRow);
            _currentRow = [];
        }
        
        _rows.Add([new InlineKeyboardButton(text) { CallbackData = callbackData }]);
        return this;
    }

    public InlineKeyboardBuilder AddRow()
    {
        if (_currentRow.Count <= 0) return this;
        _rows.Add(_currentRow);
        _currentRow = [];
        return this;
    }

    public InlineKeyboardMarkup Build()
    {
        if (_currentRow.Count > 0)
        {
            _rows.Add(_currentRow);
        }
        
        return new InlineKeyboardMarkup(_rows.Select(row => row.ToArray()).ToArray());
    }
}