namespace FlexCore.Database.SQLite.Tests
{
    using Xunit;
    using FlexCore.Database.Factory;
    using FlexCore.Database.Interfaces;
    using Moq;

    public class DatabaseProviderFactoryTests
    {
        [Fact]
        public void CreateProvider_ShouldReturnProvider()
        {
            // Arrange
            var factory = new DatabaseProviderFactory();
            var providerName = "SQLite";
            var connectionString = "Data Source=:memory:";
            var mockFactory = new Mock<Func<string, IDbConnectionFactory>>();
            mockFactory.Setup(f => f(connectionString)).Returns(new Mock<IDbConnectionFactory>().Object);

            factory.RegisterProvider(providerName, mockFactory.Object);

            // Act
            var provider = factory.CreateProvider(providerName, connectionString);

            // Assert
            Assert.NotNull(provider);
        }

        [Fact]
        public void CreateProvider_WithUnsupportedProvider_ShouldThrowException()
        {
            // Arrange
            var factory = new DatabaseProviderFactory();
            var providerName = "UnsupportedProvider";
            var connectionString = "Data Source=:memory:";

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => factory.CreateProvider(providerName, connectionString));
        }
    }
}
