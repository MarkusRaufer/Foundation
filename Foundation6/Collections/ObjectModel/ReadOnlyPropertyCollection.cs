using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.ObjectModel
{
    public class ReadOnlyPropertyCollection 
        : IReadOnlyPropertyCollection
        , IEquatable<ReadOnlyPropertyCollection>
    {
        private readonly PropertyCollection _properties;

        public ReadOnlyPropertyCollection(IEnumerable<Property> properties)
        {
            _properties = new PropertyCollection(properties);
        }

        public ReadOnlyPropertyCollection(PropertyCollection properties)
        {
            _properties = properties.ThrowIfNull();
        }
        
        public bool ContainsProperty(string name) => _properties.ContainsProperty(name);

        public int Count => _properties.Count;

        public override bool Equals(object? obj) => obj is PropertyCollection other && Equals(other);

        public bool Equals(PropertyCollection? other)
        {
            return null != other && _properties.Equals(other);
        }

        public bool Equals(ReadOnlyPropertyCollection? other)
        {
            return null != other && Equals(other._properties);
        }

        public IEnumerator<Property> GetEnumerator() => _properties.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => _properties.GetEnumerator();

        public override int GetHashCode() => _properties.GetHashCode();

        public bool TryGetProperty(string name, [MaybeNullWhen(false)] out Property property)
        {
            return _properties.TryGetProperty(name, out property);
        }
    }
}
