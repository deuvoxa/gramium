namespace Gramium.Core.Entities.Common;

public class User
{
    public long Id { get; set; }
    public bool IsBot { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? LanguageCode { get; set; }
    public bool? IsPremium { get; set; }
} 