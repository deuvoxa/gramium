namespace Gramium.Examples.BudgetManager.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }

    public User User { get; set; }
    public Account Account { get; set; }
}

public enum TransactionType
{
    Income,
    Expense
}