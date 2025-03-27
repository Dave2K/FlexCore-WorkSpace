namespace FlexCore.Database.Interfaces;

using System.Data;

/// <summary>
/// Interfaccia per la gestione della connessione al database.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Crea e restituisce una connessione aperta al database.
    /// </summary>
    /// <returns>Connessione al database.</returns>
    IDbConnection CreateConnection();
}