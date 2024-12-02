using Gramium.Core.Entities.Updates;
using Microsoft.Extensions.Logging;

namespace Gramium.Framework.Middleware
{
    public class LoggingMiddleware(ILogger<LoggingMiddleware> logger) : IUpdateMiddleware
    {
        public async Task HandleAsync(UpdateContext context, UpdateDelegate next)
        {
            var update = context.Update;
            
            logger.LogInformation(
                "Получено обновление {UpdateId}. Тип: {UpdateType}", 
                update.UpdateId,
                GetUpdateType(update));
            
            try
            {
                await next(context);
                
                logger.LogInformation(
                    "Обработка обновления {UpdateId} завершена успешно", 
                    update.UpdateId);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Ошибка при обработке обновления {UpdateId}", 
                    update.UpdateId);
                throw;
            }
        }
        
        private string GetUpdateType(Update update) =>
            update switch
            {
                { Message: not null } => "Message",
                { CallbackQuery: not null } => "CallbackQuery",
                _ => "Unknown"
            };
    }
} 