using Microsoft.Extensions.Logging;

namespace FlexCore.Caching.Core.Interfaces
{
    /// <summary>
    /// Interfaccia base per i provider di cache
    /// </summary>
    public interface ICacheProvider
    {
        bool Exists(string key);
        T? Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan expiry);
        void Remove(string key);
        void ClearAll();
    }
}