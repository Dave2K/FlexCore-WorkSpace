using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using FlexCore.Database.Interfaces;
using System;

namespace FlexCore.Database.SQLServer
{
    public class SQLServerProvider : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SQLServerProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}