using System.Collections;

namespace Foundation.Collections
{
    public class LambdaComparer : IComparer
    {
        private readonly Func<object?, object?, int> _compare;

        public LambdaComparer(Func<object?, object?, int> compare)
        {
            _compare = compare.ThrowIfNull();
        }

        public int Compare(object? x, object? y)
        {
            return (x, y) switch
            {
                (null, null) => 0,
                (null, _) => -1,
                (_, null) => 1,
                (_, _) => _compare(x, y)
            };
        }
    }

    public class LambdaComparer<T> : IComparer
    {
        private readonly Func<T, T, int> _compare;

        public LambdaComparer(Func<T, T, int> compare)
        {
            _compare = compare.ThrowIfNull();
        }

        public int Compare(object? x, object? y)
        {
            if(x is T tx && y is T ty)
                return _compare(tx, ty);

            if(x is IComparable comparableX) return comparableX.CompareTo(y);
            
            if(y is IComparable comparableY) return comparableY.CompareTo(x);

            return (x, y) switch
            {
                (null, null) => 0,
                (null, _) => -1,
                (_, null) => 1,
                (_, _) => throw new InvalidOperationException("values are not comparable")
            };
        }
    }
}
