namespace Gramium.Framework.Database.Entities;

public class UserState
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
    public DateTime? ExpiresAt { get; set; }

    public virtual TelegramUser User { get; set; } = null!;
}