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
﻿namespace Foundation.Collections.Generic
{
    public static class HashChainFactory
    {
        public static IEnumerable<HashChainElement<T, THash>> Create<T, THash>(IEnumerable<T> elements, Func<T, THash> getHash)
            where T : notnull
            where THash : notnull
        {
            var prevHash = Option.None<THash>();
            foreach (var element in elements)
            {
                var hash = getHash(element);
                if (prevHash.IsNone)
                {
                    var firstElem = new HashChainElement<T, THash>(element, getHash, prevHash);
                    prevHash = Option.Some(firstElem.Hash);
                    yield return firstElem;
                    continue;
                }

                var elem = new HashChainElement<T, THash>(element, getHash, prevHash);
                yield return elem;

                prevHash = Option.Some(elem.Hash);
            }
        }
    }
}
