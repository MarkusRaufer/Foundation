namespace Foundation.ComponentModel;

public record TransactionStarted(Guid TransactionId, Guid EventId, DateTime StartedOn)
    : TransactionStarted<Guid, Guid>(TransactionId, EventId, StartedOn);

public record TransactionStarted<TTransactionId, TEventId>(TTransactionId TransactionId, TEventId EventId, DateTime StartedOn) 
    : TransactionEvent<TTransactionId, TEventId>(TransactionId, EventId)
    , IStartedEvent<TEventId>
    where TTransactionId : notnull
    where TEventId : notnull;
