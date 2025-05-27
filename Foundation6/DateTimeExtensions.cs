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
using System.Runtime.CompilerServices;

public static class DateTimeExtensions
{
    public static readonly DateTime Empty;

    public static int DaysOfYear(this DateTime dateTime)
    {
        //return DateTimeHelper.GetDaysOfYear(dateTime.Year);
        return dateTime.DayOfYear;
    }

    public static DateTime EndOfDay(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, dateTime.Kind).AddDays(1);
    }

    public static DateTime EndOfHour(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, dateTime.Kind).AddHours(1);
    }

    public static DateTime EndOfMinute(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, dateTime.Kind).AddMinutes(1);
    }

    public static DateTime EndOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, LastDayOfMonthAsInt(date), 0, 0, 0, date.Kind);
    }

    public static DateTime EndOfWeek(this DateTime date, bool withinMonth = true)
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        return EndOfWeek(date, culture, withinMonth);
    }

    public static DateTime EndOfWeek(this DateTime date, DayOfWeek start, bool withinMonth = true)
    {
        if (!DayOfWeekHelper.IsValidFirstDayOfWeek(start))
            throw new ArgumentOutOfRangeException(nameof(start));

        if (start.LastDayOfWeek() == date.DayOfWeek) return date;

        var days = 7 + (int)start - (int)date.DayOfWeek - 1;

        if (withinMonth)
        {
            var lastDayOfMonth = date.LastDayOfMonthAsInt();
            if ((date.Day + days) > lastDayOfMonth) days = lastDayOfMonth - date.Day;
        }
        return date.AddDays((double)days).Date;
    }

    public static DateTime EndOfWeek(this DateTime date, CultureInfo culture, bool withinMonth = true)
    {
        if (null == culture) throw new ArgumentOutOfRangeException(nameof(culture));

        return EndOfWeek(date, culture.DateTimeFormat.FirstDayOfWeek, withinMonth);
    }

    public static DateTime EndOfYear(this DateTime dt)
    {
        return new DateTime(dt.Year + 1, 1, 1, 0, 0, 0, dt.Kind);
    }

    public static DateTime FirstDayOfWeek(this DateTime dateTime)
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        return FirstDayOfWeek(dateTime, culture);
    }

    public static DateTime FirstDayOfWeek(this DateTime dateTime, DayOfWeek start)
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
    public static DateTime FirstDayOfWeek(this DateTime dateTime, CultureInfo culture)
    {
        return FirstDayOfWeek(dateTime, culture.DateTimeFormat.FirstDayOfWeek);
    }

    public static bool IsEmpty(this DateTime dt)
    {
        return Empty.Equals(dt);
    }

    /// <summary>
    /// returns the last day of the month.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime LastDayOfMonth(this DateTime date)
    {
        var lastDay = LastDayOfMonthAsInt(date);
        return new DateTime(date.Year, date.Month, lastDay);
    }

    public static int LastDayOfMonthAsInt(this DateTime date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }

    public static string ToIso8601String(this DateTime dt)
    {
        return dt.ToString("o");
    }

    public static DateTime SetDate(this DateTime dt, DateOnly date)
    {
        return new DateTime(date.Year, date.Month, date.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);
    }

    public static DateTime SetTime(this DateTime dt, TimeOnly time)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, time.Hour, time.Minute, time.Second, time.Millisecond, dt.Kind);
    }
    
    public static DateOnly ToDateOnly(this DateTime dt) => DateOnly.FromDateTime(dt);

    public static TimeOnly ToTimeOnly(this DateTime dt) => TimeOnly.FromDateTime(dt);

    public static long ToUnixTimestamp(this DateTime dt)
    {
        return ((DateTimeOffset)dt).ToUnixTimeSeconds();
    }

    public static DateTime Truncate(this DateTime dt, TimeSpan timeSpan)
    {
        if (timeSpan == TimeSpan.Zero) return dt; // Or could throw an ArgumentException
        return dt.AddTicks(-(dt.Ticks % timeSpan.Ticks));
    }

    public static DateTime Truncate(this DateTime date, long resolution)
    {
        return new DateTime(date.Ticks - (date.Ticks % resolution), date.Kind);
    }

    /// <summary>
    /// Truncates the date part.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime TruncateDate(this DateTime dt)
    {
        return new DateTime(Empty.Year, Empty.Month, Empty.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, dt.Kind);
    }

    /// <summary>
    /// Truncates the time part of the <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime TruncateTime(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0, dt.Kind);
    }

    /// <summary>
    /// Truncates the minutes of the <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime TruncateMinutes(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, 0, dt.Kind);
    }

    /// <summary>
    /// Truncates the milliseconds of the <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime TruncateMilliseconds(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);
    }

    /// <summary>
    /// Truncates the seconds of the <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime TruncateSeconds(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, dt.Kind);
    }

    /// <summary>
    /// Compares values and <see cref="DateTime.Kind"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool ValueEquals(this DateTime lhs, DateTime rhs)
    {
        if (lhs.Kind != rhs.Kind) return false;
        return lhs.Equals(rhs);
    }

    /// <summary>
    /// Returns the weeks of the Month.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static IEnumerable<Period> WeeksOfMonth(this DateTime date, DateTimeKind kind = DateTimeKind.Utc)
    {
        var cultureInfo = Thread.CurrentThread.CurrentCulture;
        return date.WeeksOfMonth(cultureInfo, kind);
    }

    /// <summary>
    /// Returns the weeks of the Month.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="cultureInfo"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static IEnumerable<Period> WeeksOfMonth(this DateTime date, CultureInfo cultureInfo, DateTimeKind kind = DateTimeKind.Utc)
    {
        return date.WeeksOfMonth(cultureInfo.DateTimeFormat.FirstDayOfWeek, kind);
    }

    /// <summary>
    /// Returns the weeks of the Month.
    /// </summary>
    /// <param name="date"></param>
    /// <param name="start"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static IEnumerable<Period> WeeksOfMonth(this DateTime date, DayOfWeek start, DateTimeKind kind = DateTimeKind.Utc)
    {
        if (date.Kind != kind) throw new ArgumentException($"{nameof(date)} should be {kind}.");

        var lastDay = date.LastDayOfMonth();

        var weekStart = date.Day == 1 ? date : new DateTime(date.Year, date.Month, 1);
        for (var day = weekStart.Day; day <= lastDay.Day;)
        {
            var endOfWeek = weekStart.EndOfWeek(start);
            if (endOfWeek >= lastDay)
            {
                yield return Period.New(weekStart, endOfWeek);
                break;
            }

            yield return Period.New(weekStart, endOfWeek);
            weekStart = endOfWeek.AddDays(1);
            day = weekStart.Day;
        }
    }
}

