namespace Foundation.DesignPatterns.Saga;

public class AsyncRollbackProvider<TId, TTransactionState, TTransactionResponse, TRollbackResponse> 
    : RollbackProvider<TId, IIdentifiableTransactionAsyncRollback<TId, TTransactionState, TTransactionResponse, TRollbackResponse>>
    , IAsyncRollbackProvider<TId, TTransactionState, TTransactionResponse, TRollbackResponse>
    where TId : notnull
{
    public AsyncRollbackProvider(
        IEnumerable<IIdentifiableTransactionAsyncRollback<TId, TTransactionState, TTransactionResponse, TRollbackResponse>> strategies)
        : base(strategies)
    {
    }
}
