using Gramium.Framework.Callbacks.Interfaces;
using Gramium.Framework.Context;

namespace Gramium.Framework.Callbacks;

public abstract class CallbackQueryBase : ICallbackQueryHandler
{
    public abstract string CallbackData { get; }
    public virtual string Description => string.Empty;
    public abstract Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default);
}