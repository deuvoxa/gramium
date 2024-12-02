using Gramium.Framework.Context;
using Gramium.Core.Entities.Markup;
using Gramium.Framework.Interfaces;
using Gramium.Framework.Markup;

namespace Gramium.Examples.Basic.Handlers;

public class MessageHandler : ICommandHandler
{
    public string Command => "/test";

    public async Task HandleAsync(IMessageContext context, CancellationToken ct = default)
    {
        var keyboard = context.CreateKeyboard()
            .AddButton("Нажми меня", "test")
            .AddButton("кнопка", "test2")
            .AddButtons(
                [
                    ("да", "yes"),
                    ("нет", "no")
                ]
            )
            .Build();

        await context.ReplyAsync("Вот сообщение с кнопкой. Нажмите на неё!", keyboard);
    }
}