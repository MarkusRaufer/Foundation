using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Collections.ObjectModel
{
    public static class EnumerableExtensions
    {
        public static IPropertyCollection ToPropertyCollection(
            this IEnumerable<Property> properties)
        {
            return new PropertyCollection(properties);
        }

        public static IPropertyCollection ToPropertyCollection(this IEnumerable<KeyValuePair<string, object>> properties)
        {
            return new PropertyCollection(properties.Select(kvp => new Property(kvp.Key, kvp.Value)));
        }
        
        public static IPropertyCollection ToPropertyCollection<T, TKey, TValue>(
            this IEnumerable<T> items, Func<T, string> keySelector, Func<T, TValue> valueSelector)
        {
            var properties = new PropertyCollection();
            foreach (var item in items)
            {
                var property = new Property(keySelector(item), valueSelector(item));
                properties.Add(property);
            }
                
            return properties;
        }
    }
}
