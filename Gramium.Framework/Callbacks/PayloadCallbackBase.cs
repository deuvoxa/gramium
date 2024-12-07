using Gramium.Framework.Context;

namespace Gramium.Framework.Callbacks;

public abstract class PayloadCallbackBase<TPayload> : CallbackQueryBase 
    where TPayload : class
{
    public override string CallbackData => "";
    public override Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        var payloadContext = (ICallbackQueryPayloadContext<TPayload>)context;
        return HandlePayloadAsync(payloadContext, ct);
    }

    protected abstract Task HandlePayloadAsync(
        ICallbackQueryPayloadContext<TPayload> context, 
        CancellationToken ct = default);
}