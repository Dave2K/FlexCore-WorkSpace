using System;

namespace FlexCore.Caching.Redis
{
    /// <summary>
    /// Rappresenta un errore specifico verificatosi durante le operazioni Redis
    /// </summary>
    /// <remarks>
    /// Estende <see cref="Exception"/> per fornire contesto aggiuntivo sugli errori Redis
    /// </remarks>
    public class RedisCacheException : Exception
    {
        /// <summary>
        /// Inizializza una nuova istanza della classe con messaggio ed eccezione interna
        /// </summary>
        /// <param name="message">Messaggio descrittivo dell'errore</param>
        /// <param name="innerException">Eccezione originale che ha causato l'errore</param>
        public RedisCacheException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}