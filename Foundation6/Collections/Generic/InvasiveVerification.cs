namespace Foundation.Collections.Generic;

public static class InvasiveVerification
{
    public static InvasiveVerification<T> New<T>(params Func<T, bool>[] predicates) => new(predicates);
}

/// <summary>
/// a list of predicates that are checked each only one times.
/// </summary>
/// <typeparam name="T"></typeparam>
public class InvasiveVerification<T>
{
    private readonly IList<Func<T, bool>> _predicates;

    public InvasiveVerification(IEnumerable<Func<T, bool>> predicates)
    {
        _predicates = predicates.ToList();
    }

    /// <summary>
    /// If a predicate matches, it is removed from the list. If multiple predicates are matching on one item all of them are removed from the list.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>return a TriState. IsNone means, there are no more predicates in the list. If it does not match false is returned.</returns>
    public TriState Verify(T item)
    {
        for (var i = 0; i < _predicates.Count; i++)
        {
            var predicate = _predicates[i];
            if (null == predicate)
            {
                _predicates.RemoveAt(i);
                continue;
            }

            if (predicate(item))
            {
                _predicates.RemoveAt(i);

                return new TriState(true);
            }
        }

        return 0 == _predicates.Count ? new TriState() : new TriState(false);
    }

    public int PredicateCount => _predicates.Count;

    public IEnumerable<Func<T, bool>> Predicates => _predicates;
}

