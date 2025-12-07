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
﻿// The MIT License (MIT)
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

namespace Foundation.ComponentModel;


public interface IExistingDictionaryBuilder<TKey, TValue, TBuilder>
    where TBuilder : IExistingDictionaryBuilder<TKey, TValue, TBuilder>
{
    TBuilder And(TKey key, TValue newValue);
    TBuilder And(IEnumerable<KeyValuePair<TKey, TValue>> keyValues);
    TBuilder AndRemove(TKey key);
    TBuilder AndRemove(IEnumerable<TKey> keys);
    IDictionary<TKey, TValue> Build();
    IDictionary<TKey, TValue> Build(Action<IDictionary<TKey, EventActionValue<TValue>>> trackedChanges);
}

public class ExistingDictionaryBuilder
{
    public enum BuildMode
    {
        ChangeWith,
        UpdateWith
    };
}

public struct ExistingDictionaryBuilder<TKey, TValue> : IExistingDictionaryBuilder<TKey, TValue, ExistingDictionaryBuilder<TKey, TValue>>
    where TKey : notnull
{
    private readonly ExistingDictionaryBuilder.BuildMode _buildMode;
    private Dictionary<TKey, EventActionValue<TValue>> _properties = [];
    private readonly IDictionary<TKey, TValue> _source;

    public ExistingDictionaryBuilder(
        ExistingDictionaryBuilder.BuildMode mode,
        IDictionary<TKey, TValue> source,
        TKey key)
    {
        _buildMode = mode;
        _source = source.ThrowIfNull();

        RemoveKey(_source, key);
    }

    public ExistingDictionaryBuilder(
        ExistingDictionaryBuilder.BuildMode mode,
        IDictionary<TKey, TValue> source,
        TKey key, TValue newValue)
    {
        _buildMode = mode;
        _source = source.ThrowIfNull();

        AddKeyValue(_source, key.ThrowIfNull(), newValue);
    }

    public ExistingDictionaryBuilder(
        ExistingDictionaryBuilder.BuildMode mode,
        IDictionary<TKey, TValue> source,
        IEnumerable<TKey> keys)
    {
        _buildMode = mode;
        _source = source.ThrowIfNull();

        foreach (var key in keys)
            RemoveKey(_source, key);
    }

    public ExistingDictionaryBuilder(
        ExistingDictionaryBuilder.BuildMode mode,
        IDictionary<TKey, TValue> source,
        IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        _buildMode = mode;
        _source = source.ThrowIfNull();

        foreach (var kvp in keyValues)
            AddKeyValue(_source, kvp.Key, kvp.Value);
    }

    private void AddKeyValue(IDictionary<TKey, TValue> source, TKey key, TValue newValue)
    {
        if (source.TryGetValue(key, out var value))
        {
            if (value.EqualsNullable(newValue)) return;
            _properties.Add(key, new EventActionValue<TValue>(EventAction.Update, newValue));
            return;
        }
        _properties.Add(key, new EventActionValue<TValue>(EventAction.Add, newValue));
    }

    public ExistingDictionaryBuilder<TKey, TValue> And(TKey key, TValue newValue)
    {
        AddKeyValue(_source, key.ThrowIfNull(), newValue);
        return this;
    }

    public ExistingDictionaryBuilder<TKey, TValue> And(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        foreach (var kvp in keyValues)
            AddKeyValue(_source, kvp.Key, kvp.Value);

        return this;
    }

    public ExistingDictionaryBuilder<TKey, TValue> AndRemove(TKey key)
    {
        RemoveKey(_source, key);
        return this;
    }

    public ExistingDictionaryBuilder<TKey, TValue> AndRemove(IEnumerable<TKey> keys)
    {
        foreach (var key in keys)
            RemoveKey(_source, key);

        return this;
    }

    public IDictionary<TKey, TValue> Build()
    {
        return _buildMode switch
        {
            ExistingDictionaryBuilder.BuildMode.ChangeWith => ChangeObject(null),
            ExistingDictionaryBuilder.BuildMode.UpdateWith => UpdateObject(null),
            _ => throw new NotImplementedException(_buildMode.ToString())
        };
    }

    public IDictionary<TKey, TValue> Build(Action<IDictionary<TKey, EventActionValue<TValue>>> trackedChanges)
    {
        return _buildMode switch
        {
            ExistingDictionaryBuilder.BuildMode.ChangeWith => ChangeObject(trackedChanges),
            ExistingDictionaryBuilder.BuildMode.UpdateWith => UpdateObject(trackedChanges),
            _ => throw new NotImplementedException(_buildMode.ToString())
        };
    }

    private IDictionary<TKey, TValue> ChangeObject(Action<IDictionary<TKey, EventActionValue<TValue>>>? trackedChanges)
    {
        Dictionary<TKey, EventActionValue<TValue>> changes = [];

        foreach (var (key, actionValue) in _properties)
        {
            switch (actionValue.Action)
            {
                case EventAction.Add:
                    _source.Add(key, actionValue.Value);
                    break;
                case EventAction.Remove:
                    if (!_source.Remove(key)) continue;
                    break;
                case EventAction.Update:
                    _source[key] = actionValue.Value;
                    break;
            }
            changes.Add(key, actionValue);
        }

        if (changes.Count > 0 && trackedChanges is not null) trackedChanges(changes);

        return _source;
    }

    private void RemoveKey(IDictionary<TKey, TValue> source, TKey key)
    {
        if (key is null) return;

        if (!source.TryGetValue(key, out var value)) return;

        _properties.Add(key, new EventActionValue<TValue>(EventAction.Remove, value));
    }

    private IDictionary<TKey, TValue> UpdateObject(Action<IDictionary<TKey, EventActionValue<TValue>>>? trackedChanges)
    {
        Dictionary<TKey, EventActionValue<TValue>> changes = [];

        foreach (var (key, actionValue) in _properties)
        {
            if (actionValue.Action != EventAction.Update) continue;
            if (!_source.ContainsKey(key)) continue;

            _source[key] = actionValue.Value;
            
            changes.Add(key, actionValue);
        }

        if (changes.Count > 0 && trackedChanges is not null) trackedChanges(changes);

        return _source;
    }
}
