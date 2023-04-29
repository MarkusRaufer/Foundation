namespace Foundation.ComponentModel;

public record TransactionEnded(Guid EventId, DateTime EndedOn) : TransactionEnded<Guid>(EventId, EndedOn);

public record TransactionEnded<TTransactionId>(TTransactionId EventId, DateTime EndedOn)
    : TransactionEvent<TTransactionId>(EventId)
    , IEndedEvent<TTransactionId>
    where TTransactionId : notnull;
