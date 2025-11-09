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

namespace Foundation.ComponentModel;

/// <summary>
/// Provides an events list.
/// </summary>
/// <typeparam name="TEvent">Type of event.</typeparam>
public class EventHistory<TEvent> 
    : IEventHistory<TEvent>
    , ICanAddEvent<TEvent>
{
    private readonly List<TEvent> _events = [];

    /// <summary>
    /// Add an event tp the events list.
    /// </summary>
    /// <param name="event">The event that should be added.</param>
    public void AddEvent(TEvent @event) => _events.Add(@event);

    /// <summary>
    /// Removes all events from the events list.
    /// </summary>
    /// <param name="event"></param>
    public void ClearEvents() => _events.Clear();

    /// <summary>
    /// List of events.
    /// </summary>
    public IEnumerable<TEvent> Events => _events;

    /// <summary>
    /// True if an event exists.
    /// </summary>
    public bool HasEvents => _events.Count > 0;

    /// <summary>
    /// Removes an event at a specific index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool RemoveEventAt(int index) => _events.InRange(index) && Void.Returns(true, () => _events.RemoveAt(index));

    /// <summary>
    /// Removes a number of events at a specific index.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public void RemoveEvents(int index, int count)
        => _events.InRange<TEvent, List<TEvent>>(index, count, (List<TEvent> x) => x.RemoveRange(index, count));
}
