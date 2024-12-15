using System.Text.Json.Serialization;

namespace Gramium.Core.Entities.Common;

public class User
{
    public long Id { get; set; }

    [JsonPropertyName("first_name")] public string FirstName { get; set; } = null!;

    public string? Username { get; set; }

    [JsonPropertyName("language_code")] public string? LanguageCode { get; set; }

    [JsonPropertyName("is_premium")] public bool IsPremium { get; set; }
}