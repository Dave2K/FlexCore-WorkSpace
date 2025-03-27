#nullable enable

namespace FlexCore.ORM.Providers.ADO;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using FlexCore.ORM.Core.Interfaces;

/// <summary>
/// Fornisce le operazioni di accesso ai dati utilizzando ADO.NET per l'ORM.
/// Implementa le operazioni CRUD e le transazioni.
/// </summary>
public class AdoOrmProvider : IOrmProvider
{
    private readonly DbConnection _connection;
    private DbTransaction? _transaction = null;

    /// <summary>
    /// Inizializza una nuova istanza di <see cref="AdoOrmProvider"/>.
    /// </summary>
    /// <param name="connection">La connessione al database.</param>
    /// <exception cref="ArgumentNullException">L'argomento <paramref name="connection"/> è nullo.</exception>
    public AdoOrmProvider(DbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    /// <summary>
    /// Recupera un'entità dal database in base all'ID.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entità.</typeparam>
    /// <param name="id">L'ID dell'entità da recuperare.</param>
    /// <returns>L'entità corrispondente all'ID specificato, oppure <c>null</c> se non trovata.</returns>
    public async Task<T?> GetByIdAsync<T>(Guid id) where T : class
    {
        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        using var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM {typeof(T).Name} WHERE Id = @Id";
        var param = command.CreateParameter();
        param.ParameterName = "@Id";
        param.Value = id;
        command.Parameters.Add(param);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var entity = Activator.CreateInstance<T>();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (reader[prop.Name] != DBNull.Value)
                {
                    prop.SetValue(entity, reader[prop.Name]);
                }
            }
            return entity;
        }

        return null;
    }

    /// <summary>
    /// Recupera tutte le entità di un tipo dal database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entità.</typeparam>
    /// <returns>Una lista di entità.</returns>
    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
    {
        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        var entities = new List<T>();
        using var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM {typeof(T).Name}";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var entity = Activator.CreateInstance<T>();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (reader[prop.Name] != DBNull.Value)
                {
                    prop.SetValue(entity, reader[prop.Name]);
                }
            }
            entities.Add(entity);
        }

        return entities;
    }

    /// <summary>
    /// Aggiunge un'entità al database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entità da aggiungere.</typeparam>
    /// <param name="entity">L'entità da aggiungere.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task AddAsync<T>(T entity) where T : class
    {
        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        using var command = _connection.CreateCommand();
        var columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
        var values = string.Join(", ", typeof(T).GetProperties().Select(p => $"@{p.Name}"));

        command.CommandText = $"INSERT INTO {typeof(T).Name} ({columns}) VALUES ({values})";

        foreach (var prop in typeof(T).GetProperties())
        {
            var param = command.CreateParameter();
            param.ParameterName = $"@{prop.Name}";
            param.Value = prop.GetValue(entity) ?? DBNull.Value;
            command.Parameters.Add(param);
        }

        await command.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Aggiunge un intervallo di entità al database.
    /// </summary>
    /// <typeparam name="T">Il tipo delle entità da aggiungere.</typeparam>
    /// <param name="entities">Le entità da aggiungere.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        foreach (var entity in entities)
        {
            await AddAsync(entity);
        }
    }

    /// <summary>
    /// Aggiorna un'entità nel database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entità da aggiornare.</typeparam>
    /// <param name="entity">L'entità da aggiornare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task UpdateAsync<T>(T entity) where T : class
    {
        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        using var command = _connection.CreateCommand();
        var updates = string.Join(", ", typeof(T).GetProperties().Select(p => $"{p.Name} = @{p.Name}"));

        command.CommandText = $"UPDATE {typeof(T).Name} SET {updates} WHERE Id = @Id";

        foreach (var prop in typeof(T).GetProperties())
        {
            var param = command.CreateParameter();
            param.ParameterName = $"@{prop.Name}";
            param.Value = prop.GetValue(entity) ?? DBNull.Value;
            command.Parameters.Add(param);
        }

        await command.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Aggiorna un intervallo di entità nel database.
    /// </summary>
    /// <typeparam name="T">Il tipo delle entità da aggiornare.</typeparam>
    /// <param name="entities">Le entità da aggiornare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task UpdateRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        foreach (var entity in entities)
        {
            await UpdateAsync(entity);
        }
    }

    /// <summary>
    /// Elimina un'entità dal database.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'entità da eliminare.</typeparam>
    /// <param name="entity">L'entità da eliminare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task DeleteAsync<T>(T entity) where T : class
    {
        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();

        using var command = _connection.CreateCommand();
        command.CommandText = $"DELETE FROM {typeof(T).Name} WHERE Id = @Id";

        var idParam = command.CreateParameter();
        idParam.ParameterName = "@Id";
        idParam.Value = typeof(T).GetProperty("Id")?.GetValue(entity) ?? throw new InvalidOperationException("Entità senza proprietà 'Id'.");
        command.Parameters.Add(idParam);

        await command.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Elimina un intervallo di entità dal database.
    /// </summary>
    /// <typeparam name="T">Il tipo delle entità da eliminare.</typeparam>
    /// <param name="entities">Le entità da eliminare.</param>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task DeleteRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        foreach (var entity in entities)
        {
            await DeleteAsync(entity);
        }
    }

    /// <summary>
    /// Esegue un salvataggio delle modifiche nel contesto.
    /// </summary>
    /// <returns>Un intero che rappresenta il numero di modifiche salvate.</returns>
    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0); // ADO.NET non gestisce SaveChanges come EFCore
    }

    /// <summary>
    /// Avvia una transazione sul database.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public async Task BeginTransactionAsync()
    {
        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();
        _transaction = await _connection.BeginTransactionAsync();
    }

    /// <summary>
    /// Conferma una transazione attiva.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    /// <exception cref="InvalidOperationException">Se non c'è alcuna transazione attiva.</exception>
    public Task CommitTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Nessuna transazione attiva.");

        _transaction.Commit();
        _transaction = null;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Annulla una transazione attiva.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    /// <exception cref="InvalidOperationException">Se non c'è alcuna transazione attiva.</exception>
    public Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Nessuna transazione attiva.");

        _transaction.Rollback();
        _transaction = null;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Libera le risorse utilizzate dalla connessione e dalla transazione.
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }
}
