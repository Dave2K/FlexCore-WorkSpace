using System;

namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>
    /// Eccezione base per tutti gli errori relativi alla cache
    /// </summary>
    public class CacheException : Exception
    {
        /// <summary>
        /// Inizializza una nuova istanza della classe
        /// </summary>
        public CacheException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}