using Gramium.Core.Entities.Messages;
using Gramium.Framework.Commands;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;

namespace Gramium.Examples.BudgetManager.Handlers.Commands;

public class AddAccountCommand : CommandBase
{
    public override string Command => "/addaccount";
    public override async Task HandleAsync(IMessageContext context, CancellationToken ct = default)
    {
        var keyboard = context.CreateKeyboard()
            .AddButton("Создать счёт", "add-account")
            .Build();
        await context.SendMessageAsync("_*Привет*_", ParseMode.MarkdownV2, keyboard);
    }
}