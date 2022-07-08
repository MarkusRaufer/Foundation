using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.ObjectModel;

public class PropertyCollection
    : IPropertyCollection
    , IEquatable<PropertyCollection>
    , IDisposable
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly Dictionary<string, Action> _disposeActions;
    private bool _disposing;
    private int _hashCode;
    private bool _isHashCodeUpToDate;
    private bool _isPropertyChangedActive;
    private readonly SortedDictionary<string, Property> _properties;

    public PropertyCollection()
    {
        _properties = new SortedDictionary<string, Property>();
        _disposeActions = new Dictionary<string, Action>();
    }

    ~PropertyCollection()
    {
        Dispose();
    }
    public PropertyCollection(IEnumerable<Property> properties)
    {
        properties.ThrowIfNull();

        _properties = new SortedDictionary<string, Property>(properties.ToDictionary(p => p.Name, p => p));
        _disposeActions = new Dictionary<string, Action>();

        RegisterPropertyChanged();
    }

    public object? this[string propertyName]
    {
        get => _properties[propertyName].Value;
        set => _properties[propertyName].Value = value;
    }

    public void Add(Property property)
    {
        if (IsReadOnly) throw new InvalidOperationException("collection is readonly");

        property.ThrowIfNull();

        if (_properties.ContainsKey(property.Name))
            throw new ArgumentException($"property {property.Name} exists");

        _isHashCodeUpToDate = false;
        _properties.Add(property.Name, property);

        CollectionChanged?.Invoke(
                            this,
                            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, property));

        RegisterPropertyChanged(property);
    }

    public void Clear()
    {
        _isHashCodeUpToDate = false;
        _properties.Clear();
        CollectionChanged?.Invoke(
                this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool Contains(Property property)
    {
        return ContainsProperty(property.Name);
    }

    public bool ContainsProperty(string name)
    {
        return _properties.ContainsKey(name);
    }

    public void CopyTo(Property[] array, int arrayIndex)
    {
        if ((array.Length - 1) < arrayIndex) throw new ArgumentOutOfRangeException(nameof(arrayIndex));

        var it = _properties.Values.GetEnumerator();
        for (var i = arrayIndex; i < array.Length; i++)
        {
            if (!it.MoveNext()) break;
            array[i] = it.Current;
        }
    }

    public int Count => _properties.Count;

    public void Dispose()
    {
        if (_disposing) return;
        _disposing = true;

        foreach (var action in _disposeActions.Values)
            action();

        _properties.Clear();
        _disposeActions.Clear();

        GC.SuppressFinalize(this);
    }

    public override bool Equals(object? obj) => Equals(obj as PropertyCollection);

    public bool Equals(PropertyCollection? other)  => null != other && _properties.SequenceEqual(other._properties);

    public IEnumerator<Property> GetEnumerator() => _properties.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _properties.Values.GetEnumerator();

    public override int GetHashCode()
    {
        if(!_isHashCodeUpToDate)
            _hashCode = HashCode.FromObjects(_properties.Values);

        return _hashCode;
    }
    public bool IsPropertyChangedActive
    {
        get => _isPropertyChangedActive;
        set
        {
            if (_isPropertyChangedActive == value) return;
            _isPropertyChangedActive = value;

            foreach (var property in _properties.Values)
                property.IsPropertyChangedActive = _isPropertyChangedActive;
        }
    }

    public bool IsReadOnly { get; set; } = false;

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _isHashCodeUpToDate = false;

        PropertyChanged?.Invoke(sender, e);
    }

    protected void RegisterPropertyChanged()
    {
        foreach (var kvp in _properties)
        {
            RegisterPropertyChanged(kvp.Value);
        }
    }

    protected void RegisterPropertyChanged(Property property)
    {
        property.ThrowIfNull();

        if (_disposeActions.ContainsKey(property.Name)) return;

        property.PropertyChanged += OnPropertyChanged;
        void action() => property.PropertyChanged -= OnPropertyChanged;
        _disposeActions.Add(property.Name, action);
    }

    public bool Remove(string name)
    {
        if (IsReadOnly) throw new InvalidOperationException("collection is readonly");

        if (!TryGetProperty(name, out Property? property)) return false;

        return Remove(property);
    }

    public bool Remove(Property property)
    {
        if (IsReadOnly) throw new InvalidOperationException("collection is readonly");

        property.ThrowIfNull();

        var removed = _properties.Remove(property.Name);
        if(removed)
        {
            _isHashCodeUpToDate = false;

            CollectionChanged?.Invoke(
                this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, property));

            UnRegisterPropertyChanged(property.Name);
        }

        return removed;
    }

    public bool TryGetProperty(string name, [MaybeNullWhen(false)] out Property property)
    {
        return _properties.TryGetValue(name, out property);
    }

    protected void UnRegisterPropertyChanged(string propertyName)
    {
        if (!_disposeActions.TryGetValue(propertyName, out Action? action)) return;

        _disposeActions.Remove(propertyName);

        action();
    }
}
