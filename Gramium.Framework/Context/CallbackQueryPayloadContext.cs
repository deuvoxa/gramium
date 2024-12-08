using Gramium.Client;
using Gramium.Core.Entities.Callbacks;

namespace Gramium.Framework.Context;

public class CallbackQueryPayloadContext<TPayload>(
    CallbackQuery callbackQuery,
    ITelegramClient client,
    IServiceProvider serviceProvider,
    TPayload payload,
    CancellationToken ct = default)
    : CallbackQueryContext(callbackQuery, client, serviceProvider, ct), ICallbackQueryPayloadContext<TPayload>
    where TPayload : class
{
    public TPayload Payload { get; } = payload;
}