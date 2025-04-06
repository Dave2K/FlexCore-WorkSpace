// File: EventBusConstructorTests.cs
// Path: FlexCore-Workspace/Tests/Libraries/Infrastructure/FlexCore.Infrastructure.Events.Tests/EventBusConstructorTests.cs

using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FlexCore.Infrastructure.Events.Tests
{
    /// <summary>
    /// Test per il costruttore della classe <see cref="EventBus"/>
    /// </summary>
    public class EventBusConstructorTests
    {
        /// <summary>
        /// Verifica che il costruttore lanci un'eccezione se il service provider è nullo
        /// </summary>
        [Fact(DisplayName = "Constructor_ConServiceProviderNullo_DovrebbeLanciareArgumentNullException")]
        public void Constructor_NullServiceProvider_ShouldThrowArgumentNullException()
        {
            // Arrange
            IServiceProvider nullServiceProvider = null!;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => new EventBus(nullServiceProvider)
            );

            Assert.Equal("serviceProvider", exception.ParamName);
        }
    }
}