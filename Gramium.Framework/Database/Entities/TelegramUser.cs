using Gramium.Framework.Database.Enums;

namespace Gramium.Framework.Database.Entities;

public class TelegramUser
{
    public long Id { get; set; }
    public long TelegramId { get; set; }
    public string? Username { get; set; }
    public string? LanguageCode { get; set; }
    public AccessLevel AccessLevel { get; set; } = AccessLevel.User;

    public virtual ICollection<UserState> States { get; set; } = new List<UserState>();
    public virtual ICollection<UserMetadata> Metadata { get; set; } = new List<UserMetadata>();
}