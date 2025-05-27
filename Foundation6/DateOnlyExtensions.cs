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
using System.Globalization;

namespace Foundation;

public static class DateOnlyExtensions
{
    public static DateOnly Add(this DateOnly date, TimeSpan span)
    {
        return DateOnly.FromDateTime(date.ToDateTime().Add(span));
    }

    public static DateOnly EndOfWeek(this DateOnly date)
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        return EndOfWeek(date, culture);
    }

    public static DateOnly EndOfWeek(this DateOnly date, DayOfWeek start)
    {
        if (!DayOfWeekHelper.IsValidFirstDayOfWeek(start))
            throw new ArgumentOutOfRangeException(nameof(start));

        var days = 7 + (int)start - (int)date.DayOfWeek - 1;
        return date.AddDays(days);
    }

    public static DateOnly EndOfWeek(this DateOnly date, CultureInfo culture)
    {
        if (null == culture) throw new ArgumentOutOfRangeException(nameof(culture));

        return EndOfWeek(date, culture.DateTimeFormat.FirstDayOfWeek);
    }

    public static DateOnly FirstDayOfWeek(this DateOnly date, CultureInfo cultureInfo)
    {
        return FirstDayOfWeek(date, cultureInfo.DateTimeFormat.FirstDayOfWeek);
    }

    public static DateOnly FirstDayOfWeek(this DateOnly date, DayOfWeek start)
    {
        if (!DayOfWeekHelper.IsValidFirstDayOfWeek(start))
            throw new ArgumentOutOfRangeException(nameof(start));

        return date.AddDays(-(date.DayOfWeek - start));
    }

    /// <summary>
    /// returns the last day of the month.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateOnly LastDayOfMonth(this DateOnly date)
    {
        var lastDay = LastDayOfMonthAsInt(date);
        return new DateOnly(date.Year, date.Month, lastDay);
    }

    public static int LastDayOfMonthAsInt(this DateOnly date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }


    public static TimeSpan Subtract(this DateOnly date, DateOnly subtract)
        => TimeSpan.FromDays(date.DayNumber - subtract.DayNumber);

    public static DateOnly Subtract(this DateOnly date, TimeSpan span)
        => DateOnly.FromDateTime(date.ToDateTime().Subtract(span));

    public static DateOnly SubtractDays(this DateOnly date, int days)
        => date.AddDays(days * -1);

    public static DateOnly SubtractMonths(this DateOnly date, int months)
        => date.AddMonths(months * -1);

    public static DateOnly SubtractYears(this DateOnly date, int years)
        => date.AddMonths(years * -1);

    public static DateTime ToDateTime(this DateOnly date)
    {
        return new DateTime(date.Year, date.Month, date.Day);
    }

    public static DateTime ToDateTime(this DateOnly date, DateTimeKind kind)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, kind);
    }

    public static long ToTicks(this DateOnly date) => date.ToDateTime().Ticks;

    public static long ToTicks(this DateOnly date, DateTimeKind kind) => date.ToDateTime(kind).Ticks;

    public static IEnumerable<Period> WeeksOfMonth(this DateOnly date, DayOfWeek start, DateTimeKind kind = DateTimeKind.Utc)
    {
        var lastDay = date.LastDayOfMonth();

        var weekStart = date;
        for (var day = 1; day <= lastDay.Day; day++)
        {
            var endOfWeek = weekStart.EndOfWeek(start);
            if (endOfWeek.Month > date.Month)
            {
                endOfWeek = endOfWeek.LastDayOfMonth();
                yield return Period.New(weekStart, endOfWeek);
                break;
            }
            weekStart = endOfWeek.AddDays(1);
            yield return Period.New(weekStart, endOfWeek, kind);
        }
    }
}
