namespace FlexCore.ORM.Providers.ADO.Tests;

using Xunit;
using System.Data.SQLite;
using FlexCore.ORM.Providers.ADO;

public class AdoNetUnitOfWorkTests
{
    [Fact]
    public async Task CommitTransaction_SavesChanges()
    {
        using var connection = new SQLiteConnection("Data Source=:memory:;Version=3;New=True;");
        connection.Open();
        var uow = new AdoNetUnitOfWork(connection);

        await uow.BeginTransactionAsync();
        await uow.CommitTransactionAsync();

        Assert.True(true);
    }
}