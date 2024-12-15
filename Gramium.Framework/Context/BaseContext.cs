using Gramium.Client;
using Gramium.Core.Entities.Markup;
using Gramium.Core.Entities.Messages;
using Gramium.Framework.Context.Interfaces;

namespace Gramium.Framework.Context;

public abstract class BaseContext(
    ITelegramClient client,
    IServiceProvider services,
    Message message,
    CancellationToken ct = default)
    : IBaseContext
{
    public ITelegramClient Client { get; } = client;
    public IServiceProvider Services { get; } = services;
    public CancellationToken CancellationToken { get; } = ct;

    public Task<Message> SendMessageAsync(
        string text,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null)
    {
        return Client.SendMessageAsync(message.Chat.Id, text, parseMode, replyMarkup, CancellationToken);
    }

    public async Task<Message> EditTextMessageAsync(
        string text,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null)
    {
        if (message == null)
            throw new InvalidOperationException("Message is null");

        return await Client.EditMessageTextAsync(
            message.Chat.Id,
            message.MessageId,
            text,
            parseMode,
            replyMarkup,
            CancellationToken);
    }
}