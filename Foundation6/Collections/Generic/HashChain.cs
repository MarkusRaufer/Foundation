using System.Collections;

namespace Foundation.Collections.Generic;

public class HashChain<T, THash> : IHashChain<T>
    where T : notnull
    where THash : notnull
{
    private readonly IList<HashChainElement<T, THash>> _elements = new List<HashChainElement<T, THash>>();

    public HashChain(Func<T, THash> getHash)
    {
        GetHash = getHash.ThrowIfNull();
    }

    public void Add(T item)
    {
        HashChainElement<T, THash> chainElement;
        if (0 == _elements.Count)
        {
            chainElement = new HashChainElement<T, THash>(item, GetHash, Opt.None<THash>());
        }
            
        else
        {
            var prevElement = _elements[_elements.Count - 1];

            chainElement = new HashChainElement<T, THash>(item, GetHash, Opt.Some(prevElement.Hash));
        }

        _elements.Add(chainElement);
    }

    public void Clear() => _elements.Clear();

    public bool Contains(T item) => _elements.Any(elem => elem.Payload.Equals(item));

    public int Count => _elements.Count;

    public IEnumerator<T> GetEnumerator() => _elements.Select(x => x.Payload).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _elements.Select(x => x.Payload).GetEnumerator();

    public Func<T, THash> GetHash { get; }

    public bool IsConsistent => HashChainHelper.IsConsistent(_elements,
                                                             x => x.Hash, 
                                                             x => x.PreviousElementHash);

}
