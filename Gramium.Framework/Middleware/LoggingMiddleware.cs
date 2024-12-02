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
                "Получено обновление {UpdateId}. Тип: {UpdateType}. Данные: {@Update}", 
                update.UpdateId,
                GetUpdateType(update),
                update);
            
            try
            {
                await next(context);
                
                logger.LogInformation(
                    "Обновление {UpdateId} успешно обработано",
                    update.UpdateId);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Ошибка при обработке обновления {UpdateId}. Сообщение: {Message}", 
                    update.UpdateId,
                    ex.Message);
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