namespace Foundation.ComponentModel;

/// <summary>
/// This is an entity which is able to select a property of its own to be the identifier.
/// </summary>
/// <typeparam name="TId"></typeparam>
public class FlexEntity<TId> : IEquatable<FlexEntity<TId>>
    where TId : notnull
{
    private readonly Func<TId> _idSelector;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idSelector">This is the selector to select a property as identifier.</param>
    public FlexEntity(Func<TId> idSelector)
    {
        _idSelector = idSelector.ThrowIfNull();
    }

    public override bool Equals(object? obj) => Equals(obj as FlexEntity<TId>);
    
    /// <summary>
    /// Equals compares the selected identifiers by _idSelector.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(FlexEntity<TId>? other)
    {
        return null != other
            && _idSelector().Equals(other._idSelector());
    }

    public override int GetHashCode() => _idSelector().GetHashCode();

    public override string ToString() => $"{_idSelector()}";
}
