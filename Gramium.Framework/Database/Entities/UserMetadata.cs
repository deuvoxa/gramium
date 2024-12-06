namespace Gramium.Framework.Database.Entities;

public class UserMetadata
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
    
    public virtual TelegramUser User { get; set; } = null!;
} 