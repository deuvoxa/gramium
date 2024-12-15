using Gramium.Core.Entities.Markup;
using Gramium.Core.Entities.Messages;

namespace Gramium.Framework.Context.Interfaces;

public interface IMessageContext : IBaseContext
{
    Message Message { get; }
    Task DeleteMessageAsync();

    Task<Message> EditTextMessageAsync(
        long messageId,
        string text,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null);
}