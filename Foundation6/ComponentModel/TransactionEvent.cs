namespace Foundation.ComponentModel;
public abstract class TransactionEvent<TTransactionId> 
    : EventBase<TTransactionId>
    , ITransactionEvent<TTransactionId>
    where TTransactionId : notnull
{
    protected TransactionEvent(TTransactionId transactionId) : base(transactionId)
    {
    }
}
