using System.Threading.Tasks;

namespace FlexCore.Caching.Core.Interfaces
{
    /// <summary>  
    /// Definisce il contratto per un servizio di cache con operazioni asincrone.  
    /// </summary>  
    public interface ICacheService
    {
        /// <summary>  
        /// Ottiene un valore dalla cache in modo asincrono.  
        /// </summary>  
        Task<T?> GetAsync<T>(string key);

        /// <summary>  
        /// Memorizza un valore nella cache con scadenza in modo asincrono.  
        /// </summary>  
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expiration);

        /// <summary>  
        /// Rimuove una chiave dalla cache in modo asincrono.  
        /// </summary>  
        Task<bool> RemoveAsync(string key);

        /// <summary>  
        /// Verifica l'esistenza di una chiave nella cache in modo asincrono.  
        /// </summary>  
        Task<bool> ExistsAsync(string key);

        /// <summary>  
        /// Svuota completamente la cache in modo asincrono.  
        /// </summary>  
        Task ClearAllAsync();
    }
}