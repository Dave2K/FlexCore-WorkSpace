namespace FlexCore.ORM.Providers.Dapper;

using System.Data;
using FlexCore.ORM.Core;
using System.Threading.Tasks;
using System;

/// <summary>
/// Implementazione del UnitOfWork per Dapper. Gestisce le transazioni e l'accesso al database tramite Dapper.
/// </summary>
public class DapperUnitOfWork : UnitOfWorkBase
{
    // Connessione al database
    private readonly IDbConnection _connection;
    // Transazione associata alla connessione
    private IDbTransaction _transaction;

    /// <summary>
    /// Inizializza una nuova istanza di <see cref="DapperUnitOfWork"/>.
    /// </summary>
    /// <param name="connection">La connessione al database utilizzata da Dapper.</param>
    public DapperUnitOfWork(IDbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    /// <summary>
    /// Avvia una nuova transazione asincrona.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public override async Task BeginTransactionAsync()
    {
        // Verifica se la connessione è aperta, altrimenti la apre
        if (_connection.State != ConnectionState.Open)
            _connection.Open();

        // Inizia una nuova transazione
        _transaction = _connection.BeginTransaction();
        await Task.CompletedTask;
    }

    /// <summary>
    /// Commette la transazione corrente.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public override async Task CommitTransactionAsync()
    {
        // Commette la transazione se non è null
        _transaction?.Commit();
        _transaction = null;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Annulla la transazione corrente.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public override async Task RollbackTransactionAsync()
    {
        // Annulla la transazione se non è null
        _transaction?.Rollback();
        _transaction = null;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Metodo per salvare le modifiche. Dapper non gestisce SaveChanges come EF Core, quindi restituisce sempre 0.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona. Restituisce 0.</returns>
    public override async Task<int> SaveChangesAsync()
    {
        // Dapper non ha una gestione diretta di SaveChanges, quindi restituisce sempre 0
        return await Task.FromResult(0);
    }

    /// <summary>
    /// Rilascia le risorse utilizzate dal UnitOfWork.
    /// </summary>
    /// <param name="disposing">Indica se il metodo è stato chiamato dal Dispose o dal finalizzatore.</param>
    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Rilascia le risorse gestite
                _transaction?.Dispose();
            }

            // Rilascia le risorse non gestite (connessione al database)
            _connection?.Dispose();
            _disposed = true;
        }
    }
}
