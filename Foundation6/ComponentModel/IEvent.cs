namespace Foundation.ComponentModel;

public interface IEvent<TEventId>
{
    TEventId EventId { get; }
}
