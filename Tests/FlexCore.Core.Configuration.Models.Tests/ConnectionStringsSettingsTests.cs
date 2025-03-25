using Xunit;
using FlexCore.Core.Configuration.Models;

namespace FlexCore.Core.Configuration.Models.Tests;

public class ConnectionStringsSettingsTests
{
    [Fact]
    public void ConnectionStringsSettings_Should_Initialize_Correctly()
    {
        var settings = new ConnectionStringsSettings { DefaultDatabase = "TestDB", SQLiteDatabase = "TestSQLite", Redis = "TestRedis" };

        Assert.Equal("TestDB", settings.DefaultDatabase);
        Assert.Equal("TestSQLite", settings.SQLiteDatabase);
        Assert.Equal("TestRedis", settings.Redis);
    }
}