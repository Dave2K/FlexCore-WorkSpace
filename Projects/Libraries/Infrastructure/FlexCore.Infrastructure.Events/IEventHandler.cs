namespace FlexCore.Infrastructure.Events;

/// <summary>
/// Interfaccia per la gestione degli eventi di un tipo specifico
/// </summary>
/// <typeparam name="TEvent">Tipo dell'evento da gestire</typeparam>
public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    /// <summary>
    /// Gestisce l'evento in modo asincrono
    /// </summary>
    /// <param name="event">Istanza dell'evento da gestire</param>
    /// <returns>Task che rappresenta l'operazione asincrona</returns>
    Task HandleAsync(TEvent @event);
}