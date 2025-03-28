namespace FlexCore.ORM.Core.Interfaces
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Definisce le operazioni di base per una unità di lavoro, inclusi il supporto per le transazioni.
    /// Implementa i metodi per iniziare, confermare e annullare una transazione, nonché per salvare le modifiche.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
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

        /// <summary>
        /// Salva le modifiche effettuate nel contesto e restituisce il numero di modifiche salvate.
        /// </summary>
        /// <returns>Un task che rappresenta l'operazione asincrona e restituisce il numero di modifiche.</returns>
        Task<int> SaveChangesAsync();
    }
}
