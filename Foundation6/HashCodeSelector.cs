using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public class HashCodeSelector
{
    public static HashCodeSelector<T> Create<T, TSelector>([DisallowNull] T? obj, params Func<T, TSelector>[] selectors)
    {
        return new HashCodeSelector<T, TSelector>(obj, selectors);
    }
}

public abstract class HashCodeSelector<T>
{
    public HashCodeSelector([DisallowNull] T? obj)
    {
        Object = obj.ThrowIfNull(nameof(obj));
    }

    public abstract IEnumerable<int> GetHashCodesFromSelectors();

    public T Object { get; }
}

public class HashCodeSelector<T, TSelector> : HashCodeSelector<T>
{
    public HashCodeSelector([DisallowNull] T? obj, params Func<T, TSelector>[] selectors) : base(obj)
    {
        if (0 == selectors.Length)
            throw new ArgumentOutOfRangeException(nameof(selectors), "selectors must have at least one selector");

        Selectors = selectors;
    }

    public override IEnumerable<int> GetHashCodesFromSelectors()
    {
        foreach (var selector in Selectors)
        {
            var selected = selector(Object);

            if (null != selected) yield return selected.GetHashCode();
        }
    }

    public Func<T, TSelector>[] Selectors { get; private set; }
}

