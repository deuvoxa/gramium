using Gramium.Examples.BudgetManager.Entities;
using Gramium.Framework.Database;
using Gramium.Framework.Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gramium.Examples.BudgetManager.Database;

public class BudgetManagerDbContext(string connectionString, DatabaseProvider provider)
    : BaseDbContext(connectionString, provider)
{
    public DbSet<User> Users { get; set; } = null!;
}