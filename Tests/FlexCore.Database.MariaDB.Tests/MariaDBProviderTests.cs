namespace FlexCore.Database.MariaDB.Tests;

using Xunit;
using FlexCore.Database.MariaDB;
using System;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

/// <summary>
/// Test per la classe <see cref="MariaDBProvider"/>.
/// </summary>
public class MariaDBProviderTests
{
    private const string TestConnectionString = "Server=localhost;Database=TestDb;User=root;Password=4321;";

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
        var provider = new MariaDBProvider(TestConnectionString);
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
        var provider = new MariaDBProvider(TestConnectionString);
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
