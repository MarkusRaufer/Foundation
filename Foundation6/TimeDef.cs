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

using Foundation.Collections.Generic;

public abstract record TimeDef
{
    #region abstracts
    public abstract record BinaryTimeDef(TimeDef Lhs, TimeDef Rhs) : TimeDef;
    public abstract record QuantityTimeDef(int Quantity) : TimeDef;
    public abstract record SpanTimeDef<T>(T From, T To) : TimeDef;
    #endregion abstracts

    #region time definitions
    public sealed record And(TimeDef Lhs, TimeDef Rhs) : BinaryTimeDef(Lhs, Rhs);
    public sealed record DateSpan(DateOnly From, DateOnly To) : SpanTimeDef<DateOnly>(From, To);
    public sealed record Timespan(TimeOnly From, TimeOnly To) : SpanTimeDef<TimeOnly>(From, To);
    public sealed record DateTimeSpan(DateTime From, DateTime To) : SpanTimeDef<DateTime>(From, To);
    public sealed record Day(NonEmptyHashSetValue<int> DaysOfMonth) : TimeDef;
    public sealed record Days(int Quantity) : QuantityTimeDef(Quantity);
    public sealed record Difference(TimeDef Lhs, TimeDef Rhs) : BinaryTimeDef(Lhs, Rhs);
    public sealed record Hour(NonEmptyHashSetValue<int> HoursOfDay) : TimeDef;
    public sealed record Hours(int Quantity) : QuantityTimeDef(Quantity);
    public sealed record Minute(NonEmptyHashSetValue<int> MinutesOfHour) : TimeDef;
    public sealed record Minutes(int Quantity) : QuantityTimeDef(Quantity);
    public sealed record Month(NonEmptyHashSetValue<Foundation.Month> MonthsOfYear) : TimeDef;
    public sealed record Months(int Quantity) : QuantityTimeDef(Quantity);
    public sealed record Not(TimeDef TimeDef) : TimeDef;
    public sealed record Or(TimeDef Lhs, TimeDef Rhs) : BinaryTimeDef(Lhs, Rhs);
    public sealed record Union(TimeDef Lhs, TimeDef Rhs) : BinaryTimeDef(Lhs, Rhs);
    public sealed record Weekday(NonEmptyHashSetValue<DayOfWeek> DaysOfWeek) : TimeDef;
    public sealed record WeekOfMonth(DayOfWeek WeekStartsWith, NonEmptyHashSetValue<int> Week) : TimeDef;
    public sealed record Weeks(int Quantity, DayOfWeek WeekStartsWith) : QuantityTimeDef(Quantity);
    public sealed record Year(NonEmptyHashSetValue<int> YearsOfPeriod) : TimeDef;
    public sealed record Years(int Quantity) : QuantityTimeDef(Quantity);
    #endregion time defintions

    #region factory methods

    /// <summary>
    /// Chains a list of <see cref="TimeDef"/> by <see cref="TimeDef.And"/>.
    /// </summary>
    /// <param name="timeDefs">TimeDef list to chain by <see cref="TimeDef.And"/></param>
    /// <returns></returns>
    public static TimeDef ChainByAnd(params TimeDef[] timeDefs) => timeDefs.ChainByAnd();

    /// <summary>
    /// Chains timeDefs by Or.
    /// </summary>
    /// <param name="timeDefs"></param>
    /// <returns></returns>
    public static TimeDef ChainByOr(params TimeDef[] timeDefs) => timeDefs.ChainByOr();

    public static TimeDef FromDate(int year, int month, int day)
    {
        return FromDate(year, (Foundation.Month)month, day);
    }

    public static TimeDef FromDate(int year, Foundation.Month month, int day)
    {
        var dtYear = FromYear(year);
        var dtMonth = FromMonth(month);
        var dtDay = FromDay(day);

        return ChainByAnd(dtYear, dtMonth, dtDay);
    }

    public static TimeDef FromDateTime(DateTime dateTime)
    {
        var year = FromYear(dateTime.Year);
        var month = FromMonth((Foundation.Month)dateTime.Month);
        var day = FromDay(dateTime.Day);
        var hour = FromHour(dateTime.Hour);
        var minute = FromMinute(dateTime.Minute);
        
        return ChainByAnd(year, month, day, hour, minute);
    }

    public static TimeDef FromDay(params int[] day)
    {
        if (day.Any(d => d is < 1 or > 31)) throw new ArgumentOutOfRangeException(nameof(day), "must be between [1..31]");

        return new Day(day);
    }

    public static TimeDef FromHour(params int[] hour)
    {
        if (hour.Any(h => h is < 0 or > 23)) throw new ArgumentOutOfRangeException(nameof(hour), "must be between [0..23]");

        return new Hour(hour);
    }

    public static TimeDef FromDateOnly(DateOnly date)
    {
        return FromDate(date.Year, date.Month, date.Day);
    }

    public static TimeDef FromDay(System.Range range)
    {
        var days = range.ToEnumerable();
        return FromDay(days.ToArray());
    }

    public static TimeDef FromHour(System.Range range)
    {
        var hours = range.ToEnumerable();
        return FromHour(hours.ToArray());
    }

    public static TimeDef FromMinute(System.Range range)
    {
        var minutes = range.ToEnumerable();
        return FromMinute(minutes.ToArray());
    }

    public static TimeDef FromTimeOnly(TimeOnly time)
    {
        var hour = new Hour(new[] { time.Hour });
        var minute = new Minute(new[] { time.Minute });

        return new And(hour, minute);
    }

    public static TimeDef FromYear(System.Range range)
    {
        var years = range.ToEnumerable();
        return new Year(years.ToArray());
    }

    public static TimeDef FromMinute(params int[] minute)
    {
        if (minute.Any(m => m is < 0 or > 59)) throw new ArgumentOutOfRangeException(nameof(minute), "must be between [0..59]");

        return new Minute(minute);
    }

    public static TimeDef FromMonth(params Foundation.Month[] month)
    {
        return new Month(month);
    }

    public static TimeDef FromMonth(params int[] month)
    {
        return new Month(month.Cast<Foundation.Month>().ToArray());
    }

    public static TimeDef FromTime(int hour, int minute)
    {
        var tdHour = FromHour(hour);
        var tdMinute = FromMinute(minute);

        return new And(tdHour, tdMinute);
    }

    public static TimeDef FromWeekday(params DayOfWeek[] dayOfWeek)
    {
        return new Weekday(dayOfWeek);
    }

    public static TimeDef FromWeekOfMonth(DayOfWeek weekStartsWith, params int[] week)
    {
        return new WeekOfMonth(weekStartsWith, week);
    }

    public static TimeDef FromYear(params int[] year)
    {
        return new Year(year);
    }

    # endregion factory methods
}
