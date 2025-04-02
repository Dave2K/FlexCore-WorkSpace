namespace FlexCore.Caching.Core.Interfaces
{
    /// <summary>
    /// Defines a factory for creating cache providers.
    /// </summary>
    public interface ICacheFactory
    {
        /// <summary>
        /// Creates a cache provider with the specified name.
        /// </summary>
        /// <param name="name">The name of the cache provider.</param>
        /// <returns>An instance of the cache provider.</returns>
        ICacheProvider CreateCacheProvider(string name);
    }
}