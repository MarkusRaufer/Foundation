namespace Foundation;

using System.ComponentModel;

public class Property : Property<object>
{
    public Property(string name, object? value = null) : base(name, value)
    {
    }
}

public class Property<TValue>
    : IEquatable<Property<TValue>>
    , INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _hashCode;
    private TValue? _value;

    public Property(string name, TValue? value = default)
    {
        Name = name.ThrowIfNullOrEmpty(nameof(name));
        _value = value;

        IsPropertyChangedActive = true;

        _hashCode = System.HashCode.Combine(Name, _value);
    }

    public static bool operator ==(Property<TValue>? lhs, Property<TValue>? rhs)
    {
        if (lhs is null) return rhs is null;
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Property<TValue>? lhs, Property<TValue>? rhs)
    {
        return !(lhs == rhs);
    }

    public void Deconstruct(out string name, out object? value)
    {
        name = Name;
        value = Value;
    }

    
    public override bool Equals(object? obj) => Equals(obj as Property);

    public bool Equals(Property<TValue>? other)
    {
        return null != other
            && EqualityComparer<string>.Default.Equals(Name, other.Name)
            && EqualityComparer<object>.Default.Equals(Value, other.Value);
    }

    public override int GetHashCode() => _hashCode;

    public bool IsDirty { get; set; }

    public bool IsPropertyChangedActive { get; set; }

    public string Name { get; }

    public override string ToString() => $"({Name}, {Value})";

    public TValue? Value
    {
        get => _value;
        set
        {
            if (_value.EqualsNullable(value)) return;

            _value = value;
            if (IsPropertyChangedActive)
            {
                IsDirty = true;
                _hashCode = System.HashCode.Combine(Name, _value);

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
            }
        }
    }
}

