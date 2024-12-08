using Gramium.Client;
using Gramium.Core.Entities.Markup;
using Gramium.Core.Entities.Messages;

namespace Gramium.Framework.Context.Interfaces;

public interface IBaseContext
{
    ITelegramClient Client { get; }
    IServiceProvider Services { get; }
    CancellationToken CancellationToken { get; }
    
    Task<Message> SendMessageAsync(
        string text,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null);
} 