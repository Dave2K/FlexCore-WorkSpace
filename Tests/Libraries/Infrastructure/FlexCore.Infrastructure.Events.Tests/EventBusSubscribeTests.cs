// File: EventBusSubscribeTests.cs
// Path: FlexCore-Workspace/Tests/Libraries/Infrastructure/FlexCore.Infrastructure.Events.Tests/EventBusSubscribeTests.cs

using Xunit;
using Moq;
using FlexCore.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace FlexCore.Infrastructure.Events.Tests
{
    /// <summary>
    /// Test suite per il metodo <see cref="EventBus.Subscribe{TEvent}(IEventHandler{TEvent})"/>
    /// </summary>
    public class EventBusSubscribeTests
    {
        /// <summary>
        /// Evento di test per i casi d'uso generici
        /// </summary>
        public class TestEvent : IEvent { }

        /// <summary>
        /// Verifica che l'handler venga aggiunto correttamente alla lista degli subscribers
        /// </summary>
        [Fact(DisplayName = "Subscribe_ConHandlerValido_DovrebbeAggiungereAllaLista")]
        public void Subscribe_ValidHandler_ShouldAddToList()
        {
            // Arrange
            var eventBus = new EventBus(Mock.Of<IServiceProvider>());
            var mockHandler = new Mock<IEventHandler<TestEvent>>().Object;

            // Act
            eventBus.Subscribe(mockHandler);

            // Assert
            var subscribers = GetSubscribers(eventBus);
            Assert.Contains(mockHandler, subscribers);
        }

        /// <summary>
        /// Verifica che venga lanciata un'eccezione se si passa un handler nullo
        /// </summary>
        [Fact(DisplayName = "Subscribe_ConHandlerNullo_DovrebbeLanciareArgumentNullException")]
        public void Subscribe_NullHandler_ShouldThrowArgumentNullException()
        {
            // Arrange
            var eventBus = new EventBus(Mock.Of<IServiceProvider>());

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => eventBus.Subscribe<TestEvent>(null!)
            );

            Assert.Equal("handler", exception.ParamName);
        }

        /// <summary>
        /// Verifica che multipli handler possano essere registrati per lo stesso evento
        /// </summary>
        [Fact(DisplayName = "Subscribe_MultipliHandler_DovrebbeAggiungereTuttiAllaLista")]
        public void Subscribe_MultipleHandlers_ShouldAddAllToList()
        {
            // Arrange
            var eventBus = new EventBus(Mock.Of<IServiceProvider>());
            var handler1 = new Mock<IEventHandler<TestEvent>>().Object;
            var handler2 = new Mock<IEventHandler<TestEvent>>().Object;

            // Act
            eventBus.Subscribe(handler1);
            eventBus.Subscribe(handler2);

            // Assert
            var subscribers = GetSubscribers(eventBus);
            Assert.Contains(handler1, subscribers);
            Assert.Contains(handler2, subscribers);
            Assert.Equal(2, subscribers.Count);
        }

        /// <summary>
        /// Helper method per accedere alla lista privata _subscribers tramite reflection
        /// </summary>
        private static List<object> GetSubscribers(EventBus eventBus)
        {
            var field = typeof(EventBus).GetField(
                "_subscribers",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            return (List<object>)field!.GetValue(eventBus)!;
        }
    }
}