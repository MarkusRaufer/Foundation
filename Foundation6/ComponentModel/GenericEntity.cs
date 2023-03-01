namespace Foundation.ComponentModel;

public abstract class GenericEntity<TId> : IIdentifiable<NamedId<TId>>
    where TId : struct, IComparable<TId>, IEquatable<TId>
{
    protected GenericEntity(NamedId<TId> id) => Id = id;

    public override bool Equals(object? obj) => Equals(obj as IIdentifiable<NamedId<TId>>);

    public bool Equals(IIdentifiable<NamedId<TId>>? other) => null != other && Id.Equals(other.Id);

    public override int GetHashCode() => Id.GetHashCode();

    public NamedId<TId> Id { get; }

    public override string ToString() => $"Id: {Id}";
}
