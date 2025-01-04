#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Foundation.Collections;

public interface IReadOnlyHashMap<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
{
    bool ContainsValue(TValue value);

#if NET5_0_OR_GREATER
    public bool TryGetKey(TValue value, [MaybeNullWhen(false)] out TKey? key);
#endif
}
