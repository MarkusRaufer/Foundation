namespace Foundation.DesignPatterns.Saga;

public interface ICompensableProvider<TId, TStrategy>
{
    TStrategy GetCompensationStrategy(TId strategyId);
}

public interface ICompensableProvider<TId, TTransactionState, TTransactionResponse, TCompensationResponse>
    : ICompensableProvider<TId, IIdentifiableTransactionCompensation<TId, TTransactionState, TTransactionResponse, TCompensationResponse>>
{
}
