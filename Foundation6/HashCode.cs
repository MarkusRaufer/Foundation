using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public static class HashCode
{
    public const int DefaultPrime = -1521134295;

    public struct HashCodeBuilder
    : IEquatable<HashCodeBuilder>
    , IClearable
    {
        private int _hash;
        private bool _hashCodeCreated;
        private readonly int _multiplier;
        private readonly int _seed;

        public HashCodeBuilder(HashCodeBuilder builder)
        {
            _hash = builder._hash;
            _hashCodeCreated = builder._hashCodeCreated;
            _multiplier = builder._multiplier;
            _seed = builder._seed;

            IsInitialized = true;
        }

        public HashCodeBuilder(int seed, int multiplier)
        {
            if (0 == seed)
                throw new ArgumentOutOfRangeException(nameof(seed), "seed must not be zero");

            if (0 == multiplier)
                throw new ArgumentOutOfRangeException(nameof(multiplier), "multiplier must not be zero");

            if (multiplier % 2 == 0)
                throw new ArgumentOutOfRangeException(nameof(multiplier), "multiplier must be an odd value");

            _hash = seed;
            _hashCodeCreated = false;
            _multiplier = multiplier;
            _seed = seed;

            IsInitialized = true;
        }

        public static bool operator ==(HashCodeBuilder lhs, HashCodeBuilder rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(HashCodeBuilder lhs, HashCodeBuilder rhs)
        {
            return !(lhs == rhs);
        }

        public void AddHashCode(int hashCode)
        {
            if (0 == hashCode) return;

            _hash = _hash * _multiplier + hashCode;
            _hashCodeCreated = true;
        }

        public void AddHashCodes([DisallowNull] IEnumerable<int> hashCodes)
        {
            if (null == hashCodes) return;

            foreach (var hashCode in hashCodes)
                AddHashCode(hashCode);
        }

        public void AddObject<T>(params T?[] objects)
        {
            AddObjects(objects);
        }

        public void AddObjects([DisallowNull] IEnumerable<object> objects)
        {
            if (null == objects) return;

            foreach (var value in objects)
            {
                if (null == value) continue;

                AddHashCode(value.GetHashCode());
            }
        }

        public void AddObjects<T>([DisallowNull] IEnumerable<T?> objects)
        {
            if (null == objects) return;

            foreach (var value in objects)
            {
                if (null == value) continue;

                AddHashCode(value.GetHashCode());
            }
        }

        public void Clear()
        {
            _hash = _seed;
            _hashCodeCreated = false;
        }

        public static HashCodeBuilder Create(int seed = 1502878410, int multiplier = DefaultPrime)
        {
            return new HashCodeBuilder(seed, multiplier);
        }

        public static HashCodeBuilder Empty => new();

        public override bool Equals(object? obj)
        {
            return obj is HashCodeBuilder other && Equals(other);
        }

        public bool Equals(HashCodeBuilder other)
        {
            if (!IsInitialized) return !other.IsInitialized;

            if (!other.IsInitialized) return false;

            return _hash == other._hash
                && _multiplier == other._multiplier
                && _seed == other._seed;
        }

        public override int GetHashCode()
        {
            return (_hashCodeCreated) ? _hash : 0;
        }

        public bool IsInitialized { get; private set; }
    }

    public static HashCodeBuilder CreateBuilder()
    {
        return HashCodeBuilder.Create();
    }

    public static int From(int hashCode, params object[] objects)
    {
        if (0 == hashCode && 0 == objects.Length) return 0;
        objects.ThrowIfNull();

        var hcb = HashCodeBuilder.Create();
        hcb.AddHashCode(hashCode);
        hcb.AddObjects(objects);
        return hcb.GetHashCode();
    }

    public static int FromHashCode(params int[] hashCodes)
    {
        if (0 == hashCodes.Length) return 0;

        var hcb = HashCodeBuilder.Create();
        hcb.AddHashCodes(hashCodes);

        return hcb.GetHashCode();
    }

    public static int FromHashCodeOrdered(params int[] hashCodes)
    {
        if (0 == hashCodes.Length) return 0;

        Array.Sort(hashCodes);

        return FromHashCode(hashCodes);
    }

    public static int FromHashCodes([DisallowNull] IEnumerable<int> hashCodes)
    {
        return FromHashCode(hashCodes.ToArray());
    }

    public static int FromHashCodesOrdered([DisallowNull] IEnumerable<int> hashCodes)
    {
        var arr = hashCodes.ToArray();
        Array.Sort(arr);

        return FromHashCode(arr);
    }

    public static int FromObject(params object?[] objects)
    {
        if (0 == objects.Length) return 0;

        var hcb = HashCodeBuilder.Create();

        foreach (var obj in objects)
            hcb.AddObject(obj);

        return hcb.GetHashCode();
    }

    public static int FromObjects<T>([DisallowNull] IEnumerable<T> objects)
    {
        var list = objects.ToArray();
        if (0 == list.Length) return 0;

        var hcb = HashCodeBuilder.Create();

        hcb.AddObjects(objects);

        return hcb.GetHashCode();
    }
}

