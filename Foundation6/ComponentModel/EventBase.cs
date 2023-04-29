namespace Foundation.ComponentModel;

public abstract record EventBase<TEventId>(TEventId EventId) : IEvent<TEventId> where TEventId : notnull;

public abstract record EventBase<TEventId, TCommandId>(TEventId EventId, TCommandId CommandId) : IEvent<TEventId>
    where TEventId : notnull
    where TCommandId : notnull;
