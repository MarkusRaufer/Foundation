using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic;

public static class CollectionExtensions
{
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
