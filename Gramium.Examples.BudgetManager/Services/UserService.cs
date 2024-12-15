using Gramium.Examples.BudgetManager.Database;
using Gramium.Examples.BudgetManager.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gramium.Examples.BudgetManager.Services;

public class UserService(BudgetManagerDbContext context)
{
    public async Task<User?> GetUserByTelegramIdAsync(long userId)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.TelegramId == userId);
    }

    public async Task<User> AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        await context.SaveChangesAsync();
        return user;
    }
}