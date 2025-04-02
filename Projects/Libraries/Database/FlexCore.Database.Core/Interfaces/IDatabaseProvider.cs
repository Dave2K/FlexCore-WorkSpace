using System.Data;
using System.Threading.Tasks;

namespace FlexCore.Database.Core.Interfaces
{
    /// <summary>
    /// Interfaccia base per tutti i provider di database.
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Apre una connessione al database.
        /// </summary>
        /// <param name="connectionString">Stringa di connessione.</param>
        void Connect(string connectionString);

        /// <summary>
        /// Esegue una query e restituisce un IDataReader.
        /// </summary>
        /// <param name="query">Comando SQL da eseguire.</param>
        IDataReader ExecuteQuery(string query);

        /// <summary>
        /// Esegue un comando non query (es. INSERT, UPDATE, DELETE).
        /// </summary>
        /// <param name="command">Comando SQL da eseguire.</param>
        int ExecuteNonQuery(string command);

        /// <summary>
        /// Chiude la connessione al database.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Crea un parametro per query parametrizzate.
        /// </summary>
        /// <param name="name">Nome del parametro.</param>
        /// <param name="value">Valore del parametro.</param>
        IDbDataParameter CreateParameter(string name, object value);

        /// <summary>
        /// Crea una connessione al database senza aprirla.
        /// </summary>
        /// <param name="connectionString">Stringa di connessione.</param>
        IDbConnection CreateConnection(string connectionString);

        /// <summary>
        /// Apre una connessione in modo asincrono.
        /// </summary>
        /// <param name="connection">Connessione da aprire.</param>
        Task OpenConnectionAsync(IDbConnection connection);
    }
}