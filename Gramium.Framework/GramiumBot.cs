using Gramium.Client;
using Gramium.Framework.Database;
using Gramium.Framework.Interfaces;
using Gramium.Framework.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gramium.Framework;

public class GramiumBot : IGramiumBot
{
    private readonly ITelegramClient _client;
    private readonly ILogger<GramiumBot> _logger;
    private readonly MiddlewarePipeline _pipeline;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly IServiceProvider _serviceProvider;
    private volatile bool _isRunning;
    private long? _offset;

    public GramiumBot(
        ITelegramClient client,
        IServiceProvider serviceProvider,
        IEnumerable<IUpdateMiddleware> middleware,
        ILogger<GramiumBot> logger,
        IServiceScopeFactory scopeFactory)
    {
        _client = client;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _pipeline = new MiddlewarePipeline();
        _scopeFactory = scopeFactory;

        foreach (var m in middleware) _pipeline.Use(m);
    }

    public async Task StartAsync(CancellationToken ct = default)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GramiumDbContext>();
            await context.Database.MigrateAsync(ct);
        }

        if (!await _semaphore.WaitAsync(0, ct))
        {
            _logger.LogWarning("Бот уже запущен");
            return;
        }

        try
        {
            _isRunning = true;
            _logger.LogInformation("Бот запущен");

            while (_isRunning && !ct.IsCancellationRequested)
                try
                {
                    using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                    timeoutCts.CancelAfter(TimeSpan.FromSeconds(30));

                    var updates = await _client.GetUpdatesAsync(
                        (int?)_offset,
                        100,
                        timeoutCts.Token);

                    foreach (var update in updates)
                        try
                        {
                            var context = new UpdateContext(update, _serviceProvider, ct);
                            await _pipeline.ExecuteAsync(context);
                            _offset = update.UpdateId + 1;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Ошибка при обработке обновления {UpdateId}", update.UpdateId);
                        }
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при получении обновлений");
                    await Task.Delay(TimeSpan.FromSeconds(5), ct);
                }
        }
        finally
        {
            _isRunning = false;
            _semaphore.Release();
            _logger.LogInformation("Бот остановлен");
        }
    }

    public Task StopAsync(CancellationToken ct = default)
    {
        _isRunning = false;
        return Task.CompletedTask;
    }
}