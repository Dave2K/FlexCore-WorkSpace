using MySqlConnector;
using System.Data;
using FlexCore.Database.Interfaces;
using FlexCore.Database.Core.Interfaces; // Aggiungi questo namespace

namespace FlexCore.Database.MariaDB
{
    /// <summary>
    /// Provider per l'interazione con database MariaDB/MySQL
    /// </summary>
    public class MariaDBDatabaseProvider : IDatabaseProvider
    {
        private MySqlConnection? _connection;
        private MySqlTransaction? _currentTransaction;

        /// <inheritdoc/>
        public void Connect(string connectionString)
        {
            Disconnect();
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }

        /// <inheritdoc/>
        public void Disconnect()
        {
            if (_connection?.State != ConnectionState.Closed)
            {
                RollbackTransaction();
                _connection?.Close();
                _connection?.Dispose();
            }
            _connection = null;
        }

        /// <inheritdoc/>
        public int ExecuteNonQuery(string command)
            => ExecuteNonQuery(command, Array.Empty<IDbDataParameter>());

        /// <inheritdoc/>
        public int ExecuteNonQuery(string command, params IDbDataParameter[] parameters)
        {
            using var cmd = new MySqlCommand(command, _connection, _currentTransaction);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteNonQuery();
        }

        /// <inheritdoc/>
        public IDataReader ExecuteQuery(string query)
            => ExecuteQuery(query, Array.Empty<IDbDataParameter>());

        /// <inheritdoc/>
        public IDataReader ExecuteQuery(string query, params IDbDataParameter[] parameters)
        {
            var cmd = new MySqlCommand(query, _connection, _currentTransaction);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader();
        }

        /// <inheritdoc/>
        public T ExecuteScalar<T>(string query)
        {
            using var cmd = new MySqlCommand(query, _connection, _currentTransaction);
            object? result = cmd.ExecuteScalar();

            return result is null || result == DBNull.Value
                ? throw new InvalidOperationException("Risultato della query è nullo o DBNull")
                : (T)Convert.ChangeType(result, typeof(T));
        }

        /// <inheritdoc/>
        public IDbDataParameter CreateParameter(string name, object? value)
            => new MySqlParameter(name, value ?? DBNull.Value);

        /// <inheritdoc/>
        public IDbConnection CreateConnection(string connectionString)
            => new MySqlConnection(connectionString);

        /// <inheritdoc/>
        public bool IsTransactionActive()
            => _currentTransaction != null;

        /// <inheritdoc/>
        public void BeginTransaction()
            => _currentTransaction = _connection?.BeginTransaction();

        /// <inheritdoc/>
        public void CommitTransaction()
        {
            _currentTransaction?.Commit();
            _currentTransaction = null;
        }

        /// <inheritdoc/>
        public void RollbackTransaction()
        {
            _currentTransaction?.Rollback();
            _currentTransaction = null;
        }

        /// <inheritdoc/>
        public async Task OpenConnectionAsync(IDbConnection connection)
        {
            if (connection is MySqlConnection mysqlConn)
                await mysqlConn.OpenAsync();
            else
                throw new ArgumentException("Tipo di connessione non supportato", nameof(connection));
        }
    }
}