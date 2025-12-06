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
using Foundation.Collections.Generic;
using System.Collections.ObjectModel;

namespace Foundation.ComponentModel;


public interface IExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary, TBuilder>
    where TDictionary : IReadOnlyDictionary<TKey, TValue>
    where TBuilder : IExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary,TBuilder>
{
    TBuilder And(TKey key, TValue newValue);
    TBuilder And(IEnumerable<KeyValuePair<TKey, TValue>> keyValues);
    TBuilder AndRemove(TKey key);
    TBuilder AndRemove(IEnumerable<TKey> keys);
    IReadOnlyDictionary<TKey, TValue> Build(Action<IDictionary<TKey, EventActionValue<TValue>>> trackedChanges);
}

public class ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary> : IExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary, ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary>>
    where TKey : notnull
    where TDictionary : IReadOnlyDictionary<TKey, TValue>
{
    private Func<IEnumerable<KeyValuePair<TKey, TValue>>, TDictionary> _factory;
    private Dictionary<TKey, EventActionValue<TValue>> _properties = [];
    private readonly IReadOnlyDictionary<TKey, TValue> _source;

    public ExistingReadOnlyDictionaryBuilder(
        IReadOnlyDictionary<TKey, TValue> source,
        TKey key,
        Func<IEnumerable<KeyValuePair<TKey, TValue>>, TDictionary> factory)
    {
        _source = source.ThrowIfNull();
        _factory = factory.ThrowIfNull();

        RemoveKey(_source, key);
    }

    public ExistingReadOnlyDictionaryBuilder(
        IReadOnlyDictionary<TKey, TValue> source,
        TKey key,
        TValue newValue,
        Func<IEnumerable<KeyValuePair<TKey, TValue>>, TDictionary> factory)
    {
        _source = source.ThrowIfNull();
        _factory = factory.ThrowIfNull();

        AddKeyValue(_source, key.ThrowIfNull(), newValue);
    }

    public ExistingReadOnlyDictionaryBuilder(
        IReadOnlyDictionary<TKey, TValue> source,
        IEnumerable<TKey> keys,
        Func<IEnumerable<KeyValuePair<TKey, TValue>>, TDictionary> factory)
    {
        _source = source.ThrowIfNull();
        _factory = factory.ThrowIfNull();

        foreach (var key in keys)
            RemoveKey(_source, key);
    }

    public ExistingReadOnlyDictionaryBuilder(
        IReadOnlyDictionary<TKey, TValue> source,
        IEnumerable<KeyValuePair<TKey, TValue>> keyValues,
        Func<IEnumerable<KeyValuePair<TKey, TValue>>, TDictionary> factory)
    {
        _source = source.ThrowIfNull();
        _factory = factory.ThrowIfNull();

        foreach (var kvp in keyValues)
            AddKeyValue(_source, kvp.Key, kvp.Value);
    }

    private void AddKeyValue(IReadOnlyDictionary<TKey, TValue> source, TKey key, TValue newValue)
    {
        if (source.TryGetValue(key, out var value))
        {
            if (value.EqualsNullable(newValue)) return;
            _properties.Add(key, new EventActionValue<TValue>(EventAction.Update, newValue));
            return;
        }
        _properties.Add(key, new EventActionValue<TValue>(EventAction.Add, newValue));
    }

    public ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary> And(TKey key, TValue newValue)
    {
        AddKeyValue(_source, key.ThrowIfNull(), newValue);
        return this;
    }

    public ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary> And(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        foreach (var kvp in keyValues)
            AddKeyValue(_source, kvp.Key, kvp.Value);

        return this;
    }

    public ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary> AndRemove(TKey key)
    {
        RemoveKey(_source, key);
        return this;
    }

    public ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary> AndRemove(IEnumerable<TKey> keys)
    {
        foreach (var key in keys)
            RemoveKey(_source, key);

        return this;
    }

    public IReadOnlyDictionary<TKey, TValue> Build(Action<IDictionary<TKey, EventActionValue<TValue>>> trackedChanges)
    {
        return CreateNewObject(trackedChanges);
    }

    private IReadOnlyDictionary<TKey, TValue> CreateNewObject(
        Action<IDictionary<TKey, EventActionValue<TValue>>> trackedChanges)
    {
        var newInstance = new Dictionary<TKey, TValue>();

        Dictionary<TKey, EventActionValue<TValue>> changes = [];

        foreach (var (key, actionValue) in _properties)
        {
            switch (actionValue.Action)
            {
                case EventAction.Add:
                    newInstance.Add(key, actionValue.Value);
                    break;
                case EventAction.Update:
                    newInstance[key] = actionValue.Value;
                    break;
            }
            changes.Add(key, actionValue);
        }

        foreach (var kvp in _source)
        {
            if (_properties.TryGetValue(kvp.Key, out var actionValue)
                && actionValue.Action == EventAction.Remove) continue;

            if (newInstance.ContainsKey(kvp.Key)) continue;

            newInstance.Add(kvp.Key, kvp.Value);
        }

        if (changes.Count > 0) trackedChanges(changes);

        return _factory(newInstance);
    }

    private void RemoveKey(IReadOnlyDictionary<TKey, TValue> source, TKey key)
    {
        if (key is null) return;

        if (!source.TryGetValue(key, out var value)) return;

        _properties.Add(key, new EventActionValue<TValue>(EventAction.Remove, value));
    }
}
