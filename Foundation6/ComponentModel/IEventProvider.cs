namespace Foundation.ComponentModel;

public interface IEventProvider<TEvent> : IEventProvider<TEvent, IEnumerable<TEvent>>
{
}

public interface IEventProvider<TEvent, TEventCollection>
{
    void ClearEvents();
    TEventCollection Events { get; }
    bool HasEvents { get; }
}
