using System.Text.Json;
using Gramium.Client;
using Gramium.Framework.Callbacks.Interfaces;
using Gramium.Framework.Context;
using Gramium.Framework.Database.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gramium.Framework.Middleware
{
    public class CallbackQueryHandlingMiddleware(
        ITelegramClient client,
        IServiceScopeFactory serviceScopeFactory,
        IServiceProvider serviceProvider)
        : IUpdateMiddleware
    {
        public async Task HandleAsync(UpdateContext context, UpdateDelegate next)
        {
            if (context.Update.CallbackQuery?.Data == null)
            {   
                await next(context);
                return;
            }

            using var scope = serviceScopeFactory.CreateScope();
            var payloadService = scope.ServiceProvider.GetRequiredService<IPayloadService>();
            var handlers = scope.ServiceProvider.GetServices<ICallbackQueryHandler>().ToList();

            var callbackData = context.Update.CallbackQuery.Data;

            try
            {
                var (handlerType, payload) = await payloadService.GetPayloadAsync<object>(callbackData);
                var handler = handlers.FirstOrDefault(h => h.GetType().FullName == handlerType);

                if (handler != null)
                {
                    var payloadType = handler.GetType().BaseType?.GenericTypeArguments[0];
                    if (payloadType == null)
                        throw new InvalidOperationException($"Не удалось определить тип payload для {handler.GetType().Name}");

                    var typedPayload = ((JsonElement)payload).Deserialize(payloadType);
            
                    var payloadContextType = typeof(CallbackQueryPayloadContext<>).MakeGenericType(payloadType);
                    var payloadContext = Activator.CreateInstance(
                        payloadContextType,
                        context.Update.CallbackQuery,
                        client,
                        serviceProvider,
                        typedPayload,
                        context.CancellationToken) as ICallbackQueryContext;

                    await handler.HandleAsync(payloadContext!, context.CancellationToken);
                }
            }
            catch
            {
                var handler = handlers.FirstOrDefault(h => 
                    callbackData.Equals(h.CallbackData, StringComparison.OrdinalIgnoreCase));

                if (handler != null)
                {
                    var callbackContext = new CallbackQueryContext(
                        context.Update.CallbackQuery, 
                        client,serviceProvider,
                        context.CancellationToken);
                        
                    await handler.HandleAsync(callbackContext, context.CancellationToken);
                }
            }

            await next(context);
        }
    }
}