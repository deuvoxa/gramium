using Gramium.Core.Entities.Markup;
using Gramium.Core.Entities.Messages;
using Gramium.Core.Entities.Updates;

namespace Gramium.Client;

public interface ITelegramClient
{
    Task<Update[]> GetUpdatesAsync(int? offset = null, int? limit = null, CancellationToken ct = default);

    Task DeleteMessageAsync(long chatId, long messageId, CancellationToken ct = default);

    Task<Message> EditMessageTextAsync(
        long chatId,
        long messageId,
        string text,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default);

    Task<Message> SendPhotoAsync(
        long chatId,
        string photo,
        string? caption = null,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default);

    Task<Message> SendDocumentAsync(
        long chatId,
        string document,
        string? caption = null,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default);

    Task<Message> SendVideoAsync(
        long chatId,
        string video,
        string? caption = null,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default);

    Task<Message> SendMessageAsync(
        long chatId,
        string text,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default);
}