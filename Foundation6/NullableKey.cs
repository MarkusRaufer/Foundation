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
    public struct NullableKey<T> : IEquatable<NullableKey<T>>
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

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if(obj is NullableKey<T> key) return Equals(key);

            return false;
        }

        public bool Equals(NullableKey<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode() => Value is null ? 0 : Value.GetHashCode();

        public bool IsNull => Value is null;

        public T? Value { get; }

        public override string ToString() => $"{Value}";
    }
}
