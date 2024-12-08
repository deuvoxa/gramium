using Gramium.Client;
using Gramium.Core.Entities.Callbacks;
using Gramium.Core.Entities.Markup;

namespace Gramium.Framework.Context;

public class CallbackQueryContext(
    CallbackQuery callbackQuery,
    ITelegramClient client,
    IServiceProvider services,
    CancellationToken ct = default)
    : ICallbackQueryContext
{
    public CallbackQuery CallbackQuery { get; } = callbackQuery;
    public ITelegramClient Client { get; } = client;
    public IServiceProvider Services { get; } = services;

    public Task EditMessageTextAsync(string text, IReplyMarkup? replyMarkup = null)
    {
        if (CallbackQuery.Message == null)
            throw new InvalidOperationException("Message is null");

        return Client.EditMessageTextAsync(
            CallbackQuery.Message.Chat.Id,
            CallbackQuery.Message.MessageId,
            text,
            replyMarkup,
            ct);
    }
}