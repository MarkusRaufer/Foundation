using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.ObjectModel
{
    public interface IReadOnlyPropertyCollection : IReadOnlyPropertyCollection<Property>
    {
    }

    public interface IReadOnlyPropertyCollection<TProperty> : IReadOnlyCollection<TProperty>
    {
        bool ContainsProperty(string name);
        bool TryGetProperty(string name, [MaybeNullWhen(false)] out TProperty? property);
    }
}