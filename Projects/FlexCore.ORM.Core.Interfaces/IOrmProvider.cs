namespace FlexCore.ORM.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Interfaccia per le operazioni CRUD generiche su un database.
/// Include metodi per il recupero, l'aggiunta, l'aggiornamento e la cancellazione di entit�.
/// Inoltre gestisce le transazioni attraverso i metodi di commit, rollback e salvataggio delle modifiche.
/// </summary>
public interface IOrmProvider : IDisposable
{
    /// <summary>
    /// Recupera un'entit� dal database in base all'ID.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entit�.</typeparam>
    /// <param name="id">L'ID dell'entit� da recuperare.</param>
    /// <returns>Un task che restituisce l'entit� se trovata, altrimenti null.</returns>
    Task<T?> GetByIdAsync<T>(Guid id) where T : class;

    /// <summary>
    /// Recupera tutte le entit� di tipo <typeparamref name="T"/> dal database.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entit�.</typeparam>
    /// <returns>Un task che restituisce una raccolta di entit�.</returns>
    Task<IEnumerable<T>> GetAllAsync<T>() where T : class;

    /// <summary>
    /// Aggiunge una nuova entit� al database.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entit�.</typeparam>
    /// <param name="entity">L'entit� da aggiungere.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    Task AddAsync<T>(T entity) where T : class;

    /// <summary>
    /// Aggiunge un intervallo di entit� al database.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entit�.</typeparam>
    /// <param name="entities">Le entit� da aggiungere.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class;

    /// <summary>
    /// Aggiorna un'entit� esistente nel database.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entit�.</typeparam>
    /// <param name="entity">L'entit� da aggiornare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    Task UpdateAsync<T>(T entity) where T : class;

    /// <summary>
    /// Aggiorna un intervallo di entit� esistenti nel database.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entit�.</typeparam>
    /// <param name="entities">Le entit� da aggiornare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    Task UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class;

    /// <summary>
    /// Elimina un'entit� dal database.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entit�.</typeparam>
    /// <param name="entity">L'entit� da eliminare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    Task DeleteAsync<T>(T entity) where T : class;

    /// <summary>
    /// Elimina un intervallo di entit� dal database.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entit�.</typeparam>
    /// <param name="entities">Le entit� da eliminare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class;

    /// <summary>
    /// Salva le modifiche effettuate nel contesto.
    /// </summary>
    /// <returns>Un task che restituisce il numero di modifiche salvate.</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Avvia una nuova transazione asincrona.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    Task BeginTransactionAsync();

    /// <summary>
    /// Conferma la transazione attiva asincrona.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    Task CommitTransactionAsync();

    /// <summary>
    /// Annulla la transazione attiva asincrona.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    Task RollbackTransactionAsync();
}
