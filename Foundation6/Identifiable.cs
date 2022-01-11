namespace Foundation;

using System.Diagnostics.CodeAnalysis;

public abstract class Identifiable<TId> : IEquatable<IIdentifiable<TId>>
    where TId : notnull
{
    protected Identifiable(TId id) => Id = id.ThrowIfNull(nameof(id))!;

    public override bool Equals(object? obj) => obj is IIdentifiable<TId> other && Equals(other);

    public bool Equals(IIdentifiable<TId>? other)
    {
        return null != other && Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();

    [NotNull]
    public TId Id { get; }

    public override string ToString() => $"Id: {Id}";
}
