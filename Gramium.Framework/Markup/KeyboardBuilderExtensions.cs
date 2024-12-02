using Gramium.Framework.Context;

namespace Gramium.Framework.Markup;

public static class KeyboardBuilderExtensions
{
    public static InlineKeyboardBuilder CreateKeyboard(this IMessageContext context)
    {
        return new InlineKeyboardBuilder();
    }
} 