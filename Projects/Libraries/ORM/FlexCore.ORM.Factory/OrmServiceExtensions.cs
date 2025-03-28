namespace FlexCore.ORM.Factory;

using Microsoft.Extensions.DependencyInjection;
using FlexCore.ORM.Core.Interfaces;

/// <summary>
/// Estensioni per la configurazione del provider ORM.
/// </summary>
public static class OrmServiceExtensions
{
    /// <summary>
    /// Registra un provider ORM in base al nome e alla stringa di connessione.
    /// </summary>
    /// <param name="services">La collection di servizi DI.</param>
    /// <param name="providerName">Il nome del provider ORM.</param>
    /// <param name="connectionString">La stringa di connessione al database.</param>
    /// <returns>IServiceCollection con il provider registrato.</returns>
    public static IServiceCollection AddOrmProvider(this IServiceCollection services, string providerName, string connectionString)
    {
        services.AddSingleton<IOrmProviderFactory>(sp => new OrmProviderFactory());
        services.AddSingleton<IOrmProvider>(sp => sp.GetRequiredService<IOrmProviderFactory>().CreateProvider(providerName, connectionString));
        return services;
    }
}
