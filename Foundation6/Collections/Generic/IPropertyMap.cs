using Foundation.ComponentModel;
using System.Collections.Generic;

namespace Foundation.Collections.Generic
{
    /// <summary>
    /// A map with properties (string, object) that can handle hierarchical properties.
    /// </summary>
    public interface IPropertyMap : IDictionary<string, object>
    {
        char PathSeparator { get; }

        IEnumerable<KeyValuePair<string, object>> PropertiesStartWith(string name);

        IEnumerable<KeyValuePair<string, object>> RootProperties { get; }

    }

    public interface IPropertyMap<TObjectType> 
        : IPropertyMap
        , ITypedObject<TObjectType>
    {
    }
}