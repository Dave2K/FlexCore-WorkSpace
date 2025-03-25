namespace FlexCore.Infrastructure.Events
{
    /// <summary>
    /// Interfaccia per il bus eventi
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Pubblica un evento in modo asincrono
        /// </summary>
        /// <typeparam name="TEvent">Tipo dell'evento</typeparam>
        /// <param name="event">Istanza dell'evento</param>
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}