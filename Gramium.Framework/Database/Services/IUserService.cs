using Gramium.Framework.Database.Entities;
using Gramium.Framework.Database.Enums;

namespace Gramium.Framework.Database.Services;

public interface IUserService
{
    Task<TelegramUser?> GetUserAsync(long telegramId);

    Task<TelegramUser> CreateOrUpdateUserAsync(long telegramId, string firstName, string? username = null,
        string? languageCode = null);

    Task<bool> UpdateAccessLevelAsync(long telegramId, AccessLevel newLevel);
    Task<AccessLevel> GetAccessLevelAsync(long telegramId);
    Task<bool> HasAccessLevelAsync(long telegramId, AccessLevel requiredLevel);
    Task SaveChangesAsync();
}