namespace FlexCore.Infrastructure.Events;
using System;

/// <summary>
/// Classe astratta che rappresenta un evento di dominio.
/// </summary>
public abstract class DomainEvent : IEvent
{
    public DateTime OccurredOn { get; }
    protected DomainEvent() => OccurredOn = DateTime.UtcNow;
}
