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
﻿namespace Foundation.Collections.Generic;

public static class RangeExtensions
{
	public static RangeEnumerator GetEnumerator(this System.Range range) => new(range);
}

public ref struct RangeEnumerator
{
	private readonly System.Range _range;
	private int _current;

	public RangeEnumerator(System.Range range)
	{
		_range = range.ThrowIfOutOfRange(() => range.End.IsFromEnd, "range end must be defined");
		_current = range.Start.IsFromEnd ? -1 : range.Start.Value - 1;
    }

	public int Current => _current;

	public bool MoveNext()
	{
        _current++;

        if (_range.End.IsFromEnd) return true;

		if (_current > _range.End.Value) return false;
		
		return true;
	}
}
