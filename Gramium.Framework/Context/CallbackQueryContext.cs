using Gramium.Client;
using Gramium.Core.Entities.Callbacks;
using Gramium.Core.Entities.Markup;
using Gramium.Core.Entities.Messages;
using Gramium.Framework.Context.Interfaces;

namespace Gramium.Framework.Context;

public class CallbackQueryContext(
    CallbackQuery callbackQuery,
    ITelegramClient client,
    IServiceProvider services,
    CancellationToken ct = default)
    : BaseContext(client, services, callbackQuery.Message!, ct), ICallbackQueryContext
{
    public CallbackQuery CallbackQuery { get; } = callbackQuery;
}