namespace Foundation.ComponentModel;

public abstract class GenericEntity<TId> : Entity<NamedId<TId>>
    where TId : struct, IComparable<TId>, IEquatable<TId>
{
    protected GenericEntity(NamedId<TId> id) : base(id)
    {
    }
}
