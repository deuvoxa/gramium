using Gramium.Examples.BudgetManager.Entities;

namespace Gramium.Examples.BudgetManager.Extensions;

public static class UserExtensions
{
    public static List<Account> GetActiveAccount(this User user)
    {
        return user.Accounts.Where(a => a.IsActive).ToList();
    }
}