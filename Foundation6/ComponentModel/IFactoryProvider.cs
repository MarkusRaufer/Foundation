namespace Foundation.ComponentModel;

public interface IFactoryProvider<TSelector, TFactory>
{
    TFactory? GetFactory(TSelector selector);
}
