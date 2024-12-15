namespace Gramium.Framework.Interfaces;

public interface IGramiumBot
{
    Task StartAsync(CancellationToken ct = default);
    Task StopAsync(CancellationToken ct = default);
}