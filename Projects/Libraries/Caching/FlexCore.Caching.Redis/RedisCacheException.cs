using System;

namespace FlexCore.Caching.Redis
{
    /// <summary>
    /// Represents errors that occur during Redis cache operations.
    /// </summary>
    public class RedisCacheException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RedisCacheException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}