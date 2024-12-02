using Gramium.Client;
using Gramium.Core.Entities.Callbacks;
using Gramium.Core.Entities.Messages;
using Gramium.Framework.Context;
using Gramium.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gramium.Framework.Middleware;

public class CommandHandlingMiddleware(ITelegramClient client, ILogger<CommandHandlingMiddleware> logger)
    : IUpdateMiddleware
{
    public async Task HandleAsync(UpdateContext context, UpdateDelegate next)
    {
        var update = context.Update;
        using var scope = context.Services.CreateScope();

        if (update.Message?.Text != null)
        {
            var messageContext = new MessageContext(update.Message, client, context.CancellationToken);
            var handlers = scope.ServiceProvider.GetServices<ICommandHandler>().ToList();

            var handler = handlers.FirstOrDefault(h =>
                update.Message.Text.Equals(h.Command, StringComparison.OrdinalIgnoreCase));

            if (handler != null) await handler.HandleAsync(messageContext, context.CancellationToken);
        }
        else if (update.CallbackQuery != null)
        {
            var callbackContext = new CallbackQueryContext(update.CallbackQuery, client, context.CancellationToken);
            var handlers = scope.ServiceProvider.GetServices<ICallbackQueryHandler>().ToList();

            var handler = handlers.FirstOrDefault(h =>
                !string.IsNullOrEmpty(h.CallbackData) &&
                update.CallbackQuery.Data?.StartsWith(h.CallbackData) == true);

            if (handler != null) await handler.HandleAsync(callbackContext, context.CancellationToken);
        }

        await next(context);
    }
}