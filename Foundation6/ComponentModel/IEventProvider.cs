namespace Foundation.ComponentModel;

/// <summary>
/// Contract of an event provider.
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IEventProvider<TEvent> : IEventProvider<TEvent, IEnumerable<TEvent>>
{
}

/// <summary>
/// Contract of an event provider.
/// </summary>
/// <typeparam name="TEvent">Type of the events.</typeparam>
/// <typeparam name="TEventCollection">Type of the event collection.</typeparam>
public interface IEventProvider<TEvent, TEventCollection>
{
    /// <summary>
    /// Removes all events.
    /// </summary>
    void ClearEvents();

    /// <summary>
    /// List of events.
    /// </summary>
    TEventCollection Events { get; }

    /// <summary>
    /// True if <see cref="Events"/> contains at least one event.
    /// </summary>
    bool HasEvents { get; }
}
