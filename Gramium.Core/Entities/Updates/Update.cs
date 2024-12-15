using System.Text.Json.Serialization;
using Gramium.Core.Entities.Callbacks;
using Gramium.Core.Entities.Messages;

namespace Gramium.Core.Entities.Updates;

public class Update
{
    [JsonPropertyName("update_id")] public long UpdateId { get; set; }

    [JsonPropertyName("message")] public Message? Message { get; set; }

    [JsonPropertyName("callback_query")] public CallbackQuery? CallbackQuery { get; set; }
}