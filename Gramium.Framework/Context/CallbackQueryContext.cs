using Gramium.Client;
using Gramium.Core.Entities.Callbacks;

namespace Gramium.Framework.Context;

public class CallbackQueryContext(
    CallbackQuery callbackQuery,
    ITelegramClient client,
    CancellationToken ct = default)
    : ICallbackQueryContext
{
    public CallbackQuery CallbackQuery { get; } = callbackQuery;
    public ITelegramClient Client { get; } = client;

    public Task AnswerCallbackQueryAsync(string? text = null)
    {
        return Client.AnswerCallbackQueryAsync(CallbackQuery.Id, text, ct);
    }

    public Task EditMessageTextAsync(string text)
    {
        if (CallbackQuery.Message == null)
            throw new InvalidOperationException("Message is null");

        return Client.EditMessageTextAsync(
            CallbackQuery.Message.Chat.Id,
            CallbackQuery.Message.MessageId,
            text,
            ct);
    }
}