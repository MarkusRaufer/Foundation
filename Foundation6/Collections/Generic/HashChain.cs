using System.Collections;

namespace Foundation.Collections.Generic;

public class HashChain<T> : IHashChain<T>
    where T : notnull
{
    private readonly IList<HashChainElement<T>> _elements = new List<HashChainElement<T>>();

    public int Count => _elements.Count;

    public void Add(T item)
    {
        HashChainElement<T> chainElement;
        if (0 == _elements.Count)
            chainElement = new HashChainElement<T>(item);
        else
        {
            var prevElement = _elements[_elements.Count - 1];

            chainElement = new HashChainElement<T>(item, prevElement.GetHashCode());
        }

        _elements.Add(chainElement);
    }

    public void Clear() => _elements.Clear();

    public bool Contains(T item) => _elements.Any(elem => elem.Payload.Equals(item));

    public IEnumerator<T> GetEnumerator() => _elements.Select(x => x.Payload).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _elements.Select(x => x.Payload).GetEnumerator();

    public bool IsConsistant => HashChainHelper.IsConsistant(_elements);

}
