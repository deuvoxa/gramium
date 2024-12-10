using Gramium.Framework.Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gramium.Framework.Database;

public abstract class BaseDbContext(string connectionString, DatabaseProvider provider) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        switch (provider)
        {
            case DatabaseProvider.Postgresql:
                optionsBuilder.UseNpgsql(connectionString);
                break;
            case DatabaseProvider.SqlServer:
            case DatabaseProvider.MySql:
            default:
                throw new ArgumentException($"Неподдерживаемый провайдер базы данных: {provider}");
        }
    }
}