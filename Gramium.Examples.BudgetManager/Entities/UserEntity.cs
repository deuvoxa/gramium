namespace Gramium.Examples.BudgetManager.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public long TelegramId { get; set; }
    public string FirstName { get; set; } = null!;
    public bool IsPremium { get; set; }
}