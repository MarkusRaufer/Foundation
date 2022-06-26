using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public static class Rc
{
    /// <summary>
    /// Creates a reference counter object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Rc<T> New<T>(T obj) where T : notnull, IEquatable<T>
    {
        return new Rc<T>(obj);
    }
}

/// <summary>
/// This is a reference counter.
/// </summary>
/// <typeparam name="T"></ typeparam >
public struct Rc<T>
    : IEquatable<Rc<T>>
    , IOptionalComparable<Rc<T>>
    where T : notnull, IEquatable<T>
{
    private readonly T? _value;

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
        return rc.Get().OrThrow();
    }

    public static bool operator ==(Rc<T> left, Rc<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rc<T> left, Rc<T> right)
    {
        return !(left == right);
    }

    /// <summary>
    /// The number how often Get is called.
    /// </summary>
    public int Counter { get; private set; }

    public override bool Equals(object? obj) => obj is Rc<T> other && Equals(other);

    /// <summary>
    /// Is true if Object and Counter are equal.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Rc<T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty &&
               Counter == other.Counter &&
               EqualityComparer<T>.Default.Equals(_value, other._value);
    }

    /// <summary>
    /// On every call the counter will be incremented.
    /// </summary>
    /// <returns></returns>
    public Opt<T> Get()
    {
        Counter++;
        return Opt.Maybe(_value);
    }

    public override int GetHashCode() => IsEmpty ? 0 : System.HashCode.Combine(Counter, _value);

    public bool IsEmpty => _value is null;

    /// <summary>
    /// Returns true if <paramref name="other"/> equals the internal _object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool ValueEquals(T other)
    {
        if (IsEmpty) return false;

        return EqualityComparer<T>.Default.Equals(_value, other);
    }

    /// <summary>
    /// Returns true if Object equals the other Object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool ValueEquals(Rc<T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty
            && EqualityComparer<T>.Default.Equals(_value, other._value);
    }

    /// <summary>
    /// Compares the counters. The values must be same. If the values are different None is returned.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>Returns Some(int) if values are same otherwise None.</returns>
    public Opt<int> OptionalCompareTo(Rc<T> other)
    {
        if (!ValueEquals(other)) return Opt.None<int>();

        return Counter.CompareTo(other.Counter);
    }

    public void Reset()
    {
        Counter = 0;
    }

    public override string ToString() => $"{nameof(T)}: {_value}, {nameof(Counter)}: {Counter}";

    public bool TryGet([MaybeNullWhen(false)] out T? value)
    {
        if(IsEmpty)
        {
            value = default;
            return false;
        }

        Counter++;
        value = _value;

        return true;
    }
}

