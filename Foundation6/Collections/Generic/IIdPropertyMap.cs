namespace Foundation.Collections.Generic
{
    /// <summary>
    /// Identifiable IPropertyMap.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TEvent">The type of the changed event.</typeparam>
    public interface IIdPropertyMap<TId, TEvent>
        : IProperties<TEvent>
        , IIdentifiable<TId>
    {
    }

    /// <summary>
    /// Identifiable and typed IPropertyMap.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObjectType">The object type.</typeparam>
    /// <typeparam name="TEvent">The type of the changed event.</typeparam>
    public interface IIdPropertyMap<TObjectType, TId, TEvent>
        : IIdPropertyMap<TId, TEvent>
        , IProperties<TObjectType, TEvent>
    {
    }
}
