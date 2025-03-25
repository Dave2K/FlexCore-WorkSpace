namespace FlexCore.Caching.Factory;

using Microsoft.Extensions.DependencyInjection;
using FlexCore.Caching.Interfaces;

/// <summary>
/// Estensioni per la configurazione del provider di cache.
/// </summary>
public static class CacheServiceExtensions
{
    /// <summary>
    /// Registra un provider di cache in base al nome specificato.
    /// </summary>
    /// <param name="services">La collection di servizi DI.</param>
    /// <param name="providerName">Il nome del provider di cache.</param>
    /// <returns>IServiceCollection con il provider registrato.</returns>
    public static IServiceCollection AddCacheProvider(this IServiceCollection services, string providerName)
    {
        services.AddSingleton<ICacheFactory>(sp => new CacheProviderFactory());
        services.AddSingleton<ICacheProvider>(sp => sp.GetRequiredService<ICacheFactory>().CreateProvider(providerName));
        return services;
    }
}