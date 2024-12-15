using Gramium.Framework.Database.Entities;
using Gramium.Framework.Database.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gramium.Framework.Database.Services;

public class UserService(GramiumDbContext context) : IUserService
{
    public async Task<TelegramUser?> GetUserAsync(long telegramId)
    {
        return await context.Users
            .Include(u => u.States)
            .FirstOrDefaultAsync(u => u.TelegramId == telegramId);
    }

    public async Task<TelegramUser> CreateOrUpdateUserAsync(long telegramId, string firstName, string? username = null,
        string? languageCode = null)
    {
        var user = await GetUserAsync(telegramId);

        if (user == null)
        {
            user = new TelegramUser
            {
                TelegramId = telegramId,
                Username = username,
                LanguageCode = languageCode,
                AccessLevel = AccessLevel.User
            };
            context.Users.Add(user);
        }
        else
        {
            user.Username = username;
            user.LanguageCode = languageCode;
        }

        await context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAccessLevelAsync(long telegramId, AccessLevel newLevel)
    {
        var user = await GetUserAsync(telegramId);
        if (user == null) return false;

        user.AccessLevel = newLevel;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<AccessLevel> GetAccessLevelAsync(long telegramId)
    {
        var user = await GetUserAsync(telegramId);
        return user?.AccessLevel ?? AccessLevel.User;
    }

    public async Task<bool> HasAccessLevelAsync(long telegramId, AccessLevel requiredLevel)
    {
        var userLevel = await GetAccessLevelAsync(telegramId);
        return userLevel >= requiredLevel;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}