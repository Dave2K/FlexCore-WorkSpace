using FlexCore.Database.Core.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FlexCore.Database.SQLServer
{
    /// <summary>
    /// Provider per l'interazione con Microsoft SQL Server
    /// </summary>
    public class SQLServerDatabaseProvider : IDatabaseProvider
    {
        private SqlConnection? _connection;
        private SqlTransaction? _currentTransaction;

        /// <inheritdoc/>
        public void Connect(string connectionString)
        {
            Disconnect();
            _connection = new SqlConnection(connectionString);
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
            using var cmd = new SqlCommand(command, _connection, _currentTransaction);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteNonQuery();
        }

        /// <inheritdoc/>
        public IDataReader ExecuteQuery(string query)
            => ExecuteQuery(query, Array.Empty<IDbDataParameter>());

        /// <inheritdoc/>
        public IDataReader ExecuteQuery(string query, params IDbDataParameter[] parameters)
        {
            var cmd = new SqlCommand(query, _connection, _currentTransaction);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader();
        }

        /// <inheritdoc/>
        public T ExecuteScalar<T>(string query)
        {
            using var cmd = new SqlCommand(query, _connection, _currentTransaction);
            object result = cmd.ExecuteScalar();

            return result == null || result == DBNull.Value
                ? throw new InvalidOperationException("Il risultato della query è nullo")
                : (T)Convert.ChangeType(result, typeof(T));
        }

        /// <inheritdoc/>
        public IDbDataParameter CreateParameter(string name, object value)
            => new SqlParameter(name, value);

        /// <inheritdoc/>
        public IDbConnection CreateConnection(string connectionString)
            => new SqlConnection(connectionString);

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
            if (connection is SqlConnection sqlConn)
                await sqlConn.OpenAsync();
            else
                throw new ArgumentException("Tipo di connessione non supportato", nameof(connection));
        }
    }
}