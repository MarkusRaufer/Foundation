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

/// <summary>
/// TODO: check if obsolete
/// </summary>
public class HashCodeSelector
{
    public static HashCodeSelector<T> Create<T, TSelector>(T? obj, params Func<T, TSelector>[] selectors)
    {
        return new HashCodeSelector<T, TSelector>(obj, selectors);
    }
}

public abstract class HashCodeSelector<T>
{
    public HashCodeSelector(T? obj)
    {
        Object = obj.ThrowIfNull();
    }

    public abstract IEnumerable<int> GetHashCodesFromSelectors();

    public T Object { get; }
}

public class HashCodeSelector<T, TSelector> : HashCodeSelector<T>
{
    public HashCodeSelector(T? obj, params Func<T, TSelector>[] selectors) : base(obj)
    {
        if (0 == selectors.Length)
            throw new ArgumentOutOfRangeException(nameof(selectors), "selectors must have at least one selector");

        Selectors = selectors;
    }

    public override IEnumerable<int> GetHashCodesFromSelectors()
    {
        foreach (var selector in Selectors)
        {
            var selected = selector(Object);

            if (null != selected) yield return selected.GetHashCode();
        }
    }

    public Func<T, TSelector>[] Selectors { get; private set; }
}

