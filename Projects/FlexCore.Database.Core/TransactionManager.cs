namespace FlexCore.Database.Core;

using FlexCore.Database.Interfaces;
using System.Transactions;
using System.Threading.Tasks;

/// <summary>
/// Gestore delle transazioni.
/// </summary>
public class TransactionManager : ITransactionManager
{
    /// <summary>
    /// Avvia una transazione asincrona.
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        await Task.CompletedTask; // Implementazione di esempio
    }

    /// <summary>
    /// Conferma la transazione corrente.
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        await Task.CompletedTask; // Implementazione di esempio
    }

    /// <summary>
    /// Annulla la transazione corrente.
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        await Task.CompletedTask; // Implementazione di esempio
    }

    /// <summary>
    /// Avvia una transazione distribuita asincrona.
    /// </summary>
    public async Task BeginDistributedTransactionAsync()
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            scope.Complete();
            await Task.CompletedTask;
        }
    }
}