namespace FlexCore.Logging.Factory;

using Microsoft.Extensions.DependencyInjection;
using FlexCore.Logging.Interfaces;

/// <summary>
/// Estensioni per la configurazione del provider di logging.
/// </summary>
public static class LoggingServiceExtensions
{
    /// <summary>
    /// Registra un provider di logging in base al nome specificato.
    /// </summary>
    /// <param name="services">La collection di servizi DI.</param>
    /// <param name="providerName">Il nome del provider di logging.</param>
    /// <returns>IServiceCollection con il provider registrato.</returns>
    public static IServiceCollection AddLoggingProvider(this IServiceCollection services, string providerName)
    {
        services.AddSingleton<ILoggingFactory>(sp => new LoggingProviderFactory());
        services.AddSingleton<ILoggingProvider>(sp => sp.GetRequiredService<ILoggingFactory>().CreateProvider(providerName));
        return services;
    }
}