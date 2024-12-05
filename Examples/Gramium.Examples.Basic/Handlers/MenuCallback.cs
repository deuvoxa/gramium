using Gramium.Framework.Callbacks;
using Gramium.Framework.Context;

namespace Gramium.Examples.Basic.Handlers;

public class MenuCallback : CallbackQueryBase
{
    public override string CallbackData => "menu";
    public override string Description => "Открыть главное меню";

    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        await context.EditMessageTextAsync("Главное меню");
    }
}