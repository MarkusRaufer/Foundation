namespace Foundation.ComponentModel;

public class TransactionStarted : TransactionStarted<Guid>
{
    public TransactionStarted() : this(Guid.NewGuid())
    {
    }

    public TransactionStarted(Guid transactionId) : base(transactionId)
    {
    }

    public TransactionStarted(Guid transactionId, DateTime startedOn) : base(transactionId, startedOn)
    {
    }
}

public class TransactionStarted<TTransactionId> 
    : TransactionEvent<TTransactionId>
    , IStartedEvent<TTransactionId>
    where TTransactionId : notnull
{
    public TransactionStarted(TTransactionId transactionId) : this(transactionId, DateTime.UtcNow)
    {
    }

    public TransactionStarted(TTransactionId transactionId, DateTime startedOn) : base(transactionId)
    {
        StartedOn = startedOn;
    }

    public DateTime StartedOn { get; }
}
