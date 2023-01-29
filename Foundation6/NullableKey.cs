using System.Diagnostics.CodeAnalysis;

namespace Foundation
{
    public static class NullableKey
    {
        public static NullableKey<T> New<T>(T? value) => new (value);
    }

    /// <summary>
    /// Can be used as key for dictionaries. This allows null keys.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct NullableKey<T> 
        : IComparable<NullableKey<T>>
        , IEquatable<NullableKey<T>>
    {
        public NullableKey(T? value)
        {
            Value = value;
        }

        public static bool operator ==(NullableKey<T> left, NullableKey<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NullableKey<T> left, NullableKey<T> right)
        {
            return !(left == right);
        }

        public static bool operator <(NullableKey<T> left, NullableKey<T> right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(NullableKey<T> left, NullableKey<T> right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(NullableKey<T> left, NullableKey<T> right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(NullableKey<T> left, NullableKey<T> right)
        {
            return left.CompareTo(right) >= 0;
        }

        public int CompareTo(NullableKey<T> other)
        {
            if (IsNull) return other.IsNull ? 0 : -1;
            if (other.IsNull) return 1;

            if (Value is IComparable<T> left) return left.CompareTo(other.Value);

            if (other.Value is IComparable<T> right) return right.CompareTo(Value) * -1;

            throw new InvalidOperationException("values are not comparable");
        }

        public override bool Equals([NotNullWhen(true)] object? obj) => obj is NullableKey<T> other && Equals(other);
        
        public bool Equals(NullableKey<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode() => Value.GetNullableHashCode();

        public bool IsNull => Value is null;

        public T? Value { get; }

        public override string ToString() => $"{Value}";
    }
}
