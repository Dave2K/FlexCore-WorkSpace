using Xunit;
using FlexCore.Database.Core;
using System;
using System.Transactions;
using System.Threading.Tasks;

namespace FlexCore.Database.Core.Tests
{
    public class TransactionManagerTests
    {
        [Fact]
        public async Task BeginTransactionAsync_ShouldNotThrowException()
        {
            var manager = new TransactionManager();
            var exception = await Record.ExceptionAsync(() => manager.BeginTransactionAsync());
            Assert.Null(exception);
        }

        [Fact]
        public async Task CommitTransactionAsync_ShouldNotThrowException()
        {
            var manager = new TransactionManager();
            await manager.BeginTransactionAsync();
            var exception = await Record.ExceptionAsync(() => manager.CommitTransactionAsync());
            Assert.Null(exception);
        }

        [Fact]
        public async Task RollbackTransactionAsync_ShouldNotThrowException()
        {
            var manager = new TransactionManager();
            await manager.BeginTransactionAsync();
            var exception = await Record.ExceptionAsync(() => manager.RollbackTransactionAsync());
            Assert.Null(exception);
        }

        [Fact]
        public async Task BeginDistributedTransactionAsync_ShouldNotThrowException()
        {
            var manager = new TransactionManager();
            var exception = await Record.ExceptionAsync(() => manager.BeginDistributedTransactionAsync());
            Assert.Null(exception);
        }
    }
}
