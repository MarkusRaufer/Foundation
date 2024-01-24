namespace Foundation.ComponentModel;

public interface ITransactionProvider<TTransaction>
{
    TTransaction BeginTransaction();
}
