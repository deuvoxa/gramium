using System.Text.Json.Serialization;
using Gramium.Core.Entities.Common;

namespace Gramium.Core.Entities.Messages;

public class Message
{
    [JsonPropertyName("message_id")]
    public long MessageId { get; set; }

    [JsonPropertyName("from")]
    public User? From { get; set; }

    [JsonPropertyName("chat")]
    public Chat Chat { get; set; } = null!;

    [JsonPropertyName("date")]
    public long Date { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("photo")]
    public PhotoSize[]? Photo { get; set; }

    [JsonPropertyName("document")]
    public Document? Document { get; set; }

    [JsonPropertyName("video")]
    public Video? Video { get; set; }

    [JsonPropertyName("caption")]
    public string? Caption { get; set; }
}