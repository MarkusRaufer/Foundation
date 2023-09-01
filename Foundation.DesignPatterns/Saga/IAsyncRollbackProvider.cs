namespace Foundation.DesignPatterns.Saga;

public interface IAsyncRollbackProvider<TId, TTransactionState, TTransactionResponse, TRollbackResponse>
    : IRollbackProvider<TId, IIdentifiableTransactionAsyncRollback<TId, TTransactionState, TTransactionResponse, TRollbackResponse>>
{
}
