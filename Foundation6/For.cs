using System.Collections;

namespace Foundation
{
    public interface IForReturns<T>
    {
        IEnumerable<T> Returns(Func<T, T> generator);
    }

    /// <summary>
    /// For is a generator that returns an endless list of items. Be aware to limit the number of generated items.
    /// You can use Take or TakeUntil.
    /// </summary>
    public static class For
    {
        public static IEnumerable<T> Returns<T>(Func<T> generator)
        {
            return new ForEnumerable<T>(generator);
        }

        public static IForReturns<T> StartAt<T>(Func<T> seed) where T : notnull
        {
            return new ForReturns<T>(seed);
        }
    }

    class ForReturns<T> : IForReturns<T>
        where T : notnull
    {
        private readonly Func<T> _seed;

        public ForReturns(Func<T> seed)
        {
            _seed = seed.ThrowIfNull();
        }

        public IEnumerable<T> Returns(Func<T, T> generator)
        {
            return new ForEnumerableWithSeed<T>(_seed, generator);
        }
    }

    class ForEnumerable<T> : IEnumerable<T>
    {
        private readonly Func<T> _generator;

        public ForEnumerable(Func<T> generator)
        {
            _generator = generator.ThrowIfNull();
        }

        public IEnumerator<T> GetEnumerator() => new ForEnumerator<T>(_generator);

        IEnumerator IEnumerable.GetEnumerator() => new ForEnumerator<T>(_generator);
    }

    class ForEnumerableWithSeed<T> : IEnumerable<T>
        where T : notnull
    {
        private readonly Func<T, T> _generator;
        private readonly Func<T> _seed;

        public ForEnumerableWithSeed(Func<T> seed, Func<T, T> generator)
        {
            _seed = seed.ThrowIfNull();
            _generator = generator.ThrowIfNull();
        }

        public IEnumerator<T> GetEnumerator() => new ForEnumeratorWithSeed<T>(_seed, _generator);

        IEnumerator IEnumerable.GetEnumerator() => new ForEnumeratorWithSeed<T>(_seed, _generator);
    }

    class ForEnumerator<T> : IEnumerator<T>
    {
        private T? _current;
        private readonly Func<T> _generator;

        public ForEnumerator(Func<T> generator) => _generator = generator.ThrowIfNull();

        public T Current => _current ?? throw new InvalidOperationException("Current should not be used in the current state");

        object IEnumerator.Current => Current!;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            _current = _generator();
            return true;
        }

        public void Reset()
        {
        }
    }

    class ForEnumeratorWithSeed<T> : IEnumerator<T>
        where T : notnull
    {
        private T? _current;
        private readonly Func<T, T> _generator;
        private bool _passed;
        private readonly Func<T> _seed;

        public ForEnumeratorWithSeed(Func<T> seed, Func<T, T> generator)
        {
            _seed = seed.ThrowIfNull();
            _generator = generator.ThrowIfNull();
        }

        public T Current => _current ?? throw new InvalidOperationException("Current should not be used in the current state");

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            _current = !_passed ? _seed() : _generator(Current);

            _passed = true;

            return true;
        }

        public void Reset()
        {
            _current = default;
            _passed = false;
        }
    }
}
