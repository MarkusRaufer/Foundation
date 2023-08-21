using Foundation.ComponentModel;

namespace Foundation.Collections.Generic
{
    /// <summary>
    /// A map with properties (string, object)
    /// </summary>
    public interface IProperties<TEvent>
        : IDictionary<string, object>
        , IEventHandler<TEvent>
        , IEventProvider<TEvent>
    {
    }

    public interface IProperties<TObjectType, TEvent> 
        : IProperties<TEvent>
        , ITypedObject<TObjectType>
    {
    }
}
