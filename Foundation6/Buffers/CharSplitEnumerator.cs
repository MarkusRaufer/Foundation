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
using Foundation.Collections.Generic;

namespace Foundation.Buffers;

/// <summary>
/// Splits strings as spans.
/// Must be a ref struct as it contains a ReadOnlySpan<char>
/// </summary>
public ref struct CharSplitEnumerator
{
    private readonly int[] _indices;
    private int _indexSelector = 0;
    private readonly bool _notFoundReturnsNothing;
    private bool _passed;
    private int _rightIndex = 0;
    private ReadOnlySpan<char> _span;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="span"></param>
    /// <param name="notFoundReturnsNothing"></param>
    /// <param name="separators"></param>
    public CharSplitEnumerator(ReadOnlySpan<char> span, bool notFoundReturnsNothing, params char[] separators)
    {
        _span = span;
        
        separators.ThrowIfOutOfRange(() => separators.Length == 0);

        Separators = separators;
        _indices = span.IndicesOfSingleCharacters(separators).ToArray();

        _notFoundReturnsNothing = notFoundReturnsNothing;

        Current = default;
        _passed = false;
    }


    public ReadOnlySpan<char> Current { get; private set; }

    // Needed to be compatible with the foreach operator
    public CharSplitEnumerator GetEnumerator() => this;

    public bool MoveNext()
    {
        if (_span.Length == 0) return false; // span is empty

        if (_indices.Length == 0) // no separator found
        {
            if (_passed || _notFoundReturnsNothing) return false;

            _passed = true;

            Current = _span;
            return true;
        }

        int leftIndex;
        if (_indexSelector >= _indices.Length)
        {
            if (_passed || _rightIndex >= _span.Length) return false;

            _passed = true;
            leftIndex = _rightIndex + 1;
            _rightIndex = _span.Length;
        }
        else
        {
            leftIndex = 0 == _indexSelector ? 0 : _rightIndex + 1;

            _rightIndex = _indices[_indexSelector];
            _indexSelector++;
        }

        Current = _span[leftIndex.._rightIndex];

        return true;
    }

    public char[] Separators { get; }
}
