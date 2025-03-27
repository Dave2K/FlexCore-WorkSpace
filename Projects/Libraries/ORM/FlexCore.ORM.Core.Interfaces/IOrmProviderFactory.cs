namespace FlexCore.ORM.Core.Interfaces;

/// <summary>
/// Interfaccia per la factory che crea istanze di provider ORM.
/// </summary>
public interface IOrmProviderFactory
{
    /// <summary>
    /// Crea un'istanza di un provider ORM in base al nome del provider e alla stringa di connessione.
    /// </summary>
    /// <param name="providerName">Il nome del provider ORM (es. "EFCore", "Dapper", "ADO").</param>
    /// <param name="connectionString">La stringa di connessione al database.</param>
    /// <returns>Un'istanza del provider ORM.</returns>
    /// <exception cref="NotSupportedException">Se il provider specificato non è supportato.</exception>
    IOrmProvider CreateProvider(string providerName, string connectionString);
}