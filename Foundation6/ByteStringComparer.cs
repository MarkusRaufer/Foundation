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
﻿namespace Foundation;

public static class ByteStringComparer
{
    private class DefaultByteStringComparer : IComparer<ByteString>
    {
        /// <summary>
        /// Returns the ordinal distance.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(ByteString? x, ByteString? y)
        {
            if (ReferenceEquals(x, y)) return 0;

            if (x is null) return y is null ? 0 : -1;
            if (y is null) return 1;

            var lhs = x.AsSpan();
            var rhs = y.AsSpan();
            
            return lhs.SequenceCompareTo(rhs);
        }
    }

    private class NullByteStringComparer : IComparer<ByteString>
    {
        public int Compare(ByteString? x, ByteString? y)
        {
            if (ReferenceEquals(x, y)) return 0;

            if (x is null) return y is null ? 0 : 1;
            if (y is null) return -1;

            var lhs = x.AsSpan();
            var rhs = y.AsSpan();

            return lhs.SequenceCompareTo(rhs);
        }
    }

    /// <summary>
    /// null is considered. A ByteString is null means smaller.
    /// </summary>
    public static IComparer<ByteString> Default => new DefaultByteStringComparer();

    /// <summary>
    /// null is considered. A ByteString is null means greater. In a list including null values the null values appear at the end. 
    /// Means you can stop iterating the list on the first null ByteString.
    /// </summary>
    public static IComparer<ByteString> NullIsGreater => new NullByteStringComparer();
}
