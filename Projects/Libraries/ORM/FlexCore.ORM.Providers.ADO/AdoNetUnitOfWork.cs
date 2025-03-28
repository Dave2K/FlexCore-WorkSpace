namespace FlexCore.ORM.Providers.ADO;

using System;
using System.Data;
using System.Threading.Tasks;
using FlexCore.ORM.Core.Implementations;

/// <summary>
/// Implementa il pattern UnitOfWork utilizzando ADO.NET per la gestione delle transazioni.
/// Gestisce le operazioni di transazione come commit e rollback per una connessione ADO.NET.
/// </summary>
public class AdoNetUnitOfWork : UnitOfWorkBase
{
    private readonly IDbConnection _connection;
    private IDbTransaction? _transaction = null;

    /// <summary>
    /// Inizializza una nuova istanza di <see cref="AdoNetUnitOfWork"/>.
    /// </summary>
    /// <param name="connection">La connessione al database.</param>
    /// <exception cref="ArgumentNullException">L'argomento <paramref name="connection"/> è nullo.</exception>
    public AdoNetUnitOfWork(IDbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    /// <summary>
    /// Avvia una nuova transazione sul database.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    public override async Task BeginTransactionAsync()
    {
        if (_connection.State != ConnectionState.Open)
            await Task.Run(() => _connection.Open());

        _transaction = await Task.Run(() => _connection.BeginTransaction());
    }

    /// <summary>
    /// Conferma la transazione attiva.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    /// <exception cref="InvalidOperationException">Se non c'è alcuna transazione attiva.</exception>
    public override async Task CommitTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Nessuna transazione attiva.");

        await Task.Run(() => _transaction.Commit());
        _transaction = null;
    }

    /// <summary>
    /// Annulla la transazione attiva.
    /// </summary>
    /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
    /// <exception cref="InvalidOperationException">Se non c'è alcuna transazione attiva.</exception>
    public override async Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Nessuna transazione attiva.");

        await Task.Run(() => _transaction.Rollback());
        _transaction = null;
    }

    /// <summary>
    /// Salva le modifiche effettuate nel contesto. Non implementa operazioni come Entity Framework, quindi ritorna sempre 0.
    /// </summary>
    /// <returns>Un intero che rappresenta il numero di modifiche salvate.</returns>
    public override Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0); // ADO.NET non gestisce SaveChanges come Entity Framework.
    }

    /// <summary>
    /// Rilascia le risorse utilizzate dalla connessione e dalla transazione.
    /// </summary>
    /// <param name="disposing">Indica se il metodo è stato chiamato dal metodo Dispose pubblico o dal finalizzatore.</param>
    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
            }

            _connection?.Dispose();
            _disposed = true;
        }
    }
}
