namespace Foundation;

using System.ComponentModel;

public class Property
    : IEquatable<Property>
    , INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _hashCode;
    private object? _value;

    public Property(string name, object? value = default)
    {
        Name = name.ThrowIfNullOrEmpty(nameof(name));
        _value = value;

        IsPropertyChangedActive = true;

        _hashCode = System.HashCode.Combine(Name, _value);
    }

    public static bool operator ==(Property? lhs, Property? rhs)
    {
        if (lhs is null) return rhs is null;
        return lhs.Equals(rhs);
    }

    public void Deconstruct(out string name, out object? value)
    {
        name = Name;
        value = Value;
    }

    public static bool operator !=(Property? lhs, Property? rhs)
    {
        return !(lhs == rhs);
    }

    public override bool Equals(object? obj) => Equals(obj as Property);

    public bool Equals(Property? other)
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

    public object? Value
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

