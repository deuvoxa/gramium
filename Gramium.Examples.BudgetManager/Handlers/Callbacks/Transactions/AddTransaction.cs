using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;

namespace Gramium.Examples.BudgetManager.Handlers.Callbacks.Transactions;

public class AddTransaction : CallbackQueryBase
{
    public override string CallbackData => "add_transaction";

    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
    }
}