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

public static class InvasiveVerification
{
    public static InvasiveVerification<T> New<T>(params Func<T, bool>[] predicates) => new(predicates);
}

/// <summary>
/// a list of predicates that are checked each only one times.
/// </summary>
/// <typeparam name="T"></typeparam>
public class InvasiveVerification<T>
{
    private readonly IList<Func<T, bool>> _predicates;

    public InvasiveVerification(IEnumerable<Func<T, bool>> predicates)
    {
        _predicates = predicates.ToList();
    }

    /// <summary>
    /// If a predicate matches, it is removed from the list. If multiple predicates are matching on one item all of them are removed from the list.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>return a TriState. IsNone means, there are no more predicates in the list. If it does not match false is returned.</returns>
    public TriState Verify(T item)
    {
        for (var i = 0; i < _predicates.Count; i++)
        {
            var predicate = _predicates[i];
            if (null == predicate)
            {
                _predicates.RemoveAt(i);
                continue;
            }

            if (predicate(item))
            {
                _predicates.RemoveAt(i);

                return new TriState(true);
            }
        }

        return 0 == _predicates.Count ? new TriState() : new TriState(false);
    }

    public int PredicateCount => _predicates.Count;

    public IEnumerable<Func<T, bool>> Predicates => _predicates;
}

