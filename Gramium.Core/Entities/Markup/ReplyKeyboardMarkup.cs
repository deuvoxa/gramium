namespace Gramium.Core.Entities.Markup;

public class ReplyKeyboardMarkup : IReplyMarkup
{
    public KeyboardButton[][] Keyboard { get; set; } = null!;
} 