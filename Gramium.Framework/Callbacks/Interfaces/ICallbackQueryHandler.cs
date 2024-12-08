using Gramium.Framework.Context;
using Gramium.Framework.Context.Interfaces;

namespace Gramium.Framework.Callbacks.Interfaces;

public interface ICallbackQueryHandler : ICallbackMetadata
{
    Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default);
}