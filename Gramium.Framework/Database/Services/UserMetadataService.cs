using Gramium.Framework.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gramium.Framework.Database.Services;

public class UserMetadataService(GramiumDbContext context) : IUserMetadataService
{
    public async Task<string?> GetMetadataValueAsync(long telegramId, string key)
    {
        var metadata = await context.Metadata
            .FirstOrDefaultAsync(m => m.User.TelegramId == telegramId && m.Key == key);
        return metadata?.Value;
    }

    public async Task<bool> SetMetadataAsync(long telegramId, string key, string value)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.TelegramId == telegramId);
        if (user == null) return false;

        var metadata = await context.Metadata
            .FirstOrDefaultAsync(m => m.UserId == user.Id && m.Key == key);

        if (metadata == null)
        {
            metadata = new UserMetadata
            {
                UserId = user.Id,
                Key = key,
                Value = value
            };
            context.Metadata.Add(metadata);
        }
        else
        {
            metadata.Value = value;
        }

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveMetadataAsync(long telegramId, string key)
    {
        var metadata = await context.Metadata
            .FirstOrDefaultAsync(m => m.User.TelegramId == telegramId && m.Key == key);
            
        if (metadata == null) return false;
        
        context.Metadata.Remove(metadata);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasMetadataAsync(long telegramId, string key)
    {
        return await context.Metadata
            .AnyAsync(m => m.User.TelegramId == telegramId && m.Key == key);
    }

    public async Task<IDictionary<string, string>> GetAllMetadataAsync(long telegramId)
    {
        return await context.Metadata
            .Where(m => m.User.TelegramId == telegramId)
            .ToDictionaryAsync(m => m.Key, m => m.Value);
    }
} 