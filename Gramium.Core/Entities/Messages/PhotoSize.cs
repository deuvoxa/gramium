using System.Text.Json.Serialization;

public class PhotoSize
{
    [JsonPropertyName("file_id")]
    public string FileId { get; set; } = null!;

    [JsonPropertyName("file_unique_id")]
    public string FileUniqueId { get; set; } = null!;

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("file_size")]
    public long? FileSize { get; set; }
} 