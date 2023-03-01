namespace Foundation;

public abstract class GenericEntity<TIdKey, TIdValue> 
    : IIndexedIdentifiable<TIdKey, TIdValue>
    where TIdKey : notnull
    where TIdValue : notnull
{
    protected GenericEntity(KeyValuePair<TIdKey, TIdValue> identifier) => Identifier = identifier.ThrowIfNull();

    public override bool Equals(object? obj) => Equals(obj as IIndexedIdentifiable<TIdKey, TIdValue>);

    public bool Equals(IIndexedIdentifiable<TIdKey, TIdValue>? other) => null != other && Identifier.Equals(other.Identifier);

    public override int GetHashCode() => System.HashCode.Combine(Identifier.Key, Identifier.Value);

    public KeyValuePair<TIdKey, TIdValue> Identifier { get; }

    public override string ToString() => $"Identifier: ({Identifier.Key}, {Identifier.Value}";
}
