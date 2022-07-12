namespace Foundation.DesignPatterns.Saga;

public interface IIdentifiableTransactionRollback<TId, TTransactionState, TTransactionResponse, TRollbackResponse>
    : ITransactionRollback<TTransactionState, TTransactionResponse, TRollbackResponse>
    , IIdentifiable<TId>
    where TId : notnull
{
}
