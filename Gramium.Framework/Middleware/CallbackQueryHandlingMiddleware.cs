using Gramium.Client;
using Gramium.Framework.Callbacks.Interfaces;
using Gramium.Framework.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gramium.Framework.Middleware
{
    public class CallbackQueryHandlingMiddleware(
        ITelegramClient client,
        ILogger<CallbackQueryHandlingMiddleware> logger)
        : IUpdateMiddleware
    {
        public async Task HandleAsync(UpdateContext context, UpdateDelegate next)
        {
            if (context.Update.CallbackQuery?.Data == null)
            {
                await next(context);
                return;
            }

            using var scope = context.Services.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<ICallbackQueryHandler>().ToList();

            var callbackData = context.Update.CallbackQuery.Data;

            var handler = handlers.FirstOrDefault(h =>
                callbackData.Equals(h.CallbackData, StringComparison.OrdinalIgnoreCase));

            if (handler != null)
            {
                logger.LogInformation("Найден обработчик {HandlerType} для callback {CallbackData}", 
                    handler.GetType().Name, callbackData);
                    
                var callbackContext = new CallbackQueryContext(context.Update.CallbackQuery, client, context.CancellationToken);
                await handler.HandleAsync(callbackContext, context.CancellationToken);
            }
            else
            {
                logger.LogWarning("Обработчик для callback {CallbackData} не найден", callbackData);
            }

            await next(context);
        }
    }
} 