using Microsoft.Extensions.Logging;

namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>
    /// Eccezione specifica per errori nella MemoryCache
    /// </summary>
    public class MemoryCacheException : CacheException
    {
        public MemoryCacheException(
            ILogger<MemoryCacheException> logger,
            string message,
            Exception inner)
            : base(message, inner)
        {
            logger.LogError(inner, message);
        }
    }
}