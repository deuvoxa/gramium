using Gramium.Client;
using Gramium.Core.Entities.Updates;
using Gramium.Framework.Context;
using Gramium.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Gramium.Framework;

public class GramiumBot(
    ITelegramClient client,
    IServiceProvider serviceProvider)
    : IGramiumBot
{
    private long? _offset;
    private bool _isRunning;
    private readonly TimeSpan _errorDelay = TimeSpan.FromSeconds(5);

    public async Task StartAsync(CancellationToken ct = default)
    {
        _isRunning = true;

        while (_isRunning && !ct.IsCancellationRequested)
        {
            var updates = await client.GetUpdatesAsync(
                offset: (int?)_offset,
                limit: 100,
                ct: ct);

            foreach (var update in updates)
            {
                await ProcessUpdateAsync(update, ct);
                _offset = update.UpdateId + 1;
            }
        }
    }

    private async Task ProcessUpdateAsync(Update update, CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();

        if (update.Message?.Text != null)
        {
            var context = new MessageContext(update.Message, client, ct);
            var handlers = scope.ServiceProvider.GetServices<ICommandHandler>().ToList();

            foreach (var handler in handlers)
            {
                if (!string.IsNullOrEmpty(handler.Command) &&
                    !update.Message.Text.StartsWith(handler.Command, StringComparison.OrdinalIgnoreCase)) continue;

                await handler.HandleAsync(context, ct);
                break;
            }
        }
        else if (update.CallbackQuery != null)
        {
            var context = new CallbackQueryContext(update.CallbackQuery, client, ct);
            var handlers = scope.ServiceProvider.GetServices<ICallbackQueryHandler>().ToList();

            foreach (var handler in handlers.Where(handler =>
                         update.CallbackQuery.Data?.StartsWith(handler.CallbackData) == true))
            {
                await handler.HandleAsync(context, ct);
                break;
            }
        }
    }

    public Task StopAsync(CancellationToken ct = default)
    {
        _isRunning = false;
        return Task.CompletedTask;
    }
}