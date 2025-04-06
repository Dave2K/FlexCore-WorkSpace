// FlexCore.Caching.Core/Interfaces/ICacheProvider.cs  
using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Core.Interfaces
{
    public interface ICacheProvider : IDisposable
    {
        Task<bool> ExistsAsync(string key);
        Task<T?> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expiry);
        Task<bool> RemoveAsync(string key);
        Task ClearAllAsync();
    }
}