using Gramium.Core.Entities.Messages;
using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;

namespace Gramium.Examples.BudgetManager.Handlers.Callbacks.Menus;

public class HomeMenu : CallbackQueryBase
{
    public override string CallbackData => MenuButtons.HomeMenu.Item2;

    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        const string text = "_*Главное меню:*_";
        var keyboard = context.CreateKeyboard()
            .WithButtons(MenuButtons.TransactionsMenu, MenuButtons.AccountsMenu)
            .WithButtons(MenuButtons.StatisticsMenu)
            .Build();

        await context.EditTextMessageAsync(text, ParseMode.MarkdownV2, keyboard);
    }
}