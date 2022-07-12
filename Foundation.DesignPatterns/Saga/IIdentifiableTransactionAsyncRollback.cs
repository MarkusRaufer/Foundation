namespace Foundation.DesignPatterns.Saga;

public interface IIdentifiableTransactionAsyncRollback<TId, TTransactionState, TTransactionResponse, TRollbackResponse>
    : ITransactionAsyncRollback<TTransactionState, TTransactionResponse, TRollbackResponse>
    , IIdentifiable<TId>
    where TId : notnull
{
}
