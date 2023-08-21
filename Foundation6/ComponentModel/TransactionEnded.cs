namespace Foundation.ComponentModel;

public record TransactionEnded(Guid TransactionId, Guid EventId, DateTime EndedOn)
    : TransactionEnded<Guid, Guid>(TransactionId, EventId, EndedOn);

public record TransactionEnded<TTransactionId, TEvent>(TTransactionId TransactionId, TEvent EventId, DateTime EndedOn)
    : TransactionEvent<TTransactionId, TEvent>(TransactionId, EventId)
    , IEndedEvent<TEvent>
    where TTransactionId : notnull
    where TEvent : notnull;
