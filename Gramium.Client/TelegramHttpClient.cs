using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Gramium.Core.Contracts;
using Gramium.Core.Entities.Markup;
using Gramium.Core.Entities.Messages;
using Gramium.Core.Entities.Updates;
using Gramium.Core.Exceptions;
using Gramium.Core.Json;
using Microsoft.Extensions.Options;

namespace Gramium.Client;

public class TelegramHttpClient : ITelegramClient
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public TelegramHttpClient(
        HttpClient client,
        IOptions<TelegramClientOptions> options)
    {
        _client = client;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Converters = { new ReplyMarkupConverter() }
        };

        _client.BaseAddress = new Uri(options.Value.GetBaseUrl());
        _client.Timeout = options.Value.Timeout;
    }
    
    private async Task<TResponse> SendAsync<TResponse>(
        string method,
        object? request = null,
        CancellationToken ct = default)
    {
        var methodPath = method.TrimStart('/');
        
        using var content = request != null
            ? new StringContent(
                JsonSerializer.Serialize(request, _jsonOptions),
                Encoding.UTF8,
                "application/json")
            : null;

        using var response = await _client.PostAsync(
            methodPath,
            content ?? new StringContent(string.Empty),
            ct);

        return await HandleResponseAsync<TResponse>(response, ct);
    }

    private async Task<TResponse> HandleResponseAsync<TResponse>(
        HttpResponseMessage response, 
        CancellationToken ct)
    {
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new TelegramApiException(
                $"Запрос завершился с ошибкой: {response.StatusCode}. Тело ответа: {body}",
                (int)response.StatusCode);
        }

        var result = JsonSerializer.Deserialize<TelegramResponse<TResponse>>(
            body,
            _jsonOptions);

        if (result?.Ok != true || result.Result == null)
        {
            throw new TelegramApiException(
                $"Ошибка API Telegram: {result?.Description ?? "Неизвестная ошибка"}. Тело ответа: {body}",
                result?.ErrorCode ?? 0);
        }

        return result.Result;
    }

    public async Task<Update[]> GetUpdatesAsync(
        int? offset = null,
        int? limit = null,
        CancellationToken ct = default)
    {
        var request = new
        {
            offset,
            limit,
            timeout = 30
        };

        return await SendAsync<Update[]>("getUpdates", request, ct);
    }
    public async Task DeleteMessageAsync(
        long chatId,
        long messageId,
        CancellationToken ct = default)
    {
        var request = new
        {
            chat_id = chatId,
            message_id = messageId
        };

        await SendAsync<bool>("deleteMessage", request, ct);
    }

    public async Task<Message> EditMessageTextAsync(
        long chatId,
        long messageId,
        string text,
        IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default)
    {
        var request = new
        {
            chat_id = chatId,
            message_id = messageId,
            text,
            reply_markup = replyMarkup
        };

        return await SendAsync<Message>("editMessageText", request, ct);
    }

    public async Task<Message> SendPhotoAsync(
        long chatId,
        string photo,
        string? caption = null,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default)
    {
        var request = new
        {
            chat_id = chatId,
            photo,
            caption,
            parse_mode = GetParseModeString(parseMode),
            reply_markup = replyMarkup
        };

        return await SendAsync<Message>("sendPhoto", request, ct);
    }

    public async Task<Message> SendDocumentAsync(
        long chatId,
        string document,
        string? caption = null,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default)
    {
        var request = new
        {
            chat_id = chatId,
            document,
            caption,
            parse_mode = GetParseModeString(parseMode),
            reply_markup = replyMarkup
        };

        return await SendAsync<Message>("sendDocument", request, ct);
    }

    public async Task<Message> SendVideoAsync(
        long chatId,
        string video,
        string? caption = null,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default)
    {
        var request = new
        {
            chat_id = chatId,
            video,
            caption,
            parse_mode = GetParseModeString(parseMode),
            reply_markup = replyMarkup
        };

        return await SendAsync<Message>("sendVideo", request, ct);
    }

    public async Task<Message> SendMessageAsync(
        long chatId,
        string text,
        ParseMode parseMode = ParseMode.None,
        IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default)
    {
        var request = new
        {
            chat_id = chatId,
            text,
            parse_mode = GetParseModeString(parseMode),
            reply_markup = replyMarkup
        };

        return await SendAsync<Message>("sendMessage", request, ct);
    }

    private static string GetParseModeString(ParseMode parseMode) => parseMode switch
    {
        ParseMode.MarkdownV2 => "MarkdownV2",
        ParseMode.HTML => "HTML",
        ParseMode.Markdown => "Markdown",
        _ => string.Empty
    };
}