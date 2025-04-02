namespace FlexCore.Caching.Common.Exceptions
{
    /// <summary>
    /// Eccezione base per gli errori di caching
    /// </summary>
    public class CacheException : Exception
    {
        public CacheException(string message, Exception inner)
            : base(message, inner) { }
    }
}