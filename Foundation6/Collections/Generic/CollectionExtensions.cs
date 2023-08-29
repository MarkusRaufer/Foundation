using Foundation.Linq.Expressions;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic;

public static class CollectionExtensions
{
    public static IEnumerable<T> FindAll<T>(this ICollection<T> collection, LambdaExpression lambda)
    {
        if (!lambda.ThrowIfNull().IsPredicate())
            throw new ArgumentOutOfRangeException(nameof(lambda), $"is not a predicate");

        if (1 != lambda.Parameters.Count)
            throw new ArgumentOutOfRangeException(nameof(lambda), $"exact one parameter expected");

        if (lambda.Parameters.First().Type != typeof(T))
            throw new ArgumentOutOfRangeException(nameof(lambda), $"wrong parameter type");

        var predicate = (Func<T, bool>)lambda.Compile();

        return predicate is null ? Enumerable.Empty<T>() : collection.Where(predicate);
    }

    /// <summary>
    /// Throws an exception when collection is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ICollection<T> ThrowIfEmpty<T>(this ICollection<T> collection, [CallerArgumentExpression("collection")] string paramName = "")
    => 0 < collection.Count
       ? collection
       : throw new ArgumentOutOfRangeException($"{paramName} must not be empty");

    /// <summary>
    /// Throws an exception when collection is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ICollection<T> ThrowIfNull<T>(this ICollection<T> collection, [CallerArgumentExpression("collection")] string paramName = "")
        => collection ?? throw new ArgumentException($"{paramName} must not be empty");

    /// <summary>
    /// Throws an exception when collection is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    public static ICollection<T> ThrowIfNullOrEmpty<T>(this ICollection<T> collection, [CallerArgumentExpression("collection")] string paramName = "")
        => ThrowIfNull(collection, paramName).ThrowIfEmpty(paramName);
}
