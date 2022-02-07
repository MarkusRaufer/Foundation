using System.Collections.Specialized;
using System.ComponentModel;

namespace Foundation.Collections.ObjectModel
{
    public interface IPropertyCollection
        : ICollection<Property>
        , IReadOnlyPropertyCollection
        , INotifyCollectionChanged
        , INotifyPropertyChanged
        , IDisposable
    {
        object? this[string propertyName] { get; set; }
        bool Remove(string name);
    }
}