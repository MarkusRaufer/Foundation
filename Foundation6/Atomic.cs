using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public struct Atomic<T> : IEquatable<Atomic<T>>
{
    private readonly string _name;

    public Atomic()
    {
        _name = typeof(T).FullName.ThrowIfNull();
    }

    public static bool operator ==(Atomic<T> left, Atomic<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Atomic<T> left, Atomic<T> right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Atomic<T> other && Equals(other);   

    public bool Equals(Atomic<T> other)
    {
        if (null == _name) return null == other._name;

        return _name.Equals(other._name);
    }

    override public int GetHashCode() => null == _name ? 0 : _name.GetHashCode();

    public override string ToString() => _name;
}
