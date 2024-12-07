using Gramium.Client;
using Gramium.Framework.Commands.Interfaces;
using Gramium.Framework.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gramium.Framework.Middleware;

public class CommandHandlingMiddleware(
    ITelegramClient client, 
    ILogger<CommandHandlingMiddleware> logger,
    IServiceScopeFactory scopeFactory) : IUpdateMiddleware
{
    public async Task HandleAsync(UpdateContext context, UpdateDelegate next)
    {
        if (context.Update.Message?.Text == null)
        {
            await next(context);
            return;
        }

        using var scope = scopeFactory.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<ICommandHandler>().ToList();

        var messageText = context.Update.Message.Text;
        var command = messageText.Split(' ')[0].ToLower();

        var handler = handlers.FirstOrDefault(h =>
            command.Equals(h.Command, StringComparison.OrdinalIgnoreCase) ||
            h.Aliases.Any(a => command.Equals(a, StringComparison.OrdinalIgnoreCase)));

        if (handler != null)
        {
            logger.LogInformation("Найден обработчик {HandlerType} для команды {Command}", 
                handler.GetType().Name, command);
                
            var messageContext = new MessageContext(context.Update.Message, client, scope.ServiceProvider, context.CancellationToken);
            await handler.HandleAsync(messageContext, context.CancellationToken);
        }

        await next(context);
    }
}