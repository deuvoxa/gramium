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
    private long? _offset;
    private bool _isRunning;

    public GramiumBot(
        ITelegramClient client,
        IServiceProvider serviceProvider,
        IEnumerable<IUpdateMiddleware> middleware)
    {
        _client = client;
        _serviceProvider = serviceProvider;
        _pipeline = new MiddlewarePipeline();

        foreach (var m in middleware) _pipeline.Use(m);
    }

    public async Task StartAsync(CancellationToken ct = default)
    {
        _isRunning = true;

        while (_isRunning && !ct.IsCancellationRequested)
        {
            var updates = await _client.GetUpdatesAsync(
                offset: (int?)_offset,
                limit: 100,
                ct: ct);

            foreach (var update in updates)
            {
                var context = new UpdateContext(update, _serviceProvider, ct);
                await _pipeline.ExecuteAsync(context);
                _offset = update.UpdateId + 1;
            }
        }
    }

    public Task StopAsync(CancellationToken ct = default)
    {
        _isRunning = false;
        return Task.CompletedTask;
    }
}