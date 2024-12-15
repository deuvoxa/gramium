using Gramium.Core.Entities.Messages;
using Gramium.Examples.BudgetManager.Extensions;
using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;
using UserService = Gramium.Examples.BudgetManager.Services.UserService;

namespace Gramium.Examples.BudgetManager.Handlers.Callbacks.Menus;

public class AccountsMenu : CallbackQueryBase
{
    public override string CallbackData => MenuButtons.AccountsMenu.Item2;

    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        var userService = context.Services.GetRequiredService<UserService>();
        var user = await userService.GetUserByTelegramIdAsync(context.CallbackQuery.From.Id) ?? throw new Exception();
        var accounts = user.GetActiveAccount();

        var text = !accounts.Any()
            ? "У вас нет счетов, добавьте их"
            : $"{accounts.Aggregate("Твои счета:\n\n", (current, account) =>
                current + $"_{account.Name}_: `{account.Balance}` \u20bd\n\n")}";

        var keyboard = context.CreateKeyboard();
        if (accounts.Count >= 2) keyboard.WithButtons(TransactionButtons.Transfer);

        if (accounts.Count != 0) keyboard.WithButtons(AccountButtons.Remove);

        keyboard.WithButtons(AccountButtons.Add)
            .WithButtons(MenuButtons.HomeMenu);

        await context.EditTextMessageAsync(text, ParseMode.MarkdownV2, keyboard.Build());
    }
}