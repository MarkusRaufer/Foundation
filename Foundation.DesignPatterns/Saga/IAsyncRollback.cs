namespace Foundation.DesignPatterns.Saga;

public interface IAsyncRollback<TTransactionState, TRollbackResponse>
{
    Task<TRollbackResponse> AsyncRollback(TTransactionState state);
}
