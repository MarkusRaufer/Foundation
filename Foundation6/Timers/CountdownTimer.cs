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
﻿using System.Timers;
using Timer = System.Timers.Timer;

namespace Foundation.Timers;

/// <summary>
/// Counts a duration of time down.
/// </summary>
public sealed class CountdownTimer : IDisposable
{
    private readonly Timer _timer;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="duration">The duration of the count down (e.g. 10 seconds).</param>
    /// <param name="interval">The interval of the count down (e.g. 1 second).</param>
    public CountdownTimer(TimeSpan duration, TimeSpan interval)
    {
        Duration = duration;
        RemainingDuration = duration;
        Interval = interval;

        _timer = new Timer(interval.TotalMilliseconds);
        _timer.Elapsed += OnTimerIntervalElapsed;
    }

    public CountdownTimer(Timer timer, TimeSpan duration)
    {
        _timer = timer.ThrowIfNull();
        Duration = duration;
        RemainingDuration = duration;

        _timer.Elapsed += OnTimerIntervalElapsed;

        Interval = TimeSpan.FromMilliseconds(_timer.Interval);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Stop();

        _timer.Dispose();
        IntervalElapsed.Dispose();
        ZeroReached.Dispose();
    }

    /// <summary>
    /// The duration of the count down.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// Event which is called on each elapsed interval.
    /// </summary>
    public Event<Action<TimeSpan>> IntervalElapsed { get; } = new();

    /// <summary>
    /// The remaining duration which will be decreased on every elapsed interval.
    /// </summary>
    public TimeSpan RemainingDuration { get; private set; }

    /// <summary>
    /// The interval of the count down.
    /// </summary>
    public TimeSpan Interval { get; }

    private void OnTimerIntervalElapsed(object? sender, ElapsedEventArgs e)
    {
        RemainingDuration = RemainingDuration.Subtract(Interval);

        var zeroReached = RemainingDuration.Ticks <= 0;

        if (zeroReached)
        {
            Stop();
            RemainingDuration = TimeSpan.Zero;
        }

        IntervalElapsed.Publish(RemainingDuration);
        if (zeroReached) ZeroReached.Publish();
    }

    /// <summary>
    /// Starts the timer in another thread.
    /// </summary>
    public void Start()
    {
        _timer.AutoReset = true;
        _timer.Enabled = true;
        _timer.Start();
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void Stop()
    {
        _timer.Stop();
        _timer.AutoReset = false;
        _timer.Enabled = false;
    }

    /// <summary>
    /// Event is called once when the count down reached zero.
    /// </summary>
    public Event<Action> ZeroReached { get; } = new();
}
