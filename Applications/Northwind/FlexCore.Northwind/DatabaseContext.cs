namespace FlexCore.Northwind;

/// <summary>
/// Classe astratta per la gestione delle operazioni di accesso ai dati.
/// Supporta Entity Framework, Dapper e ADO.NET.
/// </summary>
public abstract class DatabaseContext
{
    /// <summary>
    /// Esegue una query SQL e restituisce i risultati come una lista di oggetti.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità di destinazione.</typeparam>
    /// <param name="query">Query SQL da eseguire.</param>
    /// <param name="parameters">Parametri opzionali per la query.</param>
    /// <returns>Lista di oggetti di tipo <typeparamref name="T"/>.</returns>
    public abstract Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, object? parameters = null);

    /// <summary>
    /// Esegue un comando SQL (INSERT, UPDATE, DELETE).
    /// </summary>
    /// <param name="query">Comando SQL da eseguire.</param>
    /// <param name="parameters">Parametri opzionali per il comando.</param>
    /// <returns>Numero di righe interessate.</returns>
    public abstract Task<int> ExecuteCommandAsync(string query, object? parameters = null);
}
