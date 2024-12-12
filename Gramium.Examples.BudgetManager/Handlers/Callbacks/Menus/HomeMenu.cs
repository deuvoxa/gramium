using Gramium.Core.Entities.Messages;
using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;

namespace Gramium.Examples.BudgetManager.Handlers.Callbacks.Menus;

public class HomeMenu : CallbackQueryBase
{
    public override string CallbackData => "menu-home";
    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        const string text = "_*Главное меню:*_";
        var keyboard = context.CreateKeyboard()
            .AddButtons(Buttons.TransactionsMenu, Buttons.AccountsMenu)
            .Build();

        await context.EditTextMessageAsync(text, ParseMode.MarkdownV2, keyboard);
    }
}