using System.Collections;
using System.Collections.ObjectModel;

namespace Foundation.Collections.Generic
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumerableCounter<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _items;

        public EnumerableCounter(IEnumerable<T> items)
        {
            _items = items.ThrowIfEnumerableIsNull();
        }

        public EnumerableCounter(IEnumerable<T> items, out Func<EnumerableCounter<T>> self)
        {
            _items = items.ThrowIfEnumerableIsNull();
            self = () => this;
        }
        
        public static implicit operator EnumerableCounter<T>(T[] items)
        {
            return new EnumerableCounter<T>(items);
        }

        public static implicit operator EnumerableCounter<T>(Collection<T> items)
        {
            return new EnumerableCounter<T>(items);
        }

        public static implicit operator EnumerableCounter<T>(List<T> items)
        {
            return new EnumerableCounter<T>(items);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Count = 0;
            return new CounterEnumerator(_items.GetEnumerator(), IncrementCount);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Count = 0;
            return new CounterEnumerator(_items.GetEnumerator(), IncrementCount);
        }

        public int Count { get; protected set; }

        protected void IncrementCount()
        {
            Count++;
        }

        private class CounterEnumerator : IEnumerator<T>
        {
            private readonly IEnumerator<T> _enumerator;
            private readonly Action _callOnMoveNext;

            public CounterEnumerator(IEnumerator<T> enumerator, Action callOnMoveNext)
            {
                _callOnMoveNext = callOnMoveNext;
                _enumerator = enumerator;
            }
            public T Current => _enumerator.Current;

            object? IEnumerator.Current => _enumerator.Current;

            public void Dispose() => _enumerator.Dispose();

            public bool MoveNext()
            {
                var next = _enumerator.MoveNext();
                if (next) _callOnMoveNext();

                return next;
            }

            public void Reset() => _enumerator.Reset();
        }
    }
}
