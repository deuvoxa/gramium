using Gramium.Examples.BudgetManager.Extensions;
using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;

namespace Gramium.Examples.BudgetManager.Handlers.Callbacks.Accounts;

public class RemoveAccount : CallbackQueryBase
{
    public override string CallbackData => AccountButtons.Remove.Item2;

    // todo: доп параметры для callback data
    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        var user = await context.GetUserAsync();
        var accounts = user.GetActiveAccount();

        // var removeAccount = await context.GetMetadataAsync("RemoveAccountId");
        //
        // if (removeAccount is null)
        // {
        //     var text = "*Удаление счёта*:\n\n" +
        //                "Какой счёт удалить?";
        //
        //     var keyboard = context.CreateKeyboard()
        //         .WithButtonGrid()
        //         .WithButtons(MenuButtons.AccountsMenu)
        //         .Build();
        //     
        //     return;
        // }

        

    }

    // TODO: Перед удалением уточнять у пользователя в достоверности
    // TODO: Перед удалением спрашивать, переводить ли остаток со счёта на другой счёт (если он имеется), если нет, предупреждать
}