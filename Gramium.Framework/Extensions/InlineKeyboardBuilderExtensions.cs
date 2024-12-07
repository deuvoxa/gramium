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
        var payloadService = context.Services.GetRequiredService<IPayloadService>();
        
        var callbackType = typeof(PayloadCallbackBase<>).MakeGenericType(typeof(TPayload));
        var handlerType = context.Services.GetServices<ICallbackQueryHandler>()
            .First(h => callbackType.IsInstanceOfType(h))
            .GetType()
            .FullName!;

        var callbackData = payloadService.SavePayloadAsync(handlerType, payload).Result;
        return builder.AddButton(text, callbackData);
    }
}