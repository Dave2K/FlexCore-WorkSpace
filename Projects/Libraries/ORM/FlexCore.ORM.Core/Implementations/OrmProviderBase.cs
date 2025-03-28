namespace FlexCore.ORM.Core.Implementations;

using FlexCore.ORM.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Classe base astratta per i provider ORM.
/// </summary>
public abstract class OrmProviderBase : IOrmProvider
{
    // Flag per indicare se le risorse sono gi� state rilasciate
    private bool _disposed = false;

    /// <summary>
    /// Inizializza il provider ORM.
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Esegue una query SQL sul database.
    /// </summary>
    /// <param name="query">La query SQL da eseguire.</param>
    public abstract void ExecuteQuery(string query);

    /// <summary>
    /// Recupera un'entit� dal database tramite il suo ID.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit�.</typeparam>
    /// <param name="id">L'ID dell'entit� da recuperare.</param>
    /// <returns>L'entit� trovata o null se non esiste.</returns>
    public abstract Task<T?> GetByIdAsync<T>(Guid id) where T : class;

    /// <summary>
    /// Recupera tutte le entit� di un determinato tipo dal database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit�.</typeparam>
    /// <returns>Una collezione di entit�.</returns>
    public abstract Task<IEnumerable<T>> GetAllAsync<T>() where T : class;

    /// <summary>
    /// Aggiunge una nuova entit� al database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit�.</typeparam>
    /// <param name="entity">L'entit� da aggiungere.</param>
    public abstract Task AddAsync<T>(T entity) where T : class;

    /// <summary>
    /// Aggiunge una collezione di entit� al database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit�.</typeparam>
    /// <param name="entities">Le entit� da aggiungere.</param>
    public abstract Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class;

    /// <summary>
    /// Aggiorna un'entit� esistente nel database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit�.</typeparam>
    /// <param name="entity">L'entit� da aggiornare.</param>
    public abstract Task UpdateAsync<T>(T entity) where T : class;

    /// <summary>
    /// Aggiorna una collezione di entit� nel database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit�.</typeparam>
    /// <param name="entities">Le entit� da aggiornare.</param>
    public abstract Task UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class;

    /// <summary>
    /// Elimina un'entit� dal database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit�.</typeparam>
    /// <param name="entity">L'entit� da eliminare.</param>
    public abstract Task DeleteAsync<T>(T entity) where T : class;

    /// <summary>
    /// Elimina una collezione di entit� dal database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entit�.</typeparam>
    /// <param name="entities">Le entit� da eliminare.</param>
    public abstract Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class;

    /// <summary>
    /// Salva tutte le modifiche apportate al database.
    /// </summary>
    /// <returns>Il numero di entit� modificate.</returns>
    public abstract Task<int> SaveChangesAsync();

    /// <summary>
    /// Inizia una nuova transazione asincrona.
    /// </summary>
    public abstract Task BeginTransactionAsync();

    /// <summary>
    /// Conferma la transazione corrente.
    /// </summary>
    public abstract Task CommitTransactionAsync();

    /// <summary>
    /// Annulla la transazione corrente.
    /// </summary>
    public abstract Task RollbackTransactionAsync();

    /// <summary>
    /// Rilascia le risorse gestite e non gestite.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Rilascia le risorse gestite e non gestite.
    /// </summary>
    /// <param name="disposing">Indica se rilasciare risorse gestite.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Rilascia le risorse gestite (ad esempio, connessioni al database)
            }

            // Rilascia le risorse non gestite (se presenti)
            _disposed = true;
        }
    }

    /// <summary>
    /// Distruttore per garantire il rilascio delle risorse.
    /// </summary>
    ~OrmProviderBase()
    {
        Dispose(false);
    }
}