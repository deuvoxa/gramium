using System.Reflection;
using Gramium.Client;
using Gramium.Framework.Callbacks.Interfaces;
using Gramium.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Gramium.Framework.Middleware;
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

        services.Configure<TelegramClientOptions>(options => { options.Token = token; });

        services.AddHttpClient<ITelegramClient, TelegramHttpClient>();
        services.AddSingleton<IGramiumBot, GramiumBot>();

        var callingAssembly = Assembly.GetCallingAssembly();
        
        services.AddSingleton<IUpdateMiddleware, LoggingMiddleware>();
        services.AddSingleton<IUpdateMiddleware, ErrorHandlingMiddleware>();
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

        foreach (var handler in callbackHandlers)
        {
            services.AddScoped(handler);
            services.AddScoped<ICallbackQueryHandler>(sp =>
            {
                var service = sp.GetRequiredService(handler);
                return service as ICallbackQueryHandler ??
                       throw new InvalidOperationException($"Не удалось создать обработчик callback-запросов типа {handler.Name}");
            });
        }

        return services;
    }
}