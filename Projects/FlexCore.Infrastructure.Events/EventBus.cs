namespace FlexCore.Infrastructure.Events;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Implementazione dell'EventBus per la gestione e distribuzione degli eventi.
/// </summary>
/// <param name="serviceProvider">Service provider per la risoluzione delle dipendenze.</param>
public class EventBus(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    private readonly List<object> _subscribers = [];

    /// <summary>
    /// Sottoscrive un handler per un evento specifico.
    /// </summary>
    /// <typeparam name="TEvent">Tipo dell'evento.</typeparam>
    /// <param name="handler">Handler da registrare.</param>
    /// <exception cref="ArgumentNullException">Se l'handler è nullo.</exception>
    public void Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));
        _subscribers.Add(handler);
    }

    /// <summary>
    /// Pubblica un evento e notifica tutti gli handler registrati.
    /// </summary>
    /// <typeparam name="TEvent">Tipo dell'evento pubblicato.</typeparam>
    /// <param name="event">Istanza dell'evento.</param>
    /// <returns>Task completato dopo l'elaborazione dell'evento.</returns>
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
    {
        foreach (var subscriber in _subscribers)
        {
            if (subscriber is IEventHandler<TEvent> handler)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}
