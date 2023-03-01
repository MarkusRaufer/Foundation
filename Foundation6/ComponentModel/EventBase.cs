namespace Foundation.ComponentModel;

public class EventBase<TEventId> 
    : IEvent<TEventId>
    , IEquatable<IEvent<TEventId>>
    where TEventId : notnull
{
    public EventBase(TEventId eventId)
    {
        EventId = eventId;
    }

    public bool Equals(IEvent<TEventId>? other)
    {
        return null != other && EventId.Equals(other.EventId);
    }

    public TEventId EventId { get; }

    public override bool Equals(object? obj) => Equals(obj as IEvent<TEventId>);

    public override int GetHashCode() => EventId.GetHashCode();

    public override string ToString() => $"{nameof(EventId)}: {EventId}";
}
