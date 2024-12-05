using Gramium.Framework.Context;

namespace Gramium.Framework.Callbacks.Interfaces;

public interface ICallbackQueryHandler : ICallbackMetadata
{
    Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default);
}