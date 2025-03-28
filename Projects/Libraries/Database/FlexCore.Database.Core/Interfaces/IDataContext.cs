namespace FlexCore.Database.Interfaces;

using System;
using System.Threading.Tasks;

/// <summary>
/// Interfaccia astratta per l'accesso ai dati, indipendente dal provider di database.
/// </summary>
public interface IDataContext : IDisposable
{
    /// <summary>
    /// Avvia una transazione nel database.
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// Conferma la transazione in corso.
    /// </summary>
    void CommitTransaction();

    /// <summary>
    /// Annulla la transazione in corso.
    /// </summary>
    void RollbackTransaction();

    /// <summary>
    /// Salva le modifiche nel database.
    /// </summary>
    /// <returns>Numero di record modificati.</returns>
    int SaveChanges();

    /// <summary>
    /// Salva le modifiche nel database in modo asincrono.
    /// </summary>
    /// <returns>Numero di record modificati.</returns>
    Task<int> SaveChangesAsync();
}