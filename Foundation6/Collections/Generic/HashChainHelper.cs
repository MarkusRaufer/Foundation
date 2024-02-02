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
    public static class HashChainHelper
    {
        public static bool IsConsistent<T>(IEnumerable<T> elements, Func<T, Option<int>> getPrevElementHash)
            where T : notnull
        {
            return IsConsistent(elements, elem => elem.GetHashCode(), getPrevElementHash);
        }

        public static bool IsConsistent<T>(
            IEnumerable<T> elements, 
            Func<T, int> getHash, 
            Func<T, Option<int>> getPrevElementHash)
            where T : notnull
        {
            return IsConsistent<T, int>(elements, getHash, getPrevElementHash);
        }

        public static bool IsConsistent<T, THash>(
            IEnumerable<T> elems, 
            Func<T, THash> getElementHash, 
            Func<T, Option<THash>> getPrevElementHash)
            where T : notnull
            where THash : notnull
        {
            var prevHash = Option.None<THash>();

            foreach (var elem in elems)
            {
                if (elem is null) return false;

                var currentElemHash = getElementHash(elem);
                if(currentElemHash is null) return false;

                var currentElemPrevHash = getPrevElementHash(elem);

                if (prevHash.IsNone)
                {
                    if(currentElemPrevHash.IsSome) return false;

                    prevHash = Option.Some(currentElemHash);
                    continue;
                }

                if (currentElemPrevHash.IsNone) return false;

                if (currentElemPrevHash != prevHash) return false;

                prevHash = currentElemHash;
            }

            return true;
        }
    }
}
