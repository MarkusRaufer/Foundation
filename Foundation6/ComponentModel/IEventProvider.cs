namespace Foundation.ComponentModel;

public interface IEventProvider<TEvent>
{
    void ClearEvents();
    IEnumerable<TEvent> Events { get; }
    bool HasEvents { get; }
}

