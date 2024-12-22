using Gramium.Client;
using Gramium.Framework.Context;
using Gramium.Framework.Database.Services;
using Gramium.Framework.States;
using Microsoft.Extensions.DependencyInjection;

namespace Gramium.Framework.Middleware;

public class StateHandlingMiddleware(IServiceScopeFactory scopeFactory, ITelegramClient client) : IUpdateMiddleware
{
    public async Task HandleAsync(UpdateContext context, UpdateDelegate next)
    {
        using var scope = scopeFactory.CreateScope();
        var stateService = scope.ServiceProvider.GetRequiredService<IUserStateService>();
        var handlers = scope.ServiceProvider.GetServices<IStateHandler>();

        if (context.Update.Message != null)
        {
            var userId = context.Update.Message.From!.Id;
            var states = await stateService.GetUserStatesAsync(userId);
            
            foreach (var state in states)
            {
                var handler = handlers.FirstOrDefault(h => h.StateKey == state.Key);
                if (handler == null) continue;

                var messageContext = new MessageContext(context.Update.Message, client, scope.ServiceProvider, context.CancellationToken);
                if (await handler.HandleMessageAsync(messageContext, state.Value))
                {
                    var exists = await stateService.HasStateAsync(userId, state.Key);
                    if (!exists)
                    {
                        return;
                    }
                }
            }
        }
        else if (context.Update.CallbackQuery != null)
        {
            var userId = context.Update.CallbackQuery.From.Id;
            var states = await stateService.GetUserStatesAsync(userId);
            
            foreach (var state in states)
            {
                var handler = handlers.FirstOrDefault(h => h.StateKey == state.Key);
                if (handler == null) continue;

                var callbackContext = new CallbackQueryContext(context.Update.CallbackQuery, client, scope.ServiceProvider, context.CancellationToken);
                if (await handler.HandleCallbackQueryAsync(callbackContext, state.Value))
                {
                    var exists = await stateService.HasStateAsync(userId, state.Key);
                    if (!exists)
                    {
                        return;
                    }
                }
            }
        }

        await next(context);
    }
} 