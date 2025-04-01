namespace FlexCore.Database.MariaDB.Tests;

using Xunit;
using FlexCore.Database.MariaDB;
using System;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using WorkSpace.Generated;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Test per la classe <see cref="MariaDBProvider"/>.
/// </summary>
public class MariaDBProviderTests
{
    private readonly string _connectionString;

    public MariaDBProviderTests()
    {
        _connectionString = GetConnectionString();
    }

    private static string GetConnectionString()
    {
        string resourcesFolder = Enviroment.ResourcesFolder;
        var configuration = new ConfigurationBuilder()
           .SetBasePath(resourcesFolder)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
        string? connString = configuration["DatabaseSettings:MariaDB:ConnectionString"];

        if (string.IsNullOrEmpty(connString))
        {
            throw new InvalidOperationException("ConnectionString non trovata in appsettings.json");
        }

        return connString;
    }
    /// <summary>
    /// Verifica che il costruttore sollevi un'eccezione se la stringa di connessione è nulla o vuota.
    /// </summary>
    [Fact]
    public void Constructor_ThrowsOnNullOrEmptyConnectionString()
    {
        Assert.Throws<ArgumentNullException>(() => new MariaDBProvider(null!));
        Assert.Throws<ArgumentNullException>(() => new MariaDBProvider(""));
    }

    /// <summary>
    /// Verifica che CreateConnection restituisca una connessione aperta.
    /// </summary>
    [Fact]
    public void CreateConnection_ReturnsOpenConnection()
    {
        var provider = new MariaDBProvider(_connectionString);
        using var connection = provider.CreateConnection();

        Assert.Equal(ConnectionState.Open, connection.State);
        Assert.IsType<MySqlConnection>(connection);
    }

    /// <summary>
    /// Verifica che CreateConnectionAsync restituisca una connessione aperta.
    /// </summary>
    [Fact]
    public async Task CreateConnectionAsync_ReturnsOpenConnection()
    {
        var provider = new MariaDBProvider(_connectionString);
        using var connection = await provider.CreateConnectionAsync();

        Assert.Equal(ConnectionState.Open, connection.State);
        Assert.IsType<MySqlConnection>(connection);
    }

    /// <summary>
    /// Verifica che CreateConnection sollevi un'eccezione per una stringa di connessione non valida.
    /// </summary>
    [Fact]
    public void CreateConnection_ThrowsOnInvalidConnectionString()
    {
        Assert.Throws<ArgumentException>(() => new MariaDBProvider("invalid_connection_string").CreateConnection());
    }
}
