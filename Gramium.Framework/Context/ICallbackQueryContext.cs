using Gramium.Client;
using Gramium.Core.Entities.Callbacks;

namespace Gramium.Framework.Context;

public interface ICallbackQueryContext
{
    CallbackQuery CallbackQuery { get; }
    ITelegramClient Client { get; }
    Task AnswerCallbackQueryAsync(string? text = null);
    Task EditMessageTextAsync(string text);
}