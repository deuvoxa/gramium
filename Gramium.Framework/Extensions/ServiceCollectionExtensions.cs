using System.Reflection;
using Gramium.Client;
using Gramium.Framework.Callbacks.Interfaces;
using Gramium.Framework.Database;
using Gramium.Framework.Database.Enums;
using Gramium.Framework.Database.Services;
using Gramium.Framework.Interfaces;
using Gramium.Framework.Middleware;
using Gramium.Framework.Pagination;
using Gramium.Framework.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ICommandHandler = Gramium.Framework.Commands.Interfaces.ICommandHandler;

namespace Gramium.Framework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGramium(
        this IServiceCollection services,
        string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentNullException(nameof(token));

        services.Configure<TelegramClientOptions>(options =>
        {
            options.Token = token;
            options.Timeout = TimeSpan.FromSeconds(35);
        });

        services.AddHttpClient<ITelegramClient, TelegramHttpClient>();
        services.AddSingleton<IGramiumBot, GramiumBot>();

        services.AddDbContext<GramiumDbContext>(options =>
            options.UseSqlite("Data Source=gramium.db"));

        services.AddScoped<IPayloadService, PayloadService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserStateService, UserStateService>();

        var callingAssembly = Assembly.GetCallingAssembly();

        services.AddSingleton<IUpdateMiddleware, LoggingMiddleware>();
        services.AddSingleton<IUpdateMiddleware, ErrorHandlingMiddleware>();
        services.AddSingleton<IUpdateMiddleware, UserTrackingMiddleware>();
        services.AddSingleton<IUpdateMiddleware, StateHandlingMiddleware>();
        services.AddSingleton<IUpdateMiddleware, CallbackQueryHandlingMiddleware>();
        services.AddSingleton<IUpdateMiddleware, CommandHandlingMiddleware>();

        var commandHandlers = callingAssembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(ICommandHandler).IsAssignableFrom(t));

        foreach (var handler in commandHandlers)
        {
            services.AddScoped(handler);
            services.AddScoped<ICommandHandler>(sp =>
            {
                var service = sp.GetRequiredService(handler);
                return service as ICommandHandler ??
                       throw new InvalidOperationException($"Не удалось создать обработчик команд типа {handler.Name}");
            });
        }

        var callbackHandlers = callingAssembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(ICallbackQueryHandler).IsAssignableFrom(t));

        services.AddScoped<PaginationCallback>();
        services.AddScoped<ICallbackQueryHandler>(sp =>
            sp.GetRequiredService<PaginationCallback>());

        foreach (var handler in callbackHandlers)
        {
            services.AddScoped(handler);
            services.AddScoped<ICallbackQueryHandler>(sp =>
            {
                var service = sp.GetRequiredService(handler);
                return service as ICallbackQueryHandler ??
                       throw new InvalidOperationException(
                           $"Не удалось создать обработчик callback-запросов типа {handler.Name}");
            });
        }
        
        var stateHandlers = callingAssembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(IStateHandler).IsAssignableFrom(t));

        foreach (var handler in stateHandlers)
        {
            services.AddScoped(handler);
            services.AddScoped<IStateHandler>(sp =>
            {
                var service = sp.GetRequiredService(handler);
                return service as IStateHandler ??
                       throw new InvalidOperationException(
                           $"Не удалось создать обработчик состояний типа {handler.Name}");
            });
        }
        
        return services;
    }

    public static IServiceCollection AddDatabase<TContext>(
        this IServiceCollection services,
        string connectionString,
        DatabaseProvider provider,
        bool autoApplyMigrations = true) where TContext : DbContext
    {
        services.AddScoped<TContext>(sp =>
            (TContext)Activator.CreateInstance(typeof(TContext), connectionString, provider)!);

        if (autoApplyMigrations) services.AddHostedService<DatabaseMigrationService<TContext>>();

        return services;
    }
}