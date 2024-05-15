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

public static class TimedRc
{
    /// <summary>
    /// Creates a timed reference counter. The timestamp is <see cref="DateTimeKind.Utc"./>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rc"></param>
    /// <returns></returns>
    public static TimedRc<T> New<T>(Rc<T> rc) where T : notnull, IEquatable<T>
    {
        return new TimedRc<T>(rc);
    }

    /// <summary>
    /// Creates a timed reference counter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rc"></param>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static TimedRc<T> New<T>(Rc<T> rc, DateTime timestamp) where T : notnull, IEquatable<T>
    {
        return new TimedRc<T>(rc, timestamp);
    }
}

/// <summary>
/// A timed reference counter.
/// </summary>
/// <typeparam name="T"></typeparam>
public class TimedRc<T>
    : IEquatable<TimedRc<T>>
    , IComparable<TimedRc<T>>
    where T : notnull
{
    private readonly Rc<T> _rc;

    public TimedRc(Rc<T> rc, DateTimeKind kind = DateTimeKind.Utc)
    {
        _rc = rc.ThrowIfNull();
        Timestamp = DateTimeHelper.Now(kind);
    }

    public TimedRc(Rc<T> rc, DateTime timestamp)
    {
        _rc = rc.ThrowIfNull();
        Timestamp = timestamp;
    }

    public static bool operator ==(TimedRc<T> left, TimedRc<T> right)
    {
        if (left is null) return right is null;

        return left.Equals(right);
    }

    public static bool operator !=(TimedRc<T> left, TimedRc<T> right)
    {
        return !(left == right);
    }

    public int Counter => _rc.Counter;

    public override bool Equals(object? obj) => obj is TimedRc<T> other && Equals(other);

    public bool Equals(TimedRc<T>? other)
    {
        if (other is null) return false;

        if (!_rc.Equals(other._rc)) return false;

        return Timestamp.ValueEquals(other.Timestamp);
    }

    /// <summary>
    /// Timestamp is changed on call.
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        Timestamp = DateTimeHelper.Now(Timestamp.Kind);

        return _rc.Get();
    }

#if NETSTANDARD2_0
    public override int GetHashCode() => Foundation.HashCode.FromObject(_rc, Timestamp);
#else
    public override int GetHashCode() => System.HashCode.Combine(_rc, Timestamp);
#endif

    public int CompareTo(TimedRc<T>? other)
    {
        if (other is null) return 1;

        var timeComparision = Timestamp.CompareTo(other.Timestamp);
        if (0 != timeComparision) return timeComparision;

        return _rc.CompareTo(other._rc);
    }

    public void Reset()
    {
        Timestamp = DateTimeHelper.Now(Timestamp.Kind);
        _rc.Reset();
    }

    public DateTime Timestamp { get; private set; }

    public override string ToString()
    {
        return $"{_rc}, TimeStamp: {Timestamp}";
    }
}

