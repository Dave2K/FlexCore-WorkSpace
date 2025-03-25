#nullable enable

namespace FlexCore.ORM.Providers.Dapper;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using FlexCore.ORM.Core.Interfaces;
using global::Dapper.Contrib.Extensions;

/// <summary>
/// Implementazione del provider ORM basato su Dapper.
/// Gestisce operazioni di lettura, scrittura e transazioni su un database tramite Dapper.
/// </summary>
public class DapperOrmProvider : IOrmProvider
{
    // Connessione al database
    private readonly IDbConnection _connection;
    // Transazione associata alla connessione
    private IDbTransaction? _transaction = null;

    /// <summary>
    /// Inizializza una nuova istanza di <see cref="DapperOrmProvider"/>.
    /// </summary>
    /// <param name="connection">La connessione al database utilizzata da Dapper.</param>
    public DapperOrmProvider(IDbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    /// <summary>
    /// Recupera un'entità dal database tramite il suo identificativo.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entità da recuperare.</typeparam>
    /// <param name="id">L'identificativo dell'entità.</param>
    /// <returns>L'entità recuperata, oppure null se non trovata.</returns>
    public async Task<T?> GetByIdAsync<T>(Guid id) where T : class
    {
        return await _connection.GetAsync<T>(id);
    }

    /// <summary>
    /// Recupera tutte le entità di tipo <typeparamref name="T"/> dal database.
    /// </summary>
    /// <typeparam name="T">Il tipo delle entità da recuperare.</typeparam>
    /// <returns>Una sequenza di entità di tipo <typeparamref name="T"/>.</returns>
    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
    {
        return await _connection.GetAllAsync<T>();
    }

    /// <summary>
    /// Aggiunge una nuova entità al database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entità da aggiungere.</typeparam>
    /// <param name="entity">L'entità da aggiungere.</param>
    public async Task AddAsync<T>(T entity) where T : class
    {
        await _connection.InsertAsync(entity);
    }

    /// <summary>
    /// Aggiunge un insieme di entità al database.
    /// </summary>
    /// <typeparam name="T">Il tipo delle entità da aggiungere.</typeparam>
    /// <param name="entities">Le entità da aggiungere.</param>
    public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        await _connection.InsertAsync(entities);
    }

    /// <summary>
    /// Aggiorna un'entità esistente nel database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entità da aggiornare.</typeparam>
    /// <param name="entity">L'entità da aggiornare.</param>
    public async Task UpdateAsync<T>(T entity) where T : class
    {
        await _connection.UpdateAsync(entity);
    }

    /// <summary>
    /// Aggiorna un insieme di entità esistenti nel database.
    /// </summary>
    /// <typeparam name="T">Il tipo delle entità da aggiornare.</typeparam>
    /// <param name="entities">Le entità da aggiornare.</param>
    public async Task UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        await _connection.UpdateAsync(entities);
    }

    /// <summary>
    /// Elimina un'entità dal database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entità da eliminare.</typeparam>
    /// <param name="entity">L'entità da eliminare.</param>
    public async Task DeleteAsync<T>(T entity) where T : class
    {
        await _connection.DeleteAsync(entity);
    }

    /// <summary>
    /// Elimina un insieme di entità dal database.
    /// </summary>
    /// <typeparam name="T">Il tipo delle entità da eliminare.</typeparam>
    /// <param name="entities">Le entità da eliminare.</param>
    public async Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        await _connection.DeleteAsync(entities);
    }

    /// <summary>
    /// Metodo per salvare le modifiche. Dapper non gestisce SaveChanges come EF Core, quindi restituisce sempre 0.
    /// </summary>
    /// <returns>Restituisce sempre 0, in quanto Dapper non gestisce direttamente le modifiche come EF Core.</returns>
    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0); // Dapper non gestisce SaveChanges come EFCore
    }

    /// <summary>
    /// Avvia una nuova transazione asincrona.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task BeginTransactionAsync()
    {
        // Verifica se la connessione è aperta, altrimenti la apre
        if (_connection.State != ConnectionState.Open)
            await Task.Run(() => _connection.Open());

        // Inizia una nuova transazione
        _transaction = await Task.Run(() => _connection.BeginTransaction());
    }

    /// <summary>
    /// Commette la transazione corrente.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    /// <exception cref="InvalidOperationException">Se non esiste una transazione attiva.</exception>
    public async Task CommitTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Nessuna transazione attiva.");

        await Task.Run(() => _transaction.Commit());
        _transaction = null;
    }

    /// <summary>
    /// Annulla la transazione corrente.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    /// <exception cref="InvalidOperationException">Se non esiste una transazione attiva.</exception>
    public async Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Nessuna transazione attiva.");

        await Task.Run(() => _transaction.Rollback());
        _transaction = null;
    }

    /// <summary>
    /// Rilascia le risorse utilizzate dal provider.
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
        GC.SuppressFinalize(this);
    }
}
