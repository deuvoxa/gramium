using Gramium.Core.Entities.Updates;

namespace Gramium.Framework.Middleware;

public class UpdateContext(
    Update update,
    IServiceProvider services,
    CancellationToken cancellationToken = default)
{
    public Update Update { get; } = update ?? throw new ArgumentNullException(nameof(update));
    public IServiceProvider Services { get; } = services ?? throw new ArgumentNullException(nameof(services));
    public CancellationToken CancellationToken { get; } = cancellationToken;
}