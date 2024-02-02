// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
﻿using Foundation.ComponentModel;
using System.Collections;
using System.Collections.Specialized;

namespace Foundation.Collections.ObjectModel
{
    public class ObservableCollectionDecorator<T>
        : ICollection<T>
        , IMutable
        , INotifyCollectionChanged
    {
        #region events
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        #endregion events

        private readonly ICollection<T> _collection;

        public ObservableCollectionDecorator(ICollection<T> collection, bool collectionChangedEventEnabled = true)
        {
            _collection = collection.ThrowIfNull();
            CollectionChangedEventEnabled = collectionChangedEventEnabled;
        }

        public void Add(T item)
        {
            _collection.Add(item);

            IsDirty = true;
            if (CollectionChangedEventEnabled && CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Clear()
        {
            if (0 == _collection.Count) return;

            _collection.Clear();
            IsDirty = true;
            if (CollectionChangedEventEnabled && null != CollectionChanged)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool CollectionChangedEventEnabled { get; set; }

        public bool Contains(T item) => _collection.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

        public int Count => _collection.Count;

        public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsDirty { get; set; }

        public bool IsReadOnly => _collection.IsReadOnly;

        public bool Remove(T item)
        {
            if (!_collection.Remove(item))
                return false;

            IsDirty = true;
            if (CollectionChangedEventEnabled && null != CollectionChanged)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));

            return true;
        }
    }
}
