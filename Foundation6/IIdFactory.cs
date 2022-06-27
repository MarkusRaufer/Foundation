namespace Foundation;

public interface IIdFactory<TId>
{
    TId CreateId();
}
