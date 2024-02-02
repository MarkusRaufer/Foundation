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
/// This event automatically removes all subscribers on Dispose().
/// </summary>
/// <typeparam name="TDelegate"></typeparam>
public sealed class Event<TDelegate> : IDisposable
    where TDelegate : Delegate
{
    private readonly Lazy<ICollection<TDelegate>> _subscribtions
        = new(() => new List<TDelegate>());

    private bool _disposing;

    ~Event()
    {
        Dispose();
    }

    public bool Contains(TDelegate @delegate) => _subscribtions.Value.Contains(@delegate);

    public void Dispose()
    {
        if (!_disposing)
        {
            _disposing = true;
            UnsubscribeAll();
        }
        GC.SuppressFinalize(this);
    }

    public Action<TDelegate>? OnSubscribe { get; set; }
    public Action<TDelegate>? OnUnsubscribe { get; set; }

    /// <summary>
    /// Calls the delegates of the the subscribers. 
    /// </summary>
    /// <param name="args">Each element of args corresponds to a parameter of the delegate. If you want to use a list of values as a single parameter cast it to object.</param>
    public void Publish(params object?[]? args)
    {
        foreach (var subscribtion in _subscribtions.Value.ToArray())
        {
            subscribtion?.DynamicInvoke(args);
        }
    }

    public IDisposable Subscribe(TDelegate @delegate)
    {
        var disposable = new Disposable(() => Unsubscribe(@delegate));

        if (Contains(@delegate)) return disposable;

        _subscribtions.Value.Add(@delegate);

        OnSubscribe?.Invoke(@delegate);

        return disposable;
    }

    public int SubscribtionCount => _subscribtions.Value.Count;

    public bool Unsubscribe(TDelegate @delegate)
    {
        var removed = _subscribtions.Value.Remove(@delegate);

        OnUnsubscribe?.Invoke(@delegate);

        return removed;
    }

    public void UnsubscribeAll() => _subscribtions.Value.Clear();
}
