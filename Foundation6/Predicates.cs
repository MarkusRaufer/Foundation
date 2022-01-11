namespace Foundation;

using System.Collections;

public static class Predicates
{
    public static Func<object, bool> AndAlso(Func<object, bool> left, Func<object, bool> right)
    {
        return o => left(o) && right(o);
    }

    public static Func<T, bool> AndAlso<T>(params Func<T, bool>[] predicates)
    {
        return t => predicates.All(predicate => predicate(t));
    }

    public static Func<object, bool> Contains(object value)
    {
        return o => o is IEnumerable enumerable && enumerable.OfType<object>().Contains(value);
    }

    public static Func<object, bool> Contains<T>(T value)
    {
        // TODO: Make code work with enumerables of arbitrary generic type.
        return o => o is IEnumerable<T> enumerable && enumerable.Contains(value);
    }

    public static Func<object, bool> Empty()
    {
        return o => o is IEnumerable enumerable && enumerable.OfType<object>().Any();
    }

    public static Func<object, bool> EndsWith(string suffix, StringComparison comparisonType = StringComparison.InvariantCulture)
    {
        return o => o is string str && str.EndsWith(suffix, comparisonType);
    }

    public static Func<object, bool> Equal(object value)
    {
        return o => o is IComparable comparable && comparable.CompareTo(value) == 0;
    }

    public static Func<object, bool> False()
    {
        return o => false;
    }

    public static Func<object, bool> Not(Func<object, bool> predicate)
    {
        return o => !predicate(o);
    }

    public static Func<T, bool> OrElse<T>(params Func<T, bool>[] predicates)
    {
        return t => predicates.Any(predicate => predicate(t));
    }

    public static Func<object, bool> OrElse(Func<object, bool> left, Func<object, bool> right)
    {
        return o => left(o) || right(o);
    }

    public static Func<object, bool> LessThan(object value)
    {
        return o => o is IComparable comparable && comparable.CompareTo(value) < 0;
    }

    public static Func<object, bool> LessThanOrEqual(object value)
    {
        return o => o is IComparable comparable && comparable.CompareTo(value) <= 0;
    }

    public static Func<object, bool> GreaterThan(object value)
    {
        return o => o is IComparable comparable && comparable.CompareTo(value) > 0;
    }

    public static Func<object, bool> GreaterThanOrEqual(object value)
    {
        return o => o is IComparable comparable && comparable.CompareTo(value) >= 0;
    }

    public static Func<object, bool> StartsWith(string prefix, StringComparison comparisonType = StringComparison.InvariantCulture)
    {
        return o => o is string str && str.StartsWith(prefix, comparisonType);
    }

    public static Func<object, bool> True() => o => true;
}


