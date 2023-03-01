namespace Foundation.ComponentModel;

public abstract class Entity<TId>
    : IIdentifiable<TId>
    , IEquatable<IIdentifiable<TId>>
    where TId : notnull
{
    protected Entity(TId id) => Id = id.ThrowIfNull();

    public override bool Equals(object? obj) => Equals(obj as IIdentifiable<TId>);

    public bool Equals(IIdentifiable<TId>? other) => null != other && Id.Equals(other.Id);

    public override int GetHashCode() => Id.GetHashCode();

    public TId Id { get; }

    public override string ToString() => $"Id: {Id}";
}
