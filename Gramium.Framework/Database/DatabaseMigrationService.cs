using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gramium.Framework.Database;

public class DatabaseMigrationService<TContext>(
    IServiceProvider serviceProvider,
    ILogger<DatabaseMigrationService<TContext>> logger)
    : IHostedService
    where TContext : DbContext
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Применение миграций для контекста {ContextName}", typeof(TContext).Name);
            
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            
            await context.Database.MigrateAsync(cancellationToken);
            
            logger.LogInformation("Миграции успешно применены для контекста {ContextName}", typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при применении миграций для контекста {ContextName}", typeof(TContext).Name);
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
} 