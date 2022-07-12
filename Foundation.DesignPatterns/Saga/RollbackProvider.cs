namespace Foundation.DesignPatterns.Saga;

public class RollbackProvider<TId, TTransactionState, TTransactionResponse, TRollbackResponse> 
    : RollbackProvider<TId, IIdentifiableTransactionRollback<TId, TTransactionState, TTransactionResponse, TRollbackResponse>>
    , IRollbackProvider<TId, TTransactionState, TTransactionResponse, TRollbackResponse>
    where TId : notnull
{
    public RollbackProvider(
        IEnumerable<IIdentifiableTransactionRollback<TId, TTransactionState, TTransactionResponse, TRollbackResponse>> strategies)
        : base(strategies)
    {
    }
}

public class RollbackProvider<TId, TStrategy> : IRollbackProvider<TId, TStrategy>
    where TId : notnull
    where TStrategy: IIdentifiable<TId>
{
    private readonly ICollection<TStrategy> _strategies;

    public RollbackProvider(IEnumerable<TStrategy> strategies)
    {
        _strategies = strategies.ThrowIfNull(nameof(strategies)).ToArray();
    }

    public TStrategy GetRollbackStrategy(TId strategyId)
    {
        return _strategies.First(s => s.Id.Equals(strategyId));
    }
}
