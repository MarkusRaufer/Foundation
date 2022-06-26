namespace Foundation.Collections.Generic;

using System.Collections;

public class CyclicEnumerable<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _items;

    public CyclicEnumerable(IEnumerable<T> items)
    {
        _items = items.ThrowIfNull();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new CyclicEnumerator(_items);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new CyclicEnumerator(_items);
    }

    private class CyclicEnumerator : IEnumerator<T>
    {
        private readonly IEnumerable<T> _enumerable;
        private IEnumerator<T> _enumerator;

        public CyclicEnumerator(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable.ThrowIfNull();

            _enumerator = _enumerable.GetEnumerator();
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

            _enumerator = _enumerable.GetEnumerator();

            return _enumerator.MoveNext();
        }

        public void Reset()
        {
        }
    }
}

public class CyclicEnumerable<T, TCount> : IEnumerable<(TCount, T)>
    where TCount : IComparable<TCount>
{
    private readonly IEnumerable<T> _enumerable;
    private readonly Func<TCount, TCount> _increment;
    public CyclicEnumerable(IEnumerable<T> items, TCount min, TCount max, Func<TCount, TCount> increment)
    {
        _enumerable = items.ThrowIfNull();
        _increment = increment.ThrowIfNull();

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

        public CyclicEnumerator(IEnumerable<T> items, TCount min, TCount max, Func<TCount, TCount> increment)
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

