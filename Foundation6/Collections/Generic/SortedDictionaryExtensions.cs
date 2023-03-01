namespace Foundation.Collections.Generic;

public static class SortedDictionaryExtensions
{
    public static SortedDictionary<TKey, TValue> ThrowIfNull<TKey, TValue>(this SortedDictionary<TKey, TValue>? dictionary)
        where TKey : notnull
    {
        return dictionary ?? throw new ArgumentNullException(nameof(dictionary));
    }

    public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        where TKey : notnull
    {
        var dictionary = new SortedDictionary<TKey, TValue>();
        keyValues.ForEach(kvp => dictionary.Add(kvp.Key, kvp.Value));

        return dictionary;
    }
}
