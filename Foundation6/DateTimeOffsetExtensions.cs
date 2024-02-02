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

using System.Globalization;

public static class DateTimeOffsetExtensions
{
    public static readonly DateTimeOffset Empty;

    public static DateTimeOffset EndOfDay(this DateTimeOffset dateTime)
    {
        return new DateTimeOffset(
                    dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, dateTime.Offset)
                    .AddDays(1);
    }

    public static DateTimeOffset EndOfHour(this DateTimeOffset dateTime)
    {
        return new DateTimeOffset(
                    dateTime.Year,
                    dateTime.Month,
                    dateTime.Day,
                    dateTime.Hour,
                    0,
                    0,
                    dateTime.Offset)
            .AddHours(1);
    }

    public static DateTimeOffset EndOfMinute(this DateTimeOffset dateTime)
    {
        return new DateTimeOffset(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            dateTime.Hour,
            dateTime.Minute,
            0,
            dateTime.Offset)
            .AddMinutes(1);
    }

    public static DateTimeOffset EndOfMonth(this DateTimeOffset date)
    {
        return new DateTimeOffset(date.Year, date.Month, LastDayOfMonth(date), 0, 0, 0, date.Offset);
    }

    public static DateTimeOffset EndOfWeek(this DateTimeOffset date)
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        return EndOfWeek(date, culture);
    }

    public static DateTimeOffset EndOfWeek(this DateTimeOffset date, DayOfWeek start)
    {
        if (!DayOfWeekHelper.IsValidFirstDayOfWeek(start))
            throw new ArgumentOutOfRangeException(nameof(start));

        var days = 7 + (int)start - (int)date.DayOfWeek;
        return date.AddDays((double)days).Date;
    }

    public static DateTimeOffset EndOfWeek(this DateTimeOffset date, CultureInfo culture)
    {
        if (null == culture) throw new ArgumentOutOfRangeException(nameof(culture));

        return EndOfWeek(date, culture.DateTimeFormat.FirstDayOfWeek);
    }

    public static DateTimeOffset EndOfYear(this DateTimeOffset dateTime)
    {
        return new DateTimeOffset(dateTime.Year, 12, 31, 0, 0, 0, dateTime.Offset);
    }

    public static DateTime FirstDayOfWeek(this DateTimeOffset dateTime)
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        return FirstDayOfWeek(dateTime, culture);
    }

    public static DateTime FirstDayOfWeek(this DateTimeOffset dateTime, DayOfWeek start)
    {
        if (!DayOfWeekHelper.IsValidFirstDayOfWeek(start))
            throw new ArgumentOutOfRangeException(nameof(start));

        return dateTime.AddDays(-(dateTime.DayOfWeek - start)).Date;
    }

    /// <summary>
    /// Returns the first day of the week as DateTime.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="start">Can be DayOfWeek.Sunday or DayOfWeek.Monday.</param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static DateTime FirstDayOfWeek(this DateTimeOffset dateTime, CultureInfo culture)
    {
        return FirstDayOfWeek(dateTime, culture.DateTimeFormat.FirstDayOfWeek);
    }

    public static bool IsEmpty(this DateTimeOffset dt)
    {
        return Empty.Equals(dt);
    }

    /// <summary>
    /// returns the last day of the month.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static int LastDayOfMonth(this DateTimeOffset date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }
}

