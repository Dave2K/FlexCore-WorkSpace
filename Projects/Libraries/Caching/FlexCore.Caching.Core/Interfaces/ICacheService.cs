namespace FlexCore.Caching.Core.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration);
        Task<bool> RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task ClearAllAsync();
    }
}