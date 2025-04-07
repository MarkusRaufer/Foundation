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
/// Subscriber can subscribe with a delegate by calling <see cref="Event.Subscribe(TDelegate)"/>. By calling <see cref="Event.Publish(object?[]?)"/> all subscribed delegates will be called.
/// </summary>
/// <typeparam name="TDelegate"></typeparam>
public sealed class Event<TDelegate> : IDisposable
    where TDelegate : Delegate
{
    private readonly Lazy<List<WeakReference<TDelegate>>> _subscriptions = new(() => []);

    private bool _disposing;

    ~Event()
    {
        Dispose();
    }

    /// <summary>
    /// When called all subscribers will be unsubscribed.
    /// </summary>
    public void Dispose()
    {
        if (!_disposing)
        {
            _disposing = true;
            UnsubscribeAll();
        }
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// True if at least one subscription exists.
    /// </summary>
    public bool HasSubscriptions => SubscriptionCount > 0;

    /// <summary>
    /// Is called on every Subscribe call.
    /// </summary>
    public Action<TDelegate>? OnSubscribe { get; set; }

    /// <summary>
    /// Calls the delegates of the subscribers. 
    /// </summary>
    /// <param name="args">Each element of args corresponds to a parameter of the weakReference. If you want to use a list of values as a single parameter cast it to object.</param>
    public void Publish(params object?[]? args)
    {
        foreach (var weakRef in _subscriptions.Value.ToArray())
        {
            if (!weakRef.TryGetTarget(out var target))
            {
                _subscriptions.Value.Remove(weakRef);
                continue;
            }

            target?.DynamicInvoke(args);
        }
    }

    /// <summary>
    /// Subscribes a delegate to the event.
    /// </summary>
    /// <param name="delegate"></param>
    /// <returns></returns>
    public IDisposable Subscribe(TDelegate @delegate)
    {
        @delegate.ThrowIfNull();

        var weakRef = new WeakReference<TDelegate>(@delegate);

        return Subscribe(weakRef);
    }

    /// <summary>
    /// Subscribes a <see cref="Subscribe(WeakReference{TDelegate})"/> to the event.
    /// </summary>
    /// <param name="weakReference"></param>
    /// <returns></returns>
    public IDisposable Subscribe(WeakReference<TDelegate> weakReference)
    {
        weakReference.ThrowIfNull();

        var disposable = new Disposable(() => Unsubscribe(weakReference));

        _subscriptions.Value.Add(weakReference);

        if (weakReference.TryGetTarget(out var target)) OnSubscribe?.Invoke(target);

        return disposable;
    }

    /// <summary>
    /// Number of subscriptions.
    /// </summary>
    public int SubscriptionCount => _subscriptions.Value.Count;

    /// <summary>
    /// Unsubscribes a subscription.
    /// </summary>
    /// <param name="weakRef"></param>
    /// <returns></returns>
    private bool Unsubscribe(WeakReference<TDelegate> weakRef)  => _subscriptions.Value.Remove(weakRef);

    /// <summary>
    /// Removes all subscriptions.
    /// </summary>
    public void UnsubscribeAll() => _subscriptions.Value.Clear();
}
