using System;
using System.Collections.Generic;
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
        bool Remove(string name);
    }
}