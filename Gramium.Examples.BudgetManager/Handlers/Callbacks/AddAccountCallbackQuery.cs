using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;

namespace Gramium.Examples.BudgetManager.Handlers.Callbacks;

public class AddAccountCallbackQuery : CallbackQueryBase
{
    public override string CallbackData => "add-account";
    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        await context.EditTextMessageAsync("-создаю счёт в банке-");
    }
}