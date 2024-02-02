// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
﻿using System.Collections;

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
            chainElement = new HashChainElement<T, THash>(item, GetHash, Option.None<THash>());
        }
            
        else
        {
            var prevElement = _elements[_elements.Count - 1];

            chainElement = new HashChainElement<T, THash>(item, GetHash, Option.Some(prevElement.Hash));
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
