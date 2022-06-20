namespace Foundation;

using System.Diagnostics.CodeAnalysis;

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
/// This is a reference counter;
/// </summary>
/// <typeparam name = "T" ></ typeparam >
public struct Rc<T>
    : IEquatable<Rc<T>>
    , IOptionalComparable<Rc<T>>
    where T : notnull, IEquatable<T>
{
    public Rc(T obj) : this(obj, 0)
    {
    }

    public Rc(T obj, int counter)
    {
        Object = obj.ThrowIfNull();
        Counter = counter;
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
               EqualityComparer<T>.Default.Equals(Object, other.Object);
    }

    /// <summary>
    /// On every call the counter will be incremented.
    /// </summary>
    /// <returns></returns>
    public Opt<T> Get()
    {
        Counter++;
        return Opt.Maybe(Object);
    }

    public override int GetHashCode() => System.HashCode.Combine(Counter, Object);

    public bool IsEmpty => Object is null;

    /// <summary>
    /// Returns true if obj equals the internal Object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool ObjectEquals(T obj)
    {
        if (IsEmpty) return false;

        return EqualityComparer<T>.Default.Equals(Object, obj);
    }

    /// <summary>
    /// Returns true if Object equals the other Object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool ObjectEquals(Rc<T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return EqualityComparer<T>.Default.Equals(Object, other.Object);
    }


    private T? Object { get; }

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

    public override string ToString() => $"T: {Object}, Counter: {Counter}";
}

