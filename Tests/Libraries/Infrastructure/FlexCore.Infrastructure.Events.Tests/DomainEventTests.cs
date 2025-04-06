// File: DomainEventTests.cs
// Path: FlexCore-Workspace/Tests/Libraries/Infrastructure/FlexCore.Infrastructure.Events.Tests/DomainEventTests.cs

using Xunit;
using FlexCore.Infrastructure.Events;
using System;

namespace FlexCore.Infrastructure.Events.Tests
{
    /// <summary>
    /// Test suite per la classe <see cref="DomainEvent"/>
    /// </summary>
    public class DomainEventTests
    {
        /// <summary>
        /// Implementazione concreta di DomainEvent per testing
        /// </summary>
        private class TestDomainEvent : DomainEvent
        {
            /// <summary>
            /// Proprietà aggiuntiva per test di esempio
            /// </summary>
            public string TestProperty { get; } = "Test Value";
        }

        /// <summary>
        /// Verifica che:
        /// 1. L'evento venga istanziato correttamente
        /// 2. La proprietà OccurredOn sia inizializzata al momento della creazione
        /// 3. Il valore di OccurredOn sia in UTC
        /// </summary>
        [Fact(DisplayName = "Costruttore_DovrebbeInizializzareOccurredOnCorrettamente")]
        public void Constructor_ShouldInitializeOccurredOnCorrectly()
        {
            // Arrange
            var preCreationTime = DateTime.UtcNow;

            // Act
            var domainEvent = new TestDomainEvent();
            var postCreationTime = DateTime.UtcNow;

            // Assert
            Assert.NotNull(domainEvent);
            Assert.InRange(domainEvent.OccurredOn,
                preCreationTime.AddMilliseconds(-100), // Tolleranza per ritardi minimi
                postCreationTime.AddMilliseconds(100));
            Assert.Equal(DateTimeKind.Utc, domainEvent.OccurredOn.Kind);
        }

        /// <summary>
        /// Verifica che la classe astratta DomainEvent non possa essere istanziata direttamente
        /// </summary>
        [Fact(DisplayName = "DomainEvent_Astratta_DovrebbePrevenireIstanziazioneDiretta")]
        public void DomainEvent_AbstractClass_ShouldPreventDirectInstantiation()
        {
            // Arrange & Act
            var exception = Record.Exception(() =>
            {
                // Tentativo di creazione tramite reflection
                Activator.CreateInstance(typeof(DomainEvent), nonPublic: true);
            });

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<MissingMethodException>(exception);
            Assert.Contains("abstract", exception.Message);
        }
    }
}