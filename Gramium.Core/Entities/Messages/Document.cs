using System.Text.Json.Serialization;

public class Document
{
    [JsonPropertyName("file_id")]
    public string FileId { get; set; } = null!;

    [JsonPropertyName("file_unique_id")]
    public string FileUniqueId { get; set; } = null!;

    [JsonPropertyName("file_name")]
    public string? FileName { get; set; }

    [JsonPropertyName("mime_type")]
    public string? MimeType { get; set; }

    [JsonPropertyName("file_size")]
    public long? FileSize { get; set; }
} 