namespace Foundation.ComponentModel;

public interface IEvent<TEventId>
{
    TEventId EventId { get; }
}

public interface IEvent<TEventId, TCommandId> : IEvent<TEventId>
{
    TCommandId CommandId { get; }
}
