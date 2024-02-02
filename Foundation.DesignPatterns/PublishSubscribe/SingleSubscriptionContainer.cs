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
﻿using Foundation.Collections.Generic;

namespace Foundation.DesignPatterns.PublishSubscribe;

public class SingleSubscriptionContainer<TSubject, TDelegate> 
    : ISingleSubscriptionContainer<TSubject, TDelegate>
    , IAllSubscriptions
    , ISubscriptionProvider<TSubject, TDelegate>
    , INotifySubscribe<Action<(TSubject, TDelegate)>>
    where TSubject : notnull
    where TDelegate : Delegate
{
    private readonly MultiValueMap<TSubject, TDelegate> _subscriptions;

    public SingleSubscriptionContainer()
    {
        _subscriptions = new MultiValueMap<TSubject, TDelegate>();
        OnSubscribe = new Event<Action<(TSubject, TDelegate)>>();
    }

    public Event<Action<(TSubject, TDelegate)>> OnSubscribe { get; set; }

    public IEnumerable<TDelegate> GetSubscriptions(TSubject subject) => _subscriptions.GetValues(new[] { subject })
                                                                                      .ToArray();

    public IDisposable Subscribe(TSubject subject, TDelegate @delegate)
    {
        if (_subscriptions.Contains(subject, @delegate))
            return new Disposable(() => _subscriptions.Remove(subject, @delegate));

        _subscriptions.Add(subject, @delegate);

        OnSubscribe?.Publish((subject, @delegate));

        return new Disposable(() => _subscriptions.Remove(subject, @delegate));
    }

    public void Unsubscribe(TSubject subject)
    {
        _subscriptions.Remove(subject);
    }

    public void UnsubscribeAll()
    {
        _subscriptions.Clear();
    }
}
