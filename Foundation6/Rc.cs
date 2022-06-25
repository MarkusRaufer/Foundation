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
    private readonly T? _object;

    public Rc(T obj) : this(obj, 0)
    {
    }

    public Rc(T obj, int counter)
    {
        _object = obj.ThrowIfNull();
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
               EqualityComparer<T>.Default.Equals(_object, other._object);
    }

    /// <summary>
    /// On every call the counter will be incremented.
    /// </summary>
    /// <returns></returns>
    public Opt<T> Get()
    {
        Counter++;
        return Opt.Maybe(_object);
    }

    public override int GetHashCode() => System.HashCode.Combine(Counter, _object);

    public bool IsEmpty => _object is null;

    /// <summary>
    /// Returns true if <paramref name="other"/> equals the internal _object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool ObjectEquals(T other)
    {
        if (IsEmpty) return false;

        return EqualityComparer<T>.Default.Equals(_object, other);
    }

    /// <summary>
    /// Returns true if Object equals the other Object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool ObjectEquals(Rc<T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty
            && EqualityComparer<T>.Default.Equals(_object, other._object);
    }

    /// <summary>
    /// Compares the counters. The object must be same.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public Opt<int> OptionalCompareTo(Rc<T> other)
    {
        if (!ObjectEquals(other)) return Opt.None<int>();

        return Counter.CompareTo(other.Counter);
    }

    public void Reset()
    {
        Counter = 0;
    }

    public override string ToString() => $"T: {_object}, Counter: {Counter}";

    public bool TryGet([MaybeNullWhen(false)] out T? obj)
    {
        if(IsEmpty)
        {
            obj = default;
            return false;
        }

        Counter++;
        obj = _object;

        return true;
    }
}

