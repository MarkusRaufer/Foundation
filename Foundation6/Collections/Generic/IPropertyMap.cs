using Foundation.ComponentModel;

namespace Foundation.Collections.Generic
{
    /// <summary>
    /// A map with properties (string, object)
    /// </summary>
    public interface IPropertyMap<TEvent>
        : IDictionary<string, object>
        , IEventHandler<TEvent>
        , IEventProvider<TEvent>
    {
    }

    public interface IPropertyMap<TObjectType, TEvent> 
        : IPropertyMap<TEvent>
        , ITypedObject<TObjectType>
    {
    }
}
