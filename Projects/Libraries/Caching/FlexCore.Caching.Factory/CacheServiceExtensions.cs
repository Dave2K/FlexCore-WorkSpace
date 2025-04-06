using FlexCore.Caching.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FlexCore.Caching.Factory
{
    /// <summary>
    /// Estensioni per la configurazione centralizzata del sistema di caching
    /// </summary>
    /// <remarks>
    /// Fornisce metodi di convenienza per l'integrazione con il dependency injection container
    /// </remarks>
    public static class CacheServiceExtensions
    {
        /// <summary>
        /// Registra il factory per i provider di cache e i servizi core
        /// </summary>
        /// <param name="services">Collezione di servizi</param>
        /// <returns>Riferimento alla collezione servizi per chaining</returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="services"/> è null</exception>
        /// <example>
        /// services.AddCacheFactory().AddRedisProvider().AddMemoryCacheProvider();
        /// </example>
        public static IServiceCollection AddCacheFactory(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services), "La collection di servizi è obbligatoria");

            services.AddLogging(); // Requisito fondamentale per il tracing
            services.AddSingleton<ICacheFactory, CacheProviderFactory>();

            services.AddOptions<CacheFactoryOptions>()
                .Validate(options => options.ValidateProvidersOnStartup)
                .ValidateOnStart();

            return services;
        }

        /// <summary>
        /// Configura le opzioni avanzate del factory
        /// </summary>
        /// <param name="services">Collezione di servizi</param>
        /// <param name="configureOptions">Action di configurazione</param>
        public static void ConfigureCacheFactory(
            this IServiceCollection services,
            Action<CacheFactoryOptions> configureOptions)
        {
            services.Configure(configureOptions);
        }
    }

    /// <summary>
    /// Opzioni di configurazione per il cache factory
    /// </summary>
    /// <param name="ValidateProvidersOnStartup">
    /// Abilita la validazione di tutti i provider registrati allo startup
    /// </param>
    public class CacheFactoryOptions
    {
        public bool ValidateProvidersOnStartup { get; set; } = true;
    }
}