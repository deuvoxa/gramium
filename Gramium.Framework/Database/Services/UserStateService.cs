using Gramium.Framework.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gramium.Framework.Database.Services;

public class UserStateService(GramiumDbContext context) : IUserStateService
{
    public async Task<string?> GetStateValueAsync(long telegramId, string key)
    {
        var state = await context.States
            .FirstOrDefaultAsync(s => s.User.TelegramId == telegramId && s.Key == key);
            
        if (state == null) return null;

        if (!state.ExpiresAt.HasValue || state.ExpiresAt.Value >= DateTime.UtcNow) return state.Value;
        context.States.Remove(state);
        await context.SaveChangesAsync();
        return null;

    }

    public async Task<bool> SetStateAsync(long telegramId, string key, string value, DateTime? expiresAt = null)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.TelegramId == telegramId);
        if (user == null) return false;

        var state = await context.States
            .FirstOrDefaultAsync(s => s.UserId == user.Id && s.Key == key);

        if (state == null)
        {
            state = new UserState
            {
                UserId = user.Id,
                Key = key,
                Value = value,
                ExpiresAt = expiresAt
            };
            context.States.Add(state);
        }
        else
        {
            state.Value = value;
            state.ExpiresAt = expiresAt;
        }

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveStateAsync(long telegramId, string key)
    {
        var state = await context.States
            .FirstOrDefaultAsync(s => s.User.TelegramId == telegramId && s.Key == key);
            
        if (state == null) return false;
        
        context.States.Remove(state);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasStateAsync(long telegramId, string key)
    {
        var state = await context.States
            .FirstOrDefaultAsync(s => s.User.TelegramId == telegramId && s.Key == key);
            
        if (state == null) return false;

        if (!state.ExpiresAt.HasValue || state.ExpiresAt.Value >= DateTime.UtcNow) return true;
        context.States.Remove(state);
        await context.SaveChangesAsync();
        return false;

    }

    public async Task RemoveExpiredStatesAsync()
    {
        var expiredStates = await context.States
            .Where(s => s.ExpiresAt != null && s.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();
            
        context.States.RemoveRange(expiredStates);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserState>> GetUserStatesAsync(long telegramId)
    {
        return await context.States
            .Where(s => s.User.TelegramId == telegramId)
            .Where(s => s.ExpiresAt == null || s.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();
    }
} 