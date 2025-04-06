using FlexCore.Caching.Common.Validators;
using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FlexCore.Caching.Core
{
    /// <summary>
    /// Provides base functionality for cache management with common validation and error handling
    /// </summary>
    /// <remarks>
    /// Implements core caching operations using the Template Method pattern:
    /// <list type="bullet">
    /// <item><description>Centralized key validation</description></item>
    /// <item><description>Consistent error logging</description></item>
    /// <item><description>Provider abstraction</description></item>
    /// <item><description>Async/await best practices</description></item>
    /// </list>
    /// </remarks>
    public abstract class BaseCacheManager
    {
        protected readonly ILogger _logger;
        protected readonly ICacheProvider _cacheProvider;

        /// <summary>
        /// Initializes a new instance of the cache manager
        /// </summary>
        /// <param name="logger">Logger instance for tracing</param>
        /// <param name="cacheProvider">Concrete cache provider</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if logger or cacheProvider is null
        /// </exception>
        protected BaseCacheManager(ILogger logger, ICacheProvider cacheProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger is required");
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider), "Cache provider is required");

            _logger.LogDebug("BaseCacheManager initialized");
        }

        /// <summary>
        /// Checks existence of a key in the cache
        /// </summary>
        /// <param name="key">Key to verify</param>
        /// <returns>
        /// Task returning true if key exists, false otherwise
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown for invalid key format
        /// </exception>
        public virtual async Task<bool> ExistsAsync(string key)
        {
            ValidateKey(key);

            try
            {
                _logger.LogDebug("Checking key existence: {Key}", key);
                return await _cacheProvider.ExistsAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Key existence check failed for {Key}", key);
                throw;
            }
        }

        /// <summary>
        /// Validates cache key format using configured rules
        /// </summary>
        /// <param name="key">Key to validate</param>
        /// <exception cref="ArgumentException">
        /// Thrown for empty keys or invalid format
        /// </exception>
        protected virtual void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException(
                    "Cache key cannot be empty or whitespace",
                    nameof(key));
            }

            if (!CacheKeyValidator.ValidateKey(key))
            {
                throw new ArgumentException(
                    $"Invalid key format: {key}. Valid characters: a-Z, 0-9, -, _ (max 128 chars)",
                    nameof(key));
            }
        }
    }
}