using Gramium.Client;
using Gramium.Core.Entities.Callbacks;
using Gramium.Core.Entities.Markup;

namespace Gramium.Framework.Context;

public interface ICallbackQueryContext
{
    CallbackQuery CallbackQuery { get; }
    ITelegramClient Client { get; }
    IServiceProvider Services { get; }
    Task EditMessageTextAsync(string text, IReplyMarkup? replyMarkup = null);
}