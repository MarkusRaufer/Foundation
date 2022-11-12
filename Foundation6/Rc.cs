namespace Foundation;

public static class Rc
{
    /// <summary>
    /// Creates a reference counter object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Rc<T> New<T>(T obj)
    {
        return new Rc<T>(obj);
    }
}

/// <summary>
/// This is a reference counter.
/// </summary>
/// <typeparam name="T"></ typeparam >
public class Rc<T>
    : IComparable<Rc<T>>
    , IEquatable<Rc<T>>
{
    private readonly T _value;

    public Rc(T value) : this(value, 0)
    {
    }

    public Rc(T value, int counter)
    {
        _value = value.ThrowIfNull();
        Counter = counter;
    }

    public static implicit operator T(Rc<T> rc)
    {
        return rc.Get();
    }

    public static bool operator ==(Rc<T> left, Rc<T> right)
    {
        if(left is null) return right is null;

        return left.Equals(right);
    }

    public static bool operator !=(Rc<T> left, Rc<T> right)
    {
        return !(left == right);
    }

    public int CompareTo(Rc<T>? other)
    {
        if (other is null) return 1;

        if (!ValueEquals(other)) return -1;

        return Counter.CompareTo(other.Counter);
    }

    /// <summary>
    /// The number how often Get is called.
    /// </summary>
    public int Counter { get; private set; }

    public override bool Equals(object? obj) => Equals(obj as Rc<T>);

    /// <summary>
    /// Is true if Object and Counter are equal.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Rc<T>? other)
    {
        return other is not null
            && Counter == other.Counter
            && EqualityComparer<T>.Default.Equals(_value, other._value);
    }

    /// <summary>
    /// On every call the counter will be incremented.
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        Counter++;
        return _value;
    }

    public override int GetHashCode() => System.HashCode.Combine(_value, Counter);

    public int GetValueHashCode() => _value!.GetHashCode();

    public void Reset()
    {
        Counter = 0;
    }

    public override string ToString() => $"Value: {_value}, {nameof(Counter)}: {Counter}";

    /// <summary>
    /// Returns true if <paramref name="other"/> equals the internal _object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool ValueEquals(T? other)
    {
        return EqualityComparer<T>.Default.Equals(_value, other);
    }

    /// <summary>
    /// Returns true if Object equals the other Object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool ValueEquals(Rc<T>? other)
    {
        return other is not null
            && EqualityComparer<T>.Default.Equals(_value, other._value);
    }
}

