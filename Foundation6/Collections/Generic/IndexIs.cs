namespace Foundation.Collections.Generic;

public static class IndexIs
{
    /// <summary>
    /// Returns true if the index is in range.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="array">The array where the index is used</param>
    /// <param name="index">The index which should be checked.</param>
    /// <returns>True if the index is in range.</returns>
    public static bool InRange<T>(this T[] array, int index) => index is not < 0 && index < array.Length;

    /// <summary>
    /// Returns true if the index is in range.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="array">The array where the index is used</param>
    /// <param name="index">The index which should be checked.</param>
    /// <param name="count">The number of elements which is used for range check.</param>
    /// <returns>True if the index is in range.</returns>
    public static bool InRange<T>(this T[] array, int index, int count)
    {
        if (!InRange(array, index)) return false;

        return (index + count) < array.Length;
    }

    /// <summary>
    /// Returns true if the index is in range.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="array">The array where the index is used</param>
    /// <param name="index">The index which should be checked.</param>
    /// <param name="count">The number of elements which is used for range check.</param>
    /// <param name="action">Action will be executed if index is in range.</param>
    /// <returns>True if the index is in range.</returns>
    public static bool InRange<T>(this T[] array, int index, int count, Action<T[]> action)
    {
        if (!InRange(array, index, count)) return false;
        action(array);
        return true;
    }

    /// <summary>
    /// Returns true if the index is in range.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="list">The list where the index is used</param>
    /// <param name="index">The index which should be checked.</param>
    /// <returns>True if the index is in range.</returns>
    public static bool InRange<T>(this IList<T> list, int index) => index is not < 0 && index < list.Count;

    /// <summary>
    /// Returns true if the index is in range.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="list">The list where the index is used</param>
    /// <param name="index">The index which should be checked.</param>
    /// <param name="count">The number of elements which is used for range check.</param>
    public static bool InRange<T>(this IList<T> list, int index, int count)
    {
        if (!InRange(list, index)) return false;

        return (index + count) > list.Count;
    }

    /// <summary>
    /// Returns true if the index is in range.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="list">The list where the index is used.</param>
    /// <param name="index">The index which should be checked.</param>
    /// <param name="count">The number of elements which is used for range check.</param>
    /// <param name="action">Action will be executed if index is in range.</param>
    /// <returns>True if the index is in range.</returns>
    public static bool InRange<T>(this IList<T> list, int index, int count, Action<IList<T>> action)
    {
        if (!InRange(list, index, count)) return false;
        action(list);
        return true;
    }

    /// <summary>
    /// Returns true if the index is in range.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <typeparam name="TList">The type of the list.</typeparam>
    /// <param name="list">The list where the index is used.</param>
    /// <param name="index">The index which should be checked.</param>
    /// <param name="count">The number of elements which is used for range check.</param>
    /// <param name="action">Action will be executed if index is in range.</param>
    /// <returns>True if the index is in range.</returns>
    public static bool InRange<T, TList>(this TList list, int index, int count, Action<TList> action)
        where TList : IList<T>
    {
        if (!InRange(list, index, count)) return false;
        action(list);
        return true;
    }
}
