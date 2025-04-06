// File: EventBusPublishErrorTests.cs
// Path: FlexCore-Workspace/Tests/Libraries/Infrastructure/FlexCore.Infrastructure.Events.Tests/EventBusPublishErrorTests.cs

using Xunit;
using Moq;
using FlexCore.Infrastructure.Events;
using System;
using System.Threading.Tasks;

namespace FlexCore.Infrastructure.Events.Tests
{
    /// <summary>
    /// Test suite per la gestione degli errori nel metodo <see cref="EventBus.PublishAsync{TEvent}(TEvent)"/>
    /// </summary>
    public class EventBusPublishErrorTests
    {
        /// <summary>
        /// Evento di test per i casi d'uso generici
        /// </summary>
        public class TestEvent : IEvent { }

        /// <summary>
        /// Verifica che venga lanciata un'eccezione se l'evento è nullo
        /// </summary>
        [Fact(DisplayName = "PublishAsync_ConEventoNullo_DovrebbeLanciareArgumentNullException")]
        public async Task PublishAsync_NullEvent_ShouldThrowArgumentNullException()
        {
            // Arrange
            var eventBus = new EventBus(Mock.Of<IServiceProvider>());
            TestEvent nullEvent = null!;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => eventBus.PublishAsync(nullEvent)
            );
        }

        /// <summary>
        /// Verifica che un'eccezione lanciata da un handler venga propagata correttamente
        /// </summary>
        [Fact(DisplayName = "PublishAsync_ConHandlerCheGeneraEccezione_DovrebbePropagareEccezione")]
        public async Task PublishAsync_HandlerThrowsException_ShouldPropagateException()
        {
            // Arrange
            var eventBus = new EventBus(Mock.Of<IServiceProvider>());
            var expectedException = new InvalidOperationException("Simulated error");
            var mockHandler = new Mock<IEventHandler<TestEvent>>();

            mockHandler.Setup(h => h.HandleAsync(It.IsAny<TestEvent>()))
                       .ThrowsAsync(expectedException);

            eventBus.Subscribe(mockHandler.Object);
            var testEvent = new TestEvent();

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(
                () => eventBus.PublishAsync(testEvent)
            );

            Assert.Same(expectedException, actualException);
            mockHandler.Verify(h => h.HandleAsync(testEvent), Times.Once);
        }

        /// <summary>
        /// Verifica che gli handler successivi non vengano eseguiti dopo un'eccezione
        /// </summary>
        [Fact(DisplayName = "PublishAsync_ConHandlerCheGeneraEccezione_DovrebbeInterrompereEsecuzione")]
        public async Task PublishAsync_HandlerThrowsException_ShouldStopExecution()
        {
            // Arrange
            var eventBus = new EventBus(Mock.Of<IServiceProvider>());
            var mockHandler1 = new Mock<IEventHandler<TestEvent>>();
            var mockHandler2 = new Mock<IEventHandler<TestEvent>>();

            mockHandler1.Setup(h => h.HandleAsync(It.IsAny<TestEvent>()))
                        .ThrowsAsync(new Exception());

            eventBus.Subscribe(mockHandler1.Object);
            eventBus.Subscribe(mockHandler2.Object);
            var testEvent = new TestEvent();

            // Act
            await Assert.ThrowsAsync<Exception>(() => eventBus.PublishAsync(testEvent));

            // Assert
            mockHandler2.Verify(h => h.HandleAsync(It.IsAny<TestEvent>()), Times.Never);
        }

        /// <summary>
        /// Verifica il comportamento con handler misti (con e senza errori)
        /// </summary>
        [Fact(DisplayName = "PublishAsync_ConHandlerMisti_DovrebbeGestireEccezioniSelettivamente")]
        public async Task PublishAsync_MixedHandlers_ShouldHandlePartialErrors()
        {
            // Arrange
            var eventBus = new EventBus(Mock.Of<IServiceProvider>());
            var mockWorkingHandler = new Mock<IEventHandler<TestEvent>>();
            var mockFailingHandler = new Mock<IEventHandler<TestEvent>>();

            mockFailingHandler.Setup(h => h.HandleAsync(It.IsAny<TestEvent>()))
                              .ThrowsAsync(new Exception());

            eventBus.Subscribe(mockWorkingHandler.Object); // Handler senza errori
            eventBus.Subscribe(mockFailingHandler.Object); // Handler con errore
            var testEvent = new TestEvent();

            // Act
            var exception = await Record.ExceptionAsync(() => eventBus.PublishAsync(testEvent));

            // Assert
            Assert.NotNull(exception); // Verifica che l'eccezione sia stata propagata
            mockWorkingHandler.Verify(h => h.HandleAsync(testEvent), Times.Once); // Eseguito
            mockFailingHandler.Verify(h => h.HandleAsync(testEvent), Times.Once); // Eseguito ma fallito
        }
    }
}