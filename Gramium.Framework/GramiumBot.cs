using Gramium.Client;
using Gramium.Core.Entities.Updates;
using Gramium.Framework.Context;
using Gramium.Framework.Interfaces;
using Gramium.Framework.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gramium.Framework;

public class GramiumBot : IGramiumBot
{
    private readonly ITelegramClient _client;
    private readonly IServiceProvider _serviceProvider;
    private readonly MiddlewarePipeline _pipeline;
    private readonly ILogger<GramiumBot> _logger;
    private long? _offset;
    private volatile bool _isRunning;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public GramiumBot(
        ITelegramClient client,
        IServiceProvider serviceProvider,
        IEnumerable<IUpdateMiddleware> middleware,
        ILogger<GramiumBot> logger)
    {
        _client = client;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _pipeline = new MiddlewarePipeline();

        foreach (var m in middleware) _pipeline.Use(m);
    }

    public async Task StartAsync(CancellationToken ct = default)
    {
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
            {
                try
                {
                    using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                    timeoutCts.CancelAfter(TimeSpan.FromSeconds(30)); // Таймаут для каждого запроса

                    var updates = await _client.GetUpdatesAsync(
                        offset: (int?)_offset,
                        limit: 100,
                        ct: timeoutCts.Token);

                    foreach (var update in updates)
                    {
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