namespace Foundation;

public abstract class IndexedIdentifiable<TKey, TValue>
    : IIndexedIdentifiable<TKey, TValue>
    , IEquatable<IIndexedIdentifiable<TKey, TValue>>
    where TKey : notnull
{
    protected IndexedIdentifiable(KeyValue<TKey, TValue> identifier)
    {
        Identifier = identifier.ThrowIfEmpty(nameof(identifier));
    }
    public KeyValue<TKey, TValue> Identifier { get; }

    public override bool Equals(object? obj) => obj is IIndexedIdentifiable<TKey, TValue> other && Equals(other);

    public bool Equals(IIndexedIdentifiable<TKey, TValue>? other)
    {
        return null != other && EqualityComparer<KeyValue<TKey, TValue>>.Default.Equals(Identifier, other.Identifier);
    }

    public override int GetHashCode() => Identifier.GetHashCode();

    public override string ToString() => $"{Identifier}";
}

