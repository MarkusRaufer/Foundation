namespace Foundation.ComponentModel;

public static class ObservableValue
{
    public static ObservableValue<T> Create<T>(T value)
    {
        return new ObservableValue<T>(value);
    }
}

public struct ObservableValue<T> : IEquatable<ObservableValue<T>>
{
    public event Action<T>? ValueChanged;

    private T _value;

    public ObservableValue(T? value)
    {
        _value = value.ThrowIfNull(nameof(value));
        ValueChanged = default;
    }

    public static bool operator ==(ObservableValue<T> left, ObservableValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ObservableValue<T> left, ObservableValue<T> right)
    {
        return !(left == right);
    }

    public bool IsEmpty => null == Value;

    public bool Equals(ObservableValue<T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ObservableValue<T> other) return false;
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty && Value!.Equals(other.Value);
    }

    public override int GetHashCode() => IsEmpty ? 0 : Value!.GetHashCode();

    public T Value
    {
        get => _value;
        set
        {
            if (null != _value && _value.Equals(value)) return;

            _value = value;
            if (null != ValueChanged) ValueChanged(_value);
        }
    }
}
