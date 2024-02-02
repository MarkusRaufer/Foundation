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
﻿using Foundation.Collections.Generic;
using Foundation.ComponentModel;
using System.Collections;
using System.Collections.Specialized;

namespace Foundation.Collections.ObjectModel
{
    public class ObservableDictionaryDecorator<TKey, TValue>
        : IDictionary<TKey, TValue>
        , IMutable
        , INotifyCollectionChanged
        where TKey : notnull
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private readonly IDictionary<TKey, TValue> _dictionary;

        public ObservableDictionaryDecorator(IDictionary<TKey, TValue> dictionary, bool collectionChangedEnabled = true, bool isDirty = false)
        {
            _dictionary = dictionary.ThrowIfNull();
            CollectionChangedEnabled = collectionChangedEnabled;
            IsDirty = isDirty;
        }

        #region public properties

        public bool CollectionChangedEnabled { get; set; }

        public int Count => _dictionary.Count;

        public bool IsDirty { get; set; }

        public bool IsReadOnly => _dictionary.IsReadOnly;

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        public TValue this[TKey key]
        {
            get { return _dictionary[key]; }
            set
            {
                var exists = _dictionary.TryGetValue(key, out var oldValue);

                if (exists && oldValue.EqualsNullable(value)) return;

                _dictionary[key] = value;
                IsDirty = true;

                if (CollectionChangedEnabled)
                {
                    if(exists)
                        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldValue));
                    else
                        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
                }
            }
        }

        #endregion public properties

        #region public methods

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);

            IsDirty = true;
            if (CollectionChangedEnabled)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
        }

        public void Clear()
        {
            if (0 == _dictionary.Count) return;

            _dictionary.Clear();
            IsDirty = true;
            if (CollectionChangedEnabled)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Remove(TKey key)
        {
            if (!_dictionary.TryGetValue(key, out TValue? value))
                return false;

            IsDirty = true;
            if (CollectionChangedEnabled)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, Pair.New(key, value)));

            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!_dictionary.Remove(item)) return false;

            IsDirty = true;
            if (CollectionChangedEnabled)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));

            return true;
        }

        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value!);

        #endregion public methods
    }
}
