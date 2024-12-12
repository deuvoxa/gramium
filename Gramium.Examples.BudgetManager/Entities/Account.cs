namespace Gramium.Examples.BudgetManager.Entities;

public class Account
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public bool IsActive { get; set; }
    public AccountType Type { get; set; }
    
    // Связи с другими сущностями
    public User User { get; set; }
}

public enum AccountType
{
    Cash,
    CreditCard,
    Savings
}