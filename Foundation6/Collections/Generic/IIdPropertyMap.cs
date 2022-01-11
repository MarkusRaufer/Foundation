using Foundation.ComponentModel;

namespace Foundation.Collections.Generic
{
    public interface IIdPropertyMap<TId>
        : IPropertyMap
        , IIdentifiable<TId>
        , IIndexedIdentifiable<string, TId>
        where TId : notnull
    {
    }

    /// <summary>
    /// Identifiable and typed IPropertyMap.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TObjectType"></typeparam>
    public interface IIdPropertyMap<TObjectType, TId>
        : IIdPropertyMap<TId>
        , IPropertyMap<TObjectType>
        , ITypedObject<TObjectType>
        where TId : notnull
    {
    }
}
