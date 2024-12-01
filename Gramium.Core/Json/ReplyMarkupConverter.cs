using System.Text.Json;
using System.Text.Json.Serialization;
using Gramium.Core.Entities.Markup;

namespace Gramium.Core.Json;

public class ReplyMarkupConverter : JsonConverter<IReplyMarkup>
{
    public override IReplyMarkup Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IReplyMarkup value, JsonSerializerOptions options)
    {
        if (value is InlineKeyboardMarkup markup)
        {
            JsonSerializer.Serialize(writer, markup, options);
        }
        else
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }
    }
}