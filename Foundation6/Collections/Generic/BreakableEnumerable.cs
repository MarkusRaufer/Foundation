namespace Foundation.Collections.Generic;

using Foundation.ComponentModel;
using System.Collections;

public class BreakableEnumerable<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _items;
    private Action? _stop;

    public BreakableEnumerable(IEnumerable<T> items, ref ObservableValue<bool> stop)
    {
        _items = items;
        stop.ValueChanged += Break;
    }

    private void Break(bool stop)
    {
        _stop?.Invoke();
    }

    public IEnumerator<T> GetEnumerator() => new BreakableEnumerator(_items.GetEnumerator(), out _stop);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private class BreakableEnumerator : IEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        private bool _stop;

        public BreakableEnumerator(IEnumerator<T> enumerator, out Action stop)
        {
            _enumerator = enumerator;
            stop = Break;
        }

        public void Break()
        {
            _stop = true;
        }

        public T Current => _enumerator.Current;

        public void Dispose()
        {
            _stop = false;
            _enumerator.Dispose();
        }

        object? IEnumerator.Current => _enumerator.Current;

        public bool MoveNext()
        {
            if (_stop) return false;
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _stop = false;
            _enumerator.Reset();
        }
    }
}

