namespace Gramium.Examples.BudgetManager.Entities;

public enum PaymentType
{
    RegularExpense,
    Credit,
    Debt
}

public class RegularPayment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public PaymentType PaymentType { get; set; }
    public decimal Amount { get; set; }
    public int PaymentDueDate { get; set; }
    public decimal? Debt { get; set; }
    public string Description { get; set; }
    
    public User User { get; set; }
}