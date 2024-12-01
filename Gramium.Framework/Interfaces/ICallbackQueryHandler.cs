using Gramium.Framework.Context;

namespace Gramium.Framework.Interfaces;

public interface ICallbackQueryHandler
{
    string CallbackData { get; }
    Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default);
}