namespace Foundation.ComponentModel;

/// <summary>
/// This is an abstract entity type and can be used to quickly create a derived entity type
/// where only the Id is considered for equality.
/// </summary>
/// <typeparam name="TId">Type of the Id.</typeparam>
public abstract class Entity<TId> 
    : IEntity<TId>
    , IEquatable<IEntity<TId>>
{
    /// <summary>
    /// The creation of an entity needs an identifier <paramref name="id"/>.
    /// </summary>
    /// <param name="id"></param>
    protected Entity(TId id)
    {
        Id = id.ThrowIfNull();
    }

    /// <summary>
    /// The identifier of the entity.
    /// </summary>
    public TId Id { get; }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as IEntity<TId>);

    /// <summary>
    /// This method only compares the Id property.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(IEntity<TId>? other)
    {
        return other is not null && Id.EqualsNullable(other.Id);
    }

    /// <summary>
    /// This method returns the hash code of Id.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Id.GetNullableHashCode();

    /// <summary>
    /// Prints the Id of the entity.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{nameof(Id)}: {Id}";
}
