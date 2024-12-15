using System.Text.Json.Serialization;
using Gramium.Core.Entities.Common;
using Gramium.Core.Entities.Messages;

namespace Gramium.Core.Entities.Callbacks;

public class CallbackQuery
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;

    [JsonPropertyName("from")] public User From { get; set; } = null!;

    [JsonPropertyName("message")] public Message? Message { get; set; }

    [JsonPropertyName("data")] public string? Data { get; set; }
}