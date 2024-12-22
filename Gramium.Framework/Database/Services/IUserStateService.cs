using Gramium.Framework.Database.Entities;

namespace Gramium.Framework.Database.Services;

public interface IUserStateService
{
    Task<string?> GetStateValueAsync(long telegramId, string key);
    Task<bool> SetStateAsync(long telegramId, string key, string value, DateTime? expiresAt = null);
    Task<bool> RemoveStateAsync(long telegramId, string key);
    Task<bool> HasStateAsync(long telegramId, string key);
    Task RemoveExpiredStatesAsync();
    Task<IEnumerable<UserState>> GetUserStatesAsync(long telegramId);
} 