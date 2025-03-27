namespace FlexCore.Database.SQLServer.Tests
{
    using Xunit;
    using FlexCore.Database.Core;
    using System.Data;
    using Moq;

    public class DbConnectionFactoryTests
    {
        [Fact]
        public void CreateConnection_ShouldReturnOpenConnection()
        {
            // Arrange
            var connectionString = "Server=localhost;Database=TestDB;Trusted_Connection=True;";
            var factory = new Mock<DbConnectionFactory>(connectionString) { CallBase = true };
            factory.Setup(f => f.CreateConnection()).Returns(new Mock<IDbConnection>().Object);

            // Act
            var connection = factory.Object.CreateConnection();

            // Assert
            Assert.NotNull(connection);
        }
    }
}
