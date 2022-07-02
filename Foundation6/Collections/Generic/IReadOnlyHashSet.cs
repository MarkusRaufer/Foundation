using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

public interface IReadOnlyHashSet<T>
{
    bool Contains(T item);
    void CopyTo(T[] array, int arrayIndex, int count);

    void IntersectWith(IEnumerable<T> other);
    bool IsProperSubsetOf(IEnumerable<T> other);
    bool IsProperSupersetOf(IEnumerable<T> other);
    bool IsSubsetOf(IEnumerable<T> other);
    bool IsSupersetOf(IEnumerable<T> other);
    bool Overlaps(IEnumerable<T> other);
    bool SetEquals(IEnumerable<T> other);
    bool TryGetValue(T equalValue, [MaybeNullWhen(false)] out T actualValue);
}
