namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public class CyclicEnumerable<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _items;

    public CyclicEnumerable([DisallowNull] IEnumerable<T> items)
    {
        _items = items.ThrowIfNull(nameof(items));
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new CyclicEnumerator(_items.GetEnumerator());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new CyclicEnumerator(_items.GetEnumerator());
    }

    private class CyclicEnumerator : IEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        public CyclicEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public T Current => _enumerator.Current;

        object? IEnumerator.Current => _enumerator.Current;

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public bool MoveNext()
        {
            if (_enumerator.MoveNext()) return true;

            Reset();
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }
    }
}

public class CyclicEnumerable<T, TCount> : IEnumerable<(TCount, T)>
    where TCount : IComparable<TCount>
{
    private readonly IEnumerable<T> _enumerable;
    private readonly Func<TCount, TCount> _increment;
    public CyclicEnumerable([DisallowNull] IEnumerable<T> items, TCount min, TCount max, [DisallowNull] Func<TCount, TCount> increment)
    {
        if (null == increment) throw new ArgumentNullException(nameof(increment));
        _enumerable = items;
        _increment = increment;
        Min = min;
        Max = max;
    }

    public TCount Max { get; private set; }
    public TCount Min { get; private set; }

    public IEnumerator GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator<(TCount, T)> IEnumerable<(TCount, T)>.GetEnumerator()
    {
        return new CyclicEnumerator(_enumerable, Min, Max, _increment);
    }


    private class CyclicEnumerator : IEnumerator<(TCount, T)>
    {
        private readonly IEnumerator<T> _enumerator;
        private readonly Func<TCount, TCount> _increment;
        private bool _passed = false;

        public CyclicEnumerator([DisallowNull] IEnumerable<T> items, TCount min, TCount max, [DisallowNull] Func<TCount, TCount> increment)
        {
            _enumerator = items.GetEnumerator();
            _increment = increment;
            Counter = min;
            Min = min;
            Max = max;
        }

        public TCount Counter { get; set; }

        public void Dispose()
        {
        }

        public TCount Max { get; private set; }
        public TCount Min { get; private set; }

        (TCount, T) IEnumerator<(TCount, T)>.Current => (Counter, _enumerator.Current);

        public object? Current => _enumerator.Current;

        public bool MoveNext()
        {
            if (!_enumerator.MoveNext()) return false;
            if (_passed)
            {
                var count = _increment(Counter);
                Counter = (1 == count.CompareTo(Max)) ? Min : count;
            }
            else _passed = true;

            return true;
        }

        public void Reset()
        {
            _enumerator.Reset();
            _passed = false;
        }
    }
}

