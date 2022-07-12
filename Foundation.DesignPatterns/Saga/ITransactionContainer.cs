namespace Foundation.DesignPatterns.Saga;

public interface ITransactionContainer<TTransaction>
{
    void SetTransaction(TTransaction transaction);
}
