using Foundation.Collections.Generic;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.ComponentModel;

public static class DictionaryTransactionProvider
{
    public static DictionaryTransactionProvider<TKey, TValue> New<TKey, TValue>(IDictionary<TKey, TValue> dictionary) => new(dictionary);
}

/// <summary>
/// This decorates a dictionary as <see cref="ITransactionProvider{TTransaction}"/>
/// </summary>
/// <typeparam name="TKey">The type of the keys of the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values of the dictionary.</typeparam>
public class DictionaryTransactionProvider<TKey, TValue>
    : IDictionary<TKey, TValue>
    , ITransactionProvider<ITransaction<Guid>>
{
    private IDisposable? _commited;
    private readonly IDictionary<TKey, TValue> _dictionary;
    private readonly List<ActionEvent> _events = new();

    #region events
    private record ActionEvent(DictionaryAction Action);
    private record KeyEvent(DictionaryAction Action, TKey Key) : ActionEvent(Action);
    private record KeyValueEvent(DictionaryAction Action, TKey Key, TValue Value) : KeyEvent(Action, Key);

    #endregion events

    public DictionaryTransactionProvider(IDictionary<TKey, TValue> dictionary)
    {
        _dictionary = dictionary.ThrowIfNull();
    }

    public TValue this[TKey key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public ICollection<TKey> Keys => _dictionary.Keys;

    public ICollection<TValue> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool IsReadOnly => _dictionary.IsReadOnly;

    public void Add(TKey key, TValue value) => _events.Add(new KeyValueEvent(DictionaryAction.Add, key, value));

    public void Add(KeyValuePair<TKey, TValue> item) => _events.Add(new KeyValueEvent(DictionaryAction.Add, item.Key, item.Value));

    public ITransaction<Guid> BeginTransaction()
    {
        var transaction = new Transaction();
        _commited = transaction.Committed.Subscribe(OnTransactionCommitted);

        return transaction;
    }

    public void Clear() => _events.Add(new ActionEvent(DictionaryAction.Clear));

    public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    private void OnTransactionCommitted()
    {
        foreach (var @event in _events)
        {
            switch (@event)
            {
                case KeyValueEvent e when e.Action == DictionaryAction.Add: _dictionary.Add(e.Key, e.Value); break;
                case ActionEvent e when e.Action == DictionaryAction.Clear: _dictionary.Clear(); break;
                case KeyEvent e when e.Action == DictionaryAction.Remove: _dictionary.Remove(e.Key); break;
                case KeyValueEvent e when e.Action == DictionaryAction.Remove: _dictionary.Remove(Pair.New(e.Key,  e.Value)); break;
                case KeyValueEvent e when e.Action == DictionaryAction.Replace: _dictionary[e.Key] = e.Value; break;
            }
        }

        _commited?.Dispose();
    }

    public bool Remove(TKey key)
    {
        if(_dictionary.ContainsKey(key))
        {
            _events.Add(new KeyEvent(DictionaryAction.Remove, key));
            return true;
        }
        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (_dictionary.Contains(item))
        {
            _events.Add(new KeyValueEvent(DictionaryAction.Remove, item.Key, item.Value));
            return true;
        }
        return false;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() =>_dictionary.GetEnumerator();
}
