namespace Foundation.ComponentModel;

public class TransactionEnded : TransactionEnded<Guid>
{
    public TransactionEnded() : this(Guid.NewGuid())
    {
    }

    public TransactionEnded(Guid transactionId) : base(transactionId)
    {
    }

    public TransactionEnded(Guid transactionId, DateTime endedOn) : base(transactionId, endedOn)
    {
    }
}

public class TransactionEnded<TTransactionId> 
    : TransactionEvent<TTransactionId>
    , IEndedEvent<TTransactionId>
    where TTransactionId : notnull
{
    public TransactionEnded(TTransactionId transactionId) : this(transactionId, DateTime.UtcNow)
    {
    }

    public TransactionEnded(TTransactionId transactionId, DateTime endedOn) : base(transactionId)
    {
        EndedOn = endedOn;
    }

    public DateTime EndedOn { get; }
}
