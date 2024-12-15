namespace Gramium.Examples.BudgetManager.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required long TelegramId { get; set; }
    public string FirstName { get; set; } = null!;


    public ICollection<RegularPayment> RegularPayments { get; set; } = [];
    public ICollection<Account> Accounts { get; set; } = [];
    public ICollection<Transaction> Transactions { get; set; } = [];
}