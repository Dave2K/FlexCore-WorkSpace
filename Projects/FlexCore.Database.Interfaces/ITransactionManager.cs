namespace FlexCore.Database.Interfaces;

using System.Threading.Tasks;

/// <summary>
/// Interfaccia per la gestione delle transazioni.
/// </summary>
public interface ITransactionManager
{
    /// <summary>
    /// Avvia una transazione asincrona.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Conferma la transazione corrente.
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Annulla la transazione corrente.
    /// </summary>
    Task RollbackTransactionAsync();

    /// <summary>
    /// Avvia una transazione distribuita asincrona.
    /// </summary>
    Task BeginDistributedTransactionAsync();
}