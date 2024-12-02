using Gramium.Core.Entities.Updates;
using Microsoft.Extensions.DependencyInjection;

namespace Gramium.Framework.Middleware;

public class UpdateContext(
    Update update,
    IServiceProvider services,
    CancellationToken cancellationToken = default)
{
    public Update Update { get; } = update ?? throw new ArgumentNullException(nameof(update));
    public IServiceProvider Services { get; } = services ?? throw new ArgumentNullException(nameof(services));
    public CancellationToken CancellationToken { get; } = cancellationToken;

    public T? GetService<T>() where T : class => 
        Services.GetService<T>();
        
    public T GetRequiredService<T>() where T : class => 
        Services.GetRequiredService<T>();
        
    public IServiceScope CreateScope() => 
        Services.CreateScope();
} 