namespace Foundation;

public static class HashCode
{
    public const int DefaultPrime = -1521134295;

    public readonly struct HashCodeBuilder : IEquatable<HashCodeBuilder>
    {
        private readonly int _hash;
        private readonly int _multiplier;
        private readonly int _seed;

        public HashCodeBuilder(HashCodeBuilder builder)
        {
            _hash = builder._hash;
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

        public HashCodeBuilder AddHashCode(int hashCode)
        {
            if (0 == hashCode) return this;

            return new HashCodeBuilder(CreateHashCode(_hash, hashCode), _multiplier);
        }

        public HashCodeBuilder AddHashCodes(IEnumerable<int> hashCodes)
        {
            if (null == hashCodes) return this;

            var hash = _hash;

            foreach (var hashCode in hashCodes)
                hash = CreateHashCode(hash, hashCode);

            return new HashCodeBuilder(hash, _multiplier);
        }

        public HashCodeBuilder AddObject<T>(params T?[] objects)
        {
            return AddObjects(objects);
        }

        public HashCodeBuilder AddObject<TKey, TValue>(params KeyValuePair<TKey, TValue>[] pairs)
        {
            return AddObjects(pairs);
        }

        public HashCodeBuilder AddObjects(IEnumerable<object> objects)
        {
            if (null == objects) return this;

            var hash = _hash;

            foreach (var value in objects)
            {
                if (null == value) continue;

                hash = CreateHashCode(hash, value.GetHashCode());
            }

            return new HashCodeBuilder(hash, _multiplier);
        }

        public HashCodeBuilder AddObjects<T>(IEnumerable<T?> objects)
        {
            if (null == objects) return this;

            var hash = _hash;

            foreach (var value in objects)
            {
                if (null == value) continue;

                hash = CreateHashCode(hash, value.GetHashCode());
            }

            return new HashCodeBuilder(hash, _multiplier);
        }

        public HashCodeBuilder AddObjects<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            if (null == pairs) return this;

            var hash = _hash;

            foreach (var pair in pairs)
            {
                if (null == pair.Key) continue;

                hash = CreateHashCode(hash, pair.Key.GetHashCode());

                if (null == pair.Value) continue;

                hash = CreateHashCode(hash, pair.Value.GetHashCode());
            }

            return new HashCodeBuilder(hash, _multiplier);
        }

        public HashCodeBuilder AddOrderedHashCode(params int[] hashCodes)
        {
            if (0 == hashCodes.Length) return this;

            Array.Sort(hashCodes);

            return new HashCodeBuilder(_hash, _multiplier).AddHashCodes(hashCodes);
        }

        public HashCodeBuilder AddOrderedHashCodes(IEnumerable<int> hashCodes)
        {
            return AddOrderedHashCode(hashCodes.ToArray());
        }

        public HashCodeBuilder AddOrderedObject<T>(params T[] objects)
        {
            if (0 == objects.Length) return this;
            
            return AddOrderedHashCode(objects.Select(o => o.GetNullableHashCode()).ToArray());
        }

        public HashCodeBuilder AddOrderedObjects<T>(IEnumerable<T> objects)
        {
            return AddOrderedObject(objects.ToArray());
        }


        public static HashCodeBuilder Create(int seed = 1502878410, int multiplier = DefaultPrime)
        {
            return new HashCodeBuilder(seed, multiplier);
        }

        private int CreateHashCode(int prevHashCode, int hashCode)
        {
            return prevHashCode * _multiplier + hashCode;
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
            return (IsInitialized) ? _hash : 0;
        }

        public bool IsInitialized { get; }
    }

    public static HashCodeBuilder CreateBuilder()
    {
        return HashCodeBuilder.Create();
    }

    public static int FromHashCode(params int[] hashCodes)
    {
        if (0 == hashCodes.Length) return 0;

        return FromHashCodes(hashCodes);
    }

    public static int FromHashCodes(IEnumerable<int> hashCodes)
    {
        return HashCodeBuilder.Create()
                              .AddHashCodes(hashCodes)
                              .GetHashCode();
    }

    public static int FromOrderedHashCode(params int[] hashCodes)
    {
        if (0 == hashCodes.Length) return 0;

        Array.Sort(hashCodes);

        return FromHashCodes(hashCodes);
    }

    public static int FromOrderedHashCodes(IEnumerable<int> hashCodes)
    {
        return FromOrderedHashCode(hashCodes.ToArray());
    }

    public static int FromOrderedObject<T>(params T[] objects)
    {
        if (0 == objects.Length) return 0;

        return FromOrderedHashCodes(objects.Select(o => o.GetNullableHashCode()));
    }

    public static int FromOrderedObjects<T>(IEnumerable<T> objects)
    {
        return FromOrderedObject(objects.ToArray());
    }


    public static int FromObject(params object[] objects)
    {
        return FromObjects(objects);
    }

    public static int FromObject<TKey, TValue>(params KeyValuePair<TKey, TValue>[] pairs)
    {
        return FromObjects(pairs);
    }

    public static int FromObjects<T>(IEnumerable<T> objects)
    {
        return HashCodeBuilder.Create()
                              .AddObjects(objects)
                              .GetHashCode();
    }

    public static int FromObjects<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
    {
        var keyValues = pairs.ToArray();
        if (0 == keyValues.Length) return 0;

        return HashCodeBuilder.Create()
                              .AddObjects(keyValues)
                              .GetHashCode();
    }
}


