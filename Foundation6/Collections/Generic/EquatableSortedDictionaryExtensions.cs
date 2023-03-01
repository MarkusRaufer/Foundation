namespace Foundation.Collections.Generic;

public static class EquatableSortedDictionaryExtensions
{
    public static EquatableSortedDictionary<TKey, TValue> ThrowIfNull<TKey, TValue>(this EquatableSortedDictionary<TKey, TValue>? dictionary)
        where TKey : notnull
    {
        return dictionary ?? throw new ArgumentNullException(nameof(dictionary));
    }
}
