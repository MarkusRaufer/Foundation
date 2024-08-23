using Foundation.Collections.Generic;

namespace Foundation.ComponentModel;

/// <summary>
/// Provides an events list.
/// </summary>
/// <typeparam name="TEvent">Type of event.</typeparam>
public class EventHistory<TEvent> : IEventHistory<TEvent>
{
    private readonly List<TEvent> _events = [];

    /// <summary>
    /// Add an event tp the events list.
    /// </summary>
    /// <param name="event">The event that should be added.</param>
    public void AddEvent(TEvent @event) => _events.Add(@event);

    /// <summary>
    /// Removes all events from the events list.
    /// </summary>
    /// <param name="event"></param>
    public void ClearEvents() => _events.Clear();

    /// <summary>
    /// List of events.
    /// </summary>
    public IEnumerable<TEvent> Events => _events;

    /// <summary>
    /// True if an event exists.
    /// </summary>
    public bool HasEvents => _events.Count > 0;

    /// <summary>
    /// Removes an event at a specific index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool RemoveEventAt(int index) => _events.InRange(index) && Void.Returns(true, () => _events.RemoveAt(index));

    /// <summary>
    /// Removes a number of events at a specific index.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public void RemoveEvents(int index, int count)
        => _events.InRange<TEvent, List<TEvent>>(index, count, (List<TEvent> x) => x.RemoveRange(index, count));
}
