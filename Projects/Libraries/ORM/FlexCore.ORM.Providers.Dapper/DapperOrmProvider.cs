#nullable enable
using Dapper;
using Dapper.Contrib.Extensions;
using FlexCore.ORM.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FlexCore.ORM.Providers.Dapper;

/// <summary>
/// Implementazione del provider ORM basato su Dapper.Contrib.
/// Gestisce la mappatura degli oggetti, le transazioni e le operazioni CRUD.
/// </summary>
public class DapperOrmProvider : IOrmProvider, IDisposable
{
    private readonly IDbConnection _connection;
    private IDbTransaction? _transaction;

    /// <summary>
    /// Inizializza i gestori di tipo per i GUID.
    /// </summary>
    static DapperOrmProvider()
    {
        SqlMapper.RemoveTypeMap(typeof(Guid));
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
    }

    /// <summary>
    /// Inizializza una nuova istanza del provider Dapper.
    /// </summary>
    /// <param name="connection">Connessione al database già configurata</param>
    /// <exception cref="ArgumentNullException">Se la connessione è null</exception>
    public DapperOrmProvider(IDbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    /// <summary>
    /// Gestore personalizzato per la conversione GUID/stringa.
    /// </summary>
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        /// <summary>
        /// Converte una stringa in GUID.
        /// </summary>
        /// <param name="value">Valore da convertire</param>
        /// <returns>Guid parsato</returns>
        /// <exception cref="FormatException">Se il formato non è valido</exception>
        public override Guid Parse(object value) => Guid.Parse(value.ToString()!);

        /// <summary>
        /// Imposta il parametro SQL con il GUID formattato come stringa.
        /// </summary>
        /// <param name="parameter">Parametro SQL da impostare</param>
        /// <param name="value">Valore GUID da convertire</param>
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            parameter.DbType = DbType.String;
            parameter.Value = value.ToString("N");
        }
    }

    /// <summary>
    /// Recupera un'entità tramite ID.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <param name="id">ID univoco dell'entità</param>
    /// <returns>Task che restituisce l'entità o null</returns>
    public async Task<T?> GetByIdAsync<T>(Guid id) where T : class =>
        await _connection.GetAsync<T>(id);

    /// <summary>
    /// Recupera tutte le entità di tipo T.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <returns>Task che restituisce una collezione di entità</returns>
    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class =>
        await _connection.GetAllAsync<T>();

    /// <summary>
    /// Aggiunge una nuova entità al database.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <param name="entity">Entità da aggiungere</param>
    /// <returns>Task completato</returns>
    public async Task AddAsync<T>(T entity) where T : class =>
        await _connection.InsertAsync(entity);

    /// <summary>
    /// Aggiunge una collezione di entità al database.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <param name="entities">Collezione di entità da aggiungere</param>
    /// <returns>Task completato</returns>
    public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class =>
        await _connection.InsertAsync(entities);

    /// <summary>
    /// Aggiorna un'entità esistente.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <param name="entity">Entità con i valori aggiornati</param>
    /// <returns>Task completato</returns>
    public async Task UpdateAsync<T>(T entity) where T : class =>
        await _connection.UpdateAsync(entity);

    /// <summary>
    /// Aggiorna una collezione di entità.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <param name="entities">Collezione di entità da aggiornare</param>
    /// <returns>Task completato</returns>
    public async Task UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class =>
        await _connection.UpdateAsync(entities);

    /// <summary>
    /// Elimina un'entità dal database.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <param name="entity">Entità da eliminare</param>
    /// <returns>Task completato</returns>
    public async Task DeleteAsync<T>(T entity) where T : class =>
        await _connection.DeleteAsync(entity);

    /// <summary>
    /// Elimina una collezione di entità.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    /// <param name="entities">Collezione di entità da eliminare</param>
    /// <returns>Task completato</returns>
    public async Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class =>
        await _connection.DeleteAsync(entities);

    /// <summary>
    /// Salva le modifiche (non necessario in Dapper).
    /// </summary>
    /// <returns>Task che restituisce sempre 0</returns>
    public Task<int> SaveChangesAsync() => Task.FromResult(0);

    /// <summary>
    /// Avvia una transazione asincrona.
    /// </summary>
    /// <returns>Task completato</returns>
    /// <exception cref="InvalidOperationException">Se la connessione non è aperta</exception>
    public async Task BeginTransactionAsync()
    {
        if (_connection.State != ConnectionState.Open)
            throw new InvalidOperationException("La connessione deve essere aperta");

        _transaction = await Task.Run(() => _connection.BeginTransaction());
    }

    /// <summary>
    /// Conferma la transazione corrente.
    /// </summary>
    /// <returns>Task completato</returns>
    /// <exception cref="InvalidOperationException">Se non c'è una transazione attiva</exception>
    public async Task CommitTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Nessuna transazione attiva");

        await Task.Run(() => _transaction.Commit());
        _transaction = null;
    }

    /// <summary>
    /// Annulla la transazione corrente.
    /// </summary>
    /// <returns>Task completato</returns>
    /// <exception cref="InvalidOperationException">Se non c'è una transazione attiva</exception>
    public async Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Nessuna transazione attiva");

        await Task.Run(() => _transaction.Rollback());
        _transaction = null;
    }

    /// <summary>
    /// Rilascia le risorse gestite (connessione e transazione).
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
        GC.SuppressFinalize(this);
    }
}