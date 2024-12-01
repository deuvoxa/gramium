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

    public async Task<Message> SendMessageAsync(
        long chatId,
        string text,
        IReplyMarkup? replyMarkup = null,
        CancellationToken ct = default)
    {
        var request = new
        {
            chat_id = chatId,
            text,
            reply_markup = replyMarkup
        };

        return await SendAsync<Message>("sendMessage", request, ct);
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

    public async Task AnswerCallbackQueryAsync(
        string callbackQueryId,
        string? text = null,
        CancellationToken ct = default)
    {
        var request = new
        {
            callback_query_id = callbackQueryId,
            text
        };

        await SendAsync<bool>("answerCallbackQuery", request, ct);
    }

    public async Task<Message> EditMessageTextAsync(
        long chatId,
        long messageId,
        string text,
        CancellationToken ct = default)
    {
        var request = new
        {
            chat_id = chatId,
            message_id = messageId,
            text
        };

        return await SendAsync<Message>("editMessageText", request, ct);
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

        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new TelegramApiException($"Request failed with status code: {response.StatusCode}",
                (int)response.StatusCode);
        }

        var result = JsonSerializer.Deserialize<TelegramResponse<TResponse>>(
            body,
            _jsonOptions);

        if (result?.Ok != true || result.Result == null)
        {
            throw new TelegramApiException(
                result?.Description ?? "Unknown error",
                result?.ErrorCode ?? 0);
        }

        return result.Result;
    }
}