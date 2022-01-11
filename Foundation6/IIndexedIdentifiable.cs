namespace Foundation;

public interface IIndexedIdentifiable<TKey, TValue>
    where TKey : notnull
{
    KeyValue<TKey, TValue> Identifier { get; }
}

