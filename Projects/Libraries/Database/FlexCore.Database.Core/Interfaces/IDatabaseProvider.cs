using System.Data;
using System.Threading.Tasks;

namespace FlexCore.Database.Core.Interfaces
{
    /// <summary>
    /// Interfaccia base per tutti i provider di database
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Apre una connessione al database
        /// </summary>
        /// <param name="connectionString">Stringa di connessione</param>
        void Connect(string connectionString);

        /// <summary>
        /// Chiude la connessione corrente
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Esegue un comando SQL senza parametri
        /// </summary>
        int ExecuteNonQuery(string command);

        /// <summary>
        /// Esegue un comando SQL con parametri
        /// </summary>
        int ExecuteNonQuery(string command, params IDbDataParameter[] parameters);

        /// <summary>
        /// Esegue una query SQL senza parametri
        /// </summary>
        IDataReader ExecuteQuery(string query);

        /// <summary>
        /// Esegue una query SQL con parametri
        /// </summary>
        IDataReader ExecuteQuery(string query, params IDbDataParameter[] parameters);

        /// <summary>
        /// Esegue una query e restituisce un singolo valore
        /// </summary>
        T ExecuteScalar<T>(string query);

        /// <summary>
        /// Crea un parametro per query
        /// </summary>
        IDbDataParameter CreateParameter(string name, object value);

        /// <summary>
        /// Crea una connessione non aperta
        /// </summary>
        IDbConnection CreateConnection(string connectionString);

        /// <summary>
        /// Verifica se è presente una transazione attiva
        /// </summary>
        bool IsTransactionActive();

        /// <summary>
        /// Avvia una nuova transazione
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Conferma la transazione corrente
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Annulla la transazione corrente
        /// </summary>
        void RollbackTransaction();
    }
}