namespace FlexCore.Database.Factory.Extensions;

using Microsoft.Extensions.DependencyInjection;
using FlexCore.Database.Factory;
using FlexCore.Database.Interfaces;

/// <summary>
/// Estensioni per la configurazione del provider di database tramite Dependency Injection.
/// </summary>
public static class DatabaseServiceExtensions
{
    /// <summary>
    /// Registra un provider di database nel container DI.
    /// </summary>
    /// <param name="services">Collezione di servizi.</param>
    /// <param name="providerName">Nome del provider (es. "SQLServer").</param>
    /// <param name="connectionString">Stringa di connessione al database.</param>
    /// <returns>La collezione di servizi per il method chaining.</returns>
    public static IServiceCollection AddDatabaseProvider(
        this IServiceCollection services,
        string providerName,
        string connectionString)
    {
        services.AddSingleton<IDatabaseProviderFactory, DatabaseProviderFactory>();
        services.AddSingleton<IDbConnectionFactory>(sp =>
        {
            var factory = sp.GetRequiredService<IDatabaseProviderFactory>();
            return factory.CreateProvider(providerName, connectionString);
        });
        return services;
    }
}