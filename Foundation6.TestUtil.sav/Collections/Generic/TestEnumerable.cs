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

using System.Collections;

public class TestEnumerable<T> 
    : IEnumerable<T>
    , IDisposable
{
    private bool _disposing;
    private readonly IEnumerable<T> _items;

    public TestEnumerable(IEnumerable<T> items)
    {
        _items = items.ThrowIfNull();
    }

    ~TestEnumerable()
    {
        Dispose(false);
    }

    public void Dispose() => Dispose(true);

    private void Dispose(bool disposing)
    {
        if (_disposing) return;
        _disposing = disposing;

        OnCurrent.Dispose();
        OnMoveNext.Dispose();
        OnReset.Dispose();

        GC.SuppressFinalize(this);
    }

    public Event<Action<T>> OnCurrent { get; } = new Event<Action<T>>();

    private void _OnCurrent(T item)
    {
        OnCurrent.Publish(item);
    }

    public Event<Action<bool>> OnMoveNext { get; } = new Event<Action<bool>>();

    private void _OnMoveNext(bool hasNext)
    {
        OnMoveNext.Publish(hasNext);
    }

    public Event<Action> OnReset { get; } = new Event<Action>();

    private void _OnReset()
    {
        OnReset.Publish();
    }

    public IEnumerator<T> GetEnumerator()
    {
        var enumerator = new TestEnumerator<T>(_items.GetEnumerator());

        enumerator.OnCurrent.Subscribe(_OnCurrent);
        enumerator.OnMoveNext.Subscribe(_OnMoveNext);
        enumerator.OnReset.Subscribe(_OnReset);

        return enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        var enumerator = new TestEnumerator<T>(_items.GetEnumerator());

        enumerator.OnCurrent.Subscribe(_OnCurrent);
        enumerator.OnMoveNext.Subscribe(_OnMoveNext);
        enumerator.OnReset.Subscribe(_OnReset);

        return enumerator;
    }
}

