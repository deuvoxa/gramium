using Gramium.Client;
using Gramium.Core.Entities.Markup;
using Gramium.Core.Entities.Messages;

namespace Gramium.Framework.Context;

public interface IMessageContext
{
    Message Message { get; }
    ITelegramClient Client { get; }
    CancellationToken CancellationToken { get; }
    IServiceProvider Services { get; }
    
    Task<Message> ReplyAsync(string text, IReplyMarkup? replyMarkup = null);
    Task DeleteMessageAsync();
} 