namespace FlexCore.Database.Interfaces;

/// <summary>
/// Interfaccia per la gestione dell'unità di lavoro (Unit of Work).
/// Definisce i metodi per salvare le modifiche, gestire transazioni e liberare le risorse.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Salva tutte le modifiche apportate al contesto di persistenza in modo sincrono.
    /// </summary>
    /// <returns>Il numero di entità aggiornate nel database.</returns>
    int SaveChanges();

    /// <summary>
    /// Salva tutte le modifiche apportate al contesto di persistenza in modo asincrono.
    /// </summary>
    /// <returns>Un Task che rappresenta l'operazione asincrona, contenente il numero di entità aggiornate.</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Avvia una nuova transazione.
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
}
