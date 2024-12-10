using Gramium.Framework.Database.Enums;
using Microsoft.EntityFrameworkCore.Design;

namespace Gramium.Examples.BudgetManager.Database;

public class BudgetManagerDbContextFactory : IDesignTimeDbContextFactory<BudgetManagerDbContext>
{
    public BudgetManagerDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException(
                                   "Connection string 'DefaultConnection' not found.");

        return new BudgetManagerDbContext(connectionString, DatabaseProvider.Postgresql);
    }
}