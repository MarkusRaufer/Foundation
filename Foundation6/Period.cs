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
public readonly struct Period : IComparable<Period>
{
    public enum Direction
    {
        Forward,
        Backward
    }

    public static readonly Period Empty;

    private readonly int _hashCode;

    public Period(DateTime start, DateTime end)
    {
        if (start.Kind != end.Kind)
            throw new ArgumentException($"start.{nameof(start.Kind)}: {start.Kind} must be same as end.{nameof(end.Kind)}: {end.Kind}");

        Start = start;
        End = end;
#if NETSTANDARD2_0
        _hashCode = Foundation.HashCode.FromObject(start, end);
#else
        _hashCode = System.HashCode.Combine(start, end);
#endif
    }

    public static bool operator ==(Period lhs, Period rhs) => lhs.Equals(rhs);

    public static bool operator !=(Period lhs, Period rhs) => !lhs.Equals(rhs);

    public static Period operator +(Period period, TimeSpan span) => New(period.Start, period.End + span);

    public static Period operator -(Period period, TimeSpan span)
    {
        if (period.Duration < span) throw new ArithmeticException(nameof(span));
        return New(period.Start, period.End - span);
    }

    public static bool operator <(Period lhs, Period rhs)
    {
        if (lhs.IsEmpty) return true;
        if (rhs.IsEmpty) return false;

        if (lhs.Start < rhs.Start) return true;
        if (lhs.Start > rhs.Start) return false;
        return lhs.End < rhs.End;
    }

    public static bool operator <=(Period lhs, Period rhs)
    {
        return (lhs < rhs) || lhs.Equals(rhs);
    }

    public static bool operator >(Period lhs, Period rhs)
    {
        return !(lhs <= rhs);
    }

    public static bool operator >=(Period lhs, Period rhs)
    {
        return !(lhs < rhs);
    }

    public int CompareTo(Period other)
    {
        if(this == other) return 0;
        return this < other ? -1 : 1;
    }

    public TimeSpan Duration => End - Start;

    public DateTime End { get; }

    public override bool Equals(object? obj) => obj is Period other && Equals(other);

    public bool Equals(Period other)
    {
        if (IsEmpty) return other.IsEmpty;
        return !other.IsEmpty && Start == other.Start && End == other.End;
    }

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => Start.IsEmpty() && End.IsEmpty();

    /// <summary>
    /// Creates a new Period-Object.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static Period New(DateTime start, DateTime end)
    {
        if (start > end)
            throw new ArgumentException("end is before start");

        return new Period(start, end);
    }

    public static Period New(DateOnly start, DateOnly end, DateTimeKind kind = DateTimeKind.Utc)
    {
        if (start > end)
            throw new ArgumentException("end is before start");

        return new Period(start.ToDateTime(kind), end.ToDateTime(kind));
    }

    public static Period New(TimeOnly start, TimeOnly end, DateTimeKind kind = DateTimeKind.Utc)
    {
        if (start > end)
            throw new ArgumentException("end is before start");

        return new Period(start.ToDateTime(kind), end.ToDateTime(kind));
    }

    /// <summary>
    /// Creates a new Period-Object.
    /// </summary>
    /// <param name="dateTime">The offset of the period.</param>
    /// <param name="duration">The duration of the period</param>
    /// <param name="direction">The direction which is used for calculation.
    /// If it is Forward then Start is dateTime and End is dateTime + duration.
    /// If it is Backward then End is dateTime and Start is dateTime - duration.</param>
    /// <returns></returns>
    public static Period New(DateTime dateTime, TimeSpan duration, Direction direction = Direction.Forward)
    {
        if (Direction.Forward == direction)
            return new Period(dateTime, dateTime + duration);

        return new Period(dateTime - duration, dateTime);
    }

    public DateTime Start { get; }

    public override string ToString() => $"{nameof(Start)}={Start:o}, {nameof(End)}={End:o}, {nameof(Duration)}={Duration}";
}

