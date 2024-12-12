using Gramium.Core.Entities.Messages;
using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;

namespace Gramium.Examples.BudgetManager.Handlers.Callbacks.Menus;

public class TransactionsMenu : CallbackQueryBase
{
    public override string CallbackData => "menu-transactions";
    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        const string text = "_*Меню транзакций:*_";
        var keyboard = context.CreateKeyboard()
            .AddButtons(Buttons.HomeMenu)
            .Build();
        await context.EditTextMessageAsync(text, ParseMode.MarkdownV2, keyboard);
    }
}