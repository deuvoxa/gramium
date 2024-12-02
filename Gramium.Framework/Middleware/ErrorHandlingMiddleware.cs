using Gramium.Client;

namespace Gramium.Framework.Middleware;

public class ErrorHandlingMiddleware(ITelegramClient client) : IUpdateMiddleware
{
    public async Task HandleAsync(UpdateContext context, UpdateDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception)
        {
            var chatId = context.Update switch
            {
                { Message: { Chat: { Id: var id } } } => id,
                { CallbackQuery: { Message: { Chat: { Id: var id } } } } => id,
                _ => null as long?
            };

            if (chatId.HasValue)
            {
                await client.SendMessageAsync(
                    chatId.Value,
                    "Произошла ошибка при обработке запроса. Попробуйте позже.",
                    ct: context.CancellationToken);
            }
            
            throw;
        }
    }
}