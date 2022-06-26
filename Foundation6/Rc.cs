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
    : IComparable<Rc<T>>
    , IEquatable<Rc<T>>
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

    public int CompareTo(Rc<T> other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : -1;

        if (!ValueEquals(other)) return -1;

        return Counter.CompareTo(other.Counter);
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
}

