using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Collections.Generic;

public static class HashSetExtensions
{
    /// <summary>
    /// Throws an exception when hashSet is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="hashSet"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static HashSet<T> ThrowIfEmpty<T>(this HashSet<T> hashSet, [CallerArgumentExpression("hashSet")] string paramName = "")
    => 0 < hashSet.Count
       ? hashSet
       : throw new ArgumentOutOfRangeException($"{paramName} must not be empty");

    /// <summary>
    /// Throws an exception when hashSet is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="hashSet"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static HashSet<T> ThrowIfNull<T>(this HashSet<T> hashSet, [CallerArgumentExpression("hashSet")] string paramName = "")
        => hashSet ?? throw new ArgumentException($"{paramName} must not be empty");

    /// <summary>
    /// Throws an exception when hashSet is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="hashSet"></param>
    /// <param name="paramName"></param>
    /// <returns></returns>
    public static HashSet<T> ThrowIfNullOrEmpty<T>(this HashSet<T> hashSet, [CallerArgumentExpression("hashSet")] string paramName = "")
        => ThrowIfNull(hashSet, paramName).ThrowIfEmpty(paramName);
}
