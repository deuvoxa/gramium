using System.Reflection;
using Gramium.Client;
using Gramium.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
        });
        
        services.AddHttpClient<ITelegramClient, TelegramHttpClient>();

        services.AddSingleton<IGramiumBot, GramiumBot>();

        var callingAssembly = Assembly.GetCallingAssembly();
        
        var commandHandlers = callingAssembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(ICommandHandler).IsAssignableFrom(t));

        foreach (var handler in commandHandlers)
        {
            services.AddScoped(typeof(ICommandHandler), handler);
        }
        
        var callbackHandlers = callingAssembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(ICallbackQueryHandler).IsAssignableFrom(t));

        foreach (var handler in callbackHandlers)
        {
            services.AddScoped(typeof(ICallbackQueryHandler), handler);
        }

        return services;
    }
} 