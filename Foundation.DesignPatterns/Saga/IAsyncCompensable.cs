namespace Foundation.DesignPatterns.Saga;

public interface IAsyncCompensable<TTransactionState, TCompensationResponse>
{
    Task<TCompensationResponse> AsyncCompensate(TTransactionState state);
}
