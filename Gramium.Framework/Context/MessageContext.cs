using Gramium.Client;
using Gramium.Core.Entities.Markup;
using Gramium.Core.Entities.Messages;

namespace Gramium.Framework.Context;

public class MessageContext(
    Message message,
    ITelegramClient client,
    CancellationToken ct = default)
    : IMessageContext
{
    public Message Message { get; } = message;
    public ITelegramClient Client { get; } = client;
    public CancellationToken CancellationToken { get; } = ct;

    public Task<Message> ReplyAsync(string text, IReplyMarkup? replyMarkup = null)
    {
        return Client.SendMessageAsync(Message.Chat.Id, text, replyMarkup, CancellationToken);
    }

    public Task DeleteMessageAsync()
    {
        return Client.DeleteMessageAsync(Message.Chat.Id, Message.MessageId, CancellationToken);
    }
}