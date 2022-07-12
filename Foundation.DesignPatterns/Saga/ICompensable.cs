namespace Foundation.DesignPatterns.Saga;

public interface ICompensable<TTransactionState, TCompensationResponse>
{
    TCompensationResponse Compensate(TTransactionState state);
}
