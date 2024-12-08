using Gramium.Core.Entities.Callbacks;
using Gramium.Core.Entities.Markup;
using Gramium.Core.Entities.Messages;

namespace Gramium.Framework.Context.Interfaces;

public interface ICallbackQueryContext : IBaseContext
{
    CallbackQuery CallbackQuery { get; }
    Task<Message> EditTextMessageAsync(
        string text,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null);
}