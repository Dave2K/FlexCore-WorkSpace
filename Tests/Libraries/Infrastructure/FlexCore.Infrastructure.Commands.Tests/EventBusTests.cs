namespace FlexCore.Infrastructure.Commands.Tests;

using Xunit;
using Moq;
using FlexCore.Infrastructure.Events;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Test per l'EventBus.
/// </summary>
public class EventBusTests
{
    private readonly EventBus _eventBus;
    private readonly Mock<IServiceProvider> _serviceProviderMock;

    /// <summary>
    /// Inizializza una nuova istanza di <see cref="EventBusTests"/>.
    /// </summary>
    public EventBusTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _eventBus = new EventBus(_serviceProviderMock.Object);
    }

    /// <summary>
    /// Evento di test.
    /// </summary>
    public class TestEvent : IEvent { }

    /// <summary>
    /// Verifica che PublishAsync notifichi l'handler corretto.
    /// </summary>
    [Fact]
    public async Task PublishAsync_ShouldNotifyHandler()
    {
        var mockHandler = new Mock<IEventHandler<TestEvent>>();
        _eventBus.Subscribe(mockHandler.Object);
        var testEvent = new TestEvent();

        await _eventBus.PublishAsync(testEvent);

        mockHandler.Verify(h => h.HandleAsync(testEvent), Times.Once);
    }
}
