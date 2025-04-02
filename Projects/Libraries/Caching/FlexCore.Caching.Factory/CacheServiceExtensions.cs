using Microsoft.Extensions.DependencyInjection;
using FlexCore.Caching.Core.Interfaces;

namespace FlexCore.Caching.Factory
{
    /// <summary>
    /// Provides extension methods for configuring caching services.
    /// </summary>
    public static class CacheServiceExtensions
    {
        /// <summary>
        /// Adds a cache factory and related services to the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The modified service collection.</returns>
        public static IServiceCollection AddCacheFactory(this IServiceCollection services)
        {
            services.AddSingleton<ICacheFactory, CacheProviderFactory>(); // Nome corretto della classe
            return services;
        }
    }
}