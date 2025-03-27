namespace FlexCore.Database.MariaDB;

using FlexCore.Database.Core;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

public class MariaDBProvider : DbConnectionFactory
{
    public MariaDBProvider(string connectionString) : base(connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));
    }

    public override IDbConnection CreateConnection()
    {
        var connection = new MySqlConnection(_connectionString);
        connection.Open();
        return connection;
    }

    public override async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}