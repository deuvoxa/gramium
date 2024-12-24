using Gramium.Client;
using Gramium.Core.Entities.Markup;
using Gramium.Core.Entities.Messages;
using Gramium.Framework.Context.Interfaces;

namespace Gramium.Framework.Context;

public class MessageContext(
    Message message,
    ITelegramClient client,
    IServiceProvider services,
    CancellationToken ct = default)
    : BaseContext(client, services, message, ct), IMessageContext
{
    public Message Message { get; } = message;

    public async Task DeleteMessageAsync(long messageId)
        => await Client.DeleteMessageAsync(Message.Chat.Id, messageId, CancellationToken);


    public async Task<Message> EditTextMessageAsync(long messageId, string text, ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null)
    {
        if (Message == null)
            throw new InvalidOperationException("Message is null");

        return await Client.EditMessageTextAsync(
            Message.Chat.Id,
            messageId,
            text,
            parseMode,
            replyMarkup,
            CancellationToken);
    }
}