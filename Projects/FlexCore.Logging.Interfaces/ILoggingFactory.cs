namespace FlexCore.Logging.Interfaces;

/// <summary>
/// Interfaccia per la factory che crea i provider di logging.
/// </summary>
public interface ILoggingFactory
{
    /// <summary>
    /// Crea un'istanza di un provider di logging in base al nome.
    /// </summary>
    /// <param name="providerName">Nome del provider da creare.</param>
    /// <returns>Un'istanza del provider di logging.</returns>
    ILoggingProvider CreateProvider(string providerName);
}