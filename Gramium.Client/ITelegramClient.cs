using Gramium.Core.Entities.Markup;
using Gramium.Core.Entities.Messages;
using Gramium.Core.Entities.Updates;

namespace Gramium.Client;

public interface ITelegramClient
{
    Task<Update[]> GetUpdatesAsync(int? offset = null, int? limit = null, CancellationToken ct = default);

    Task<Message> SendMessageAsync(long chatId, string text, IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default);

    Task DeleteMessageAsync(long chatId, long messageId, CancellationToken ct = default);

    Task AnswerCallbackQueryAsync(string callbackQueryId, string? text = null, CancellationToken ct = default);

    Task<Message> EditMessageTextAsync(long chatId, long messageId, string text, CancellationToken ct = default);
}