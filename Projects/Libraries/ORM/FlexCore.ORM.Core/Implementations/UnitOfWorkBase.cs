namespace FlexCore.ORM.Core.Implementations
{
    using System;

    /// <summary>
    /// Classe astratta che fornisce le basi per l'implementazione di un'unità di lavoro.
    /// Gestisce la transazione e il salvataggio delle modifiche in un contesto di dati.
    /// </summary>
    public abstract class UnitOfWorkBase : IDisposable
    {
        // Flag per tenere traccia se l'oggetto è stato già rilasciato.
        protected bool _disposed;

        /// <summary>
        /// Avvia una transazione in modo asincrono.
        /// </summary>
        /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
        public abstract Task BeginTransactionAsync();

        /// <summary>
        /// Commette la transazione in corso in modo asincrono.
        /// </summary>
        /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
        public abstract Task CommitTransactionAsync();

        /// <summary>
        /// Annulla la transazione in corso in modo asincrono.
        /// </summary>
        /// <returns>Un task che rappresenta l'operazione asincrona.</returns>
        public abstract Task RollbackTransactionAsync();

        /// <summary>
        /// Salva le modifiche effettuate nel contesto di dati in modo asincrono.
        /// </summary>
        /// <returns>Un task che rappresenta l'operazione asincrona, restituendo il numero di entità salvate.</returns>
        public abstract Task<int> SaveChangesAsync();

        /// <summary>
        /// Esegue la logica di rilascio delle risorse.
        /// Questo metodo è protetto e deve essere implementato dalle classi derivate.
        /// </summary>
        /// <param name="disposing">Indica se il metodo è stato chiamato tramite il metodo Dispose o dal finalizzatore.</param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Rilascia le risorse quando l'oggetto non è più necessario.
        /// </summary>
        public void Dispose()
        {
            // Non chiamare Dispose più di una volta.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
