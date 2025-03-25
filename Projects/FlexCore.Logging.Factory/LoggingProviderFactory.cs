namespace FlexCore.Logging.Factory;

using FlexCore.Logging.Interfaces;
using System;
using System.Collections.Generic;

/// <summary>
/// Factory per la creazione e registrazione dinamica dei provider di logging.
/// </summary>
public class LoggingProviderFactory : ILoggingFactory
{
    private readonly Dictionary<string, Func<ILoggingProvider>> _providers = new();

    /// <summary>
    /// Registra un provider di logging con un nome specifico.
    /// </summary>
    /// <param name="name">Nome del provider (es. "Console", "Log4Net", "Serilog").</param>
    /// <param name="providerFactory">Funzione che crea un'istanza del provider.</param>
    public void RegisterProvider(string name, Func<ILoggingProvider> providerFactory)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Il nome del provider non può essere vuoto.", nameof(name));

        if (providerFactory == null)
            throw new ArgumentNullException(nameof(providerFactory), "La factory del provider non può essere nulla.");

        _providers[name] = providerFactory;
    }

    /// <summary>
    /// Crea un'istanza di un provider di logging in base al nome.
    /// </summary>
    /// <param name="providerName">Nome del provider da creare.</param>
    /// <returns>Un'istanza del provider di logging.</returns>
    public ILoggingProvider CreateProvider(string providerName)
    {
        if (_providers.TryGetValue(providerName, out var providerFactory))
        {
            return providerFactory();
        }
        throw new NotSupportedException($"Provider '{providerName}' non supportato.");
    }
}