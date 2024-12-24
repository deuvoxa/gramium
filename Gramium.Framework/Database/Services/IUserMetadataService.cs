namespace Gramium.Framework.Database.Services;

public interface IUserMetadataService
{
    Task<string?> GetMetadataValueAsync(long telegramId, string key);
    Task<bool> SetMetadataAsync(long telegramId, string key, string value);
    Task<bool> RemoveMetadataAsync(long telegramId, string key);
    Task<bool> HasMetadataAsync(long telegramId, string key);
    Task<IDictionary<string, string>> GetAllMetadataAsync(long telegramId);
} 