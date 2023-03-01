using Foundation.ComponentModel;

namespace Foundation.Collections.Generic
{
    /// <summary>
    /// Identifiable IPropertyMap.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TEvent">The type of the changed event.</typeparam>
    public interface IIdPropertyMap<TId, TEvent>
        : IPropertyMap<TEvent>
        , IIdentifiable<TId>
        where TId : notnull
    {
    }

    /// <summary>
    /// Identifiable and typed IPropertyMap.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObjectType">The object type.</typeparam>
    /// <typeparam name="TEvent">The type of the changed event.</typeparam>
    public interface IIdPropertyMap<TObjectType, TId, TEvent>
        : IPropertyMap<TObjectType, TEvent>
        where TId : notnull
    {
    }
}
