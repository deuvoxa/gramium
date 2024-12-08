using Gramium.Framework.Callbacks;
using Gramium.Framework.Callbacks.Interfaces;
using Gramium.Framework.Context;
using Gramium.Framework.Database.Services;
using Gramium.Framework.Markup;
using Microsoft.Extensions.DependencyInjection;

namespace Gramium.Framework.Extensions;

public static class InlineKeyboardBuilderExtensions
{
    public static InlineKeyboardBuilder WithPayloadButton<TPayload>(
        this InlineKeyboardBuilder builder,
        IMessageContext context,
        string text,
        TPayload payload) where TPayload : class
    {
        return WithPayloadButtonInternal(builder, context.Services, text, payload);
    }

    // dont work 
    public static InlineKeyboardBuilder WithPayloadButton<TPayload>(
        this InlineKeyboardBuilder builder,
        ICallbackQueryContext context,
        string text,
        TPayload payload) where TPayload : class
    {
        return WithPayloadButtonInternal(builder, context.Services, text, payload);
    }

    private static InlineKeyboardBuilder WithPayloadButtonInternal<TPayload>(
        InlineKeyboardBuilder builder,
        IServiceProvider services,
        string text,
        TPayload payload) where TPayload : class
    {
        var payloadService = services.GetRequiredService<IPayloadService>();
        
        var callbackType = typeof(PayloadCallbackBase<>).MakeGenericType(typeof(TPayload));
        var handlerType = services.GetServices<ICallbackQueryHandler>()
            .First(h => callbackType.IsInstanceOfType(h))
            .GetType()
            .FullName!;

        var callbackData = payloadService.SavePayloadAsync(handlerType, payload).Result;
        return builder.AddButton(text, callbackData);
    }
}