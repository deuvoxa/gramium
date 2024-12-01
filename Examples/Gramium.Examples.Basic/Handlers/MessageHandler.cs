using Gramium.Framework.Context;
using Gramium.Core.Entities.Markup;
using Gramium.Framework.Interfaces;

namespace Gramium.Examples.Basic.Handlers;

public class MessageHandler : ICommandHandler
{
    public string Command => "";

    public async Task HandleAsync(IMessageContext context, CancellationToken ct = default)
    {
        var button = InlineKeyboardButton.WithCallbackData("Нажми меня", "test");

        await context.ReplyAsync("Вот сообщение с кнопкой. Нажмите на неё!", InlineKeyboardMarkup.Create(button));
    }
}