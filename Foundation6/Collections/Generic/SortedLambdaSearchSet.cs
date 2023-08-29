using Foundation.Linq.Expressions;
using System.Linq.Expressions;

namespace Foundation.Collections.Generic
{
    public class SortedLambdaSearchSet<T>
        : SortedSet<T>
        , ILambdaSearch<T, IEnumerable<T>>
    {
        private readonly SortedSet<T> _sortedSet;

        public SortedLambdaSearchSet() : this(new SortedSet<T>())
        {
        }

        public SortedLambdaSearchSet(IEnumerable<T> items) : this(new SortedSet<T>(items))
        {
        }

        public SortedLambdaSearchSet(SortedSet<T> sortedSet)
        {
            _sortedSet = sortedSet.ThrowIfNull();
        }

        public IEnumerable<T> FindAll(LambdaExpression lambda)
        {
            if (!lambda.ThrowIfNull().IsPredicate())
                throw new ArgumentOutOfRangeException(nameof(lambda), $"is not a predicate");

            if (1 != lambda.Parameters.Count)
                throw new ArgumentOutOfRangeException(nameof(lambda), $"exact one parameter expected");

            if (lambda.Parameters.First().Type != typeof(T))
                throw new ArgumentOutOfRangeException(nameof(lambda), $"wrong parameter type");

            var func = (Func<T, bool>)lambda.Compile();

            return func is null ? Enumerable.Empty<T>() : FindAll(func);
        }

        public IEnumerable<T> FindAll(Func<T, bool> predicate)
        {
            predicate.ThrowIfNull();
            return _sortedSet.Where(x => predicate(x));
        }
    }
}
