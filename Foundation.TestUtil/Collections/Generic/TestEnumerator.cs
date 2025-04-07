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
﻿namespace Foundation.TestUtil.Collections.Generic;

using Foundation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public class TestEnumerator<T> : IEnumerator<T>
{
    private bool _disposing;
    private readonly IEnumerator<T> _enumerator;

    public TestEnumerator(IEnumerator<T> enumerator)
    {
        _enumerator = enumerator.ThrowIfNull();
    }

    ~TestEnumerator()
    {
        Dispose(false);
    }

    public T Current
    {
        get 
        { 
            var item = _enumerator.Current;
            OnCurrent.Publish(item);
            return item;
        }
    }

#if NET6_0_OR_GREATER
    [MaybeNull]
#endif
#pragma warning disable CS8603
    object IEnumerator.Current
    {
        get
        {
            var item = _enumerator.Current;

            if (item is T t) OnCurrent.Publish(t);
            else OnCurrent.Publish(item);
            
            return item;
        }
    }
#pragma warning restore CS8603

    public void Dispose() => Dispose(true);

    private void Dispose(bool disposing)
    {
        if (_disposing) return;
        _disposing = disposing;

        OnCurrent.Dispose();
        OnMoveNext.Dispose();
        OnReset.Dispose();

        _enumerator.Dispose();
        GC.SuppressFinalize(this);
    }

    public bool MoveNext()
    {
        var hasNext = _enumerator.MoveNext();
        OnMoveNext.Publish(hasNext);
        return hasNext;
    }

    public Event<Action<T>> OnCurrent { get; } = new Event<Action<T>>();

    public Event<Action<bool>> OnMoveNext { get; } = new Event<Action<bool>>();

    public Event<Action> OnReset { get; } = new Event<Action>();

    public void Reset()
    {
        _enumerator.Reset();
        OnReset.Publish();
    }
}

