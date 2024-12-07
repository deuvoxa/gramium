using Gramium.Client;
using Gramium.Core.Entities.Callbacks;

namespace Gramium.Framework.Context;

public class CallbackQueryPayloadContext<TPayload>(
    CallbackQuery callbackQuery,
    ITelegramClient client,
    TPayload payload,
    CancellationToken ct = default)
    : CallbackQueryContext(callbackQuery, client, ct), ICallbackQueryPayloadContext<TPayload>
    where TPayload : class
{
    public TPayload Payload { get; } = payload;
}