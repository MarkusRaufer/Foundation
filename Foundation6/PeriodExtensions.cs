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

public static class PeriodExtensions
{
    public static Period AddDuration(this Period period, TimeSpan duration)
    {
        return Period.New(period.Start, period.Duration + duration);
    }

    public static bool ContainsDay(this Period period, int day)
    {
        return period.Days().Any(p => p.Start.Day == day);
    }

    public static bool ContainsHour(this Period period, int hour)
    {
        return period.Hours().Any(p => p.Start.Hour == hour);
    }

    public static bool ContainsMinute(this Period period, int minute)
    {
        return period.Minutes().Any(p => p.Start.Minute == minute);
    }

    public static bool ContainsMonth(this Period period, int month)
    {
        return period.Months().Any(p => p.Start.Month == month);
    }

    public static bool ContainsWeekday(this Period period, DayOfWeek dayOfWeek)
    {
        return period.Days().Any(p => p.Start.DayOfWeek == dayOfWeek);
    }

    public static bool ContainsYear(this Period period, int year)
    {
        return period.Years().Any(p => p.Start.Year == year);
    }

    /// <summary>
    /// returns a period for each day.
    /// </summary>
    /// <param name="period"></param>
    /// <returns></returns>
    public static IEnumerable<Period> Days(this Period period)
    {
        if (period.Duration.Days < 1)
        {
            yield return period;
            yield break;
        }
        var currentDay = period.Start;
        while (currentDay < period.End)
        {
            var end = currentDay.EndOfDay();
            if (end > period.End) end = period.End;
            yield return Period.New(currentDay, end);
            currentDay = currentDay.Date + TimeSpan.FromDays(1);
        }
    }

    /// <summary>
    /// Returns period of lhs which is not in rhs.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static IEnumerable<Period> Except(this Period lhs, Period rhs)
    {
        //lhs   [------]
        //rhs [----------]
        if (rhs.Start <= lhs.Start && rhs.End >= lhs.End) yield break;

        //lhs [----------
        //rhs    [-------
        if (lhs.Start < rhs.Start) yield return Period.New(lhs.Start, rhs.Start);

        //lhs --------]
        //rhs -----]
        if (lhs.End > rhs.End) yield return Period.New(rhs.End, lhs.End);
    }

    /// <summary>
    /// returns a period for each hour.
    /// </summary>
    /// <param name="period"></param>
    /// <returns></returns>
    public static IEnumerable<Period> Hours(this Period period)
    {
        if (period.Duration.TotalHours < 1)
        {
            yield return period;
            yield break;
        }
        var currentHour = period.Start;
        while (currentHour < period.End)
        {
            var end = currentHour.EndOfHour();
            if (end > period.End) end = period.End;
            yield return Period.New(currentHour, end);
            currentHour = end;
        }
    }

    public static Option<Period> Intersect(this Period lhs, Period rhs) => lhs.Intersect(rhs.Start, rhs.End);

    public static IEnumerable<Period> Intersect(this Period period, IEnumerable<Period> periods)
    {
        foreach (var p in periods)
        {
            var intersected = period.Intersect(p);
            if (intersected.TryGet(out Period value)) yield return value;
        }
    }

    public static Option<Period> Intersect(this Period period, DateTime start, DateTime end)
    {
        if (period.IsEmpty) return Option.None<Period>();
        if (period.Start < start)
        {
            if (period.End < start) return Option.None<Period>();
            if (period.End < end)
                end = period.End;

            return Option.Some(Period.New(start, end));
        }

        if (period.Start > end) return Option.None<Period>();

        if (period.End < end)
            end = period.End;

        return Option.Some(Period.New(period.Start, end));
    }

    /// <summary>
    /// Returns true if dateTime greater or equal the periods Start and smaller or equal the periods End.
    /// </summary>
    /// <param name="period">A given period</param>
    /// <param name="dateTime">A DateTime which is checked for a period.</param>
    /// <returns></returns>
    public static bool IsBetween(this Period period, DateTime dateTime)
    {
        return dateTime >= period.Start && dateTime <= period.End;
    }

    /// <summary>
    /// Checks if period rhs is between period lhs;
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool IsBetween(this Period lhs, Period rhs)
    {
        return lhs.Start >= rhs.Start && lhs.End <= rhs.End;
    }

    /// <summary>
    /// Checks if the period is between the start and end datetime.
    /// </summary>
    /// <param name="period"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static bool IsBetween(this Period period, DateTime start, DateTime end)
    {
        return period.Start >= start && period.End <= end;
    }

    /// <summary>
    /// Checks if the period is between the start and end date
    /// </summary>
    /// <param name="period"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static bool IsBetween(this Period period, DateOnly start, DateOnly end)
    {
        return period.Start.ToDateOnly() >= start && period.End.ToDateOnly() <= end;
    }

    /// <summary>
    /// Checks if the period is between the start and end time
    /// </summary>
    /// <param name="period"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static bool IsBetween(this Period period, TimeOnly start, TimeOnly end)
    {
        return period.Start.ToTimeOnly() >= start && period.End.ToTimeOnly() <= end;
    }

    /// <summary>
    /// Checks if the period is between the date start and the duration.
    /// </summary>
    /// <param name="period"></param>
    /// <param name="start"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static bool IsBetween(this Period period, DateTime start, TimeSpan duration) => IsBetween(period, start, start.Add(duration));

    /// <summary>
    /// Is the period between a list of periods.
    /// </summary>
    /// <param name="period"></param>
    /// <param name="periods"></param>
    /// <returns></returns>
    public static bool IsBetween(this Period period, IEnumerable<Period> periods)
    {
        var diffs = new List<Period>();
        foreach (var coverPeriod in PeriodHelper.Merge(periods.ToArray()))
        {
            if (period.IsBetween(coverPeriod)) return true;
            if (!period.IsOverlapping(coverPeriod)) continue;

            var diff = period.Intersect(coverPeriod);

            if (!diff.TryGet(out Period diffValue)) continue;

            if (period.IsBetween(diffValue)) return true;

            diffs.Add(diffValue);
        }
        return IsBetween(period, diffs);
    }

    public static bool IsDayBetween(this Period period, int day)
    {
        return day >= period.Start.Day && day <= period.End.Day;
    }

    public static bool IsHourBetween(this Period period, int hour)
    {
        return hour >= period.Start.Hour && hour <= period.End.Hour;
    }

    public static bool IsMinuteBetween(this Period period, int minute)
    {
        return minute >= period.Start.Minute && minute <= period.End.Minute;
    }

    public static bool IsMonthBetween(this Period period, int month)
    {
        return month >= period.Start.Month && month <= period.End.Month;
    }

    public static bool IsYearBetween(this Period period, int year)
    {
        return year >= period.Start.Year && year <= period.End.Year;
    }

    /// <summary>
    /// Checks if the period lhs overlaps the period rhs.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool IsOverlapping(this Period lhs, Period rhs) => lhs.IsOverlapping(rhs.Start, rhs.End);

    /// <summary>
    /// Checks if a period overlaps a period between a start and an end date.
    /// </summary>
    /// <param name="period"></param>
    /// <param name="start">The start date of the period.</param>
    /// <param name="end">The end date of the period.</param>
    /// <returns></returns>
    public static bool IsOverlapping(this Period period, DateTime start, DateTime end)
    {
        if (period.Start <= start)
            return (period.End >= start);

        return (period.Start <= end);
    }

    /// <summary>
    /// Checks if a period overlaps a period starting from a start date.
    /// </summary>
    /// <param name="period"></param>
    /// <param name="start">The start date.</param>
    /// <param name="duration">the duration of the period.</param>
    /// <returns></returns>
    public static bool IsOverlapping(this Period period, DateTime start, TimeSpan duration)
    {
        return period.IsOverlapping(start, start.Add(duration));
    }

    /// <summary>
    /// Checks if the time part of date time is between the period times.
    /// The date is ignored.
    /// </summary>
    /// <param name="period"></param>
    /// <param name="time"></param>
    /// <returns></returns>

#if NETSTANDARD2_0
    public static bool IsTimeBetween(this Period period, DateTime time)
    {
        var start = period.Start.TruncateDate();
        var end = period.End.TruncateDate();

        time = time.TruncateDate();
#else
    public static bool IsTimeBetween(this Period period, TimeOnly time)
    {
        var start = period.Start.ToTimeOnly();
        var end = period.End.ToTimeOnly();
#endif

        return time >= start && time <= end;
    }

    /// <summary>
    /// Checks if the time part of a period is within an other period times.
    /// The date is ignored.
    /// </summary>
    /// <param name="period"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool IsTimeBetween(this Period period, Period other)
    {
#if NETSTANDARD2_0
        var start = period.Start.TruncateDate();
        var end = period.End.TruncateDate();

        var otherStart = other.Start.TruncateDate();
        var otherEnd = other.End.TruncateDate();
#else
        var start = period.Start.ToTimeOnly();
        var end = period.End.ToTimeOnly();

        var otherStart = other.Start.ToTimeOnly();
        var otherEnd = other.End.ToTimeOnly();
#endif
        return otherStart >= start && otherEnd <= end;
    }

    public static bool IsTimeBetween(this Period period, IEnumerable<Period> coverPeriods)
    {
        var diffs = new List<Period>();
        foreach (var coverPeriod in PeriodHelper.Merge(coverPeriods.ToArray()))
        {
            if (period.IsTimeBetween(coverPeriod)) return true;
            if (!period.IsOverlapping(coverPeriod)) continue;

            var diff = period.Intersect(coverPeriod);

            if (!diff.TryGet(out Period diffValue)) continue;

            if (period.IsTimeBetween(diffValue)) return true;

            diffs.Add(diffValue);
        }
        return IsBetween(period, diffs);
    }

    /// <summary>
    /// Returns true if dateTime greater than the period Start and smaller than period End.
    /// </summary>
    /// <param name="period">A given period</param>
    /// <param name="dateTime">A DateTime which is checked for a period.</param>
    /// <returns></returns>
    public static bool IsWithin(this Period period, DateTime dateTime)
    {
        return dateTime > period.Start && dateTime < period.End;
    }

    public static bool IsWithin(this Period lhs, Period rhs)
    {
        return lhs.Start > rhs.Start && lhs.End < rhs.End;
    }

    /// <summary>
    /// returns a period for each minute.
    /// </summary>
    /// <param name="period"></param>
    /// <returns></returns>
    public static IEnumerable<Period> Minutes(this Period period)
    {
        if (period.Duration.TotalMinutes < 1)
        {
            yield return period;
            yield break;
        }
        var currentMinute = period.Start;
        while (currentMinute < period.End)
        {
            var end = currentMinute.EndOfMinute();
            if (end > period.End) end = period.End;
            yield return Period.New(currentMinute, end);
            currentMinute = end;
        }
    }

    /// <summary>
    /// returns a period for each month.
    /// </summary>
    /// <param name="period"></param>
    /// <returns></returns>
    public static IEnumerable<Period> Months(this Period period)
    {
        if (period.Start.Year == period.End.Year && period.Start.Month == period.End.Month)
        {
            yield return period;
            yield break;
        }
        var currentMonth = period.Start;
        while (currentMonth < period.End)
        {
            var end = currentMonth.EndOfMonth().EndOfDay();
            if (end > period.End) end = period.End;
            yield return Period.New(currentMonth, end);
            currentMonth = currentMonth.Date.AddMonths(1);
        }
    }

    public static IEnumerable<Period> SymmetricDifference(this Period lhs, Period rhs)
    {
        //lhs [----------]
        //rhs [----------]
        if (lhs.Start == rhs.Start && lhs.End == rhs.End) yield break;

        //lhs [----------]
        //rhs               [----------]
        if (lhs.Start < rhs.Start && lhs.End < rhs.Start || rhs.Start < lhs.Start && rhs.End < lhs.Start)
        {
            yield return lhs;
            yield return rhs;
            yield break;
        }

        //lhs [--------
        //rhs     [----
        if (lhs.Start < rhs.Start) yield return Period.New(lhs.Start, rhs.Start);
        else if (lhs.Start > rhs.Start) yield return Period.New(rhs.Start, lhs.Start);

        //lhs -------]
        //rhs ----------]
        if (lhs.End < rhs.End) yield return Period.New(lhs.End, rhs.End);
        else if (lhs.End > rhs.End) yield return Period.New(rhs.End, lhs.End);
    }


    /// <summary>
    /// Merges two periods if the are overlapping.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns>If the last value of the tuple is true the periods where merged.</returns>
    public static Option<Period> Union(this Period lhs, Period rhs)
    {
        if (rhs.IsEmpty) return Option.None<Period>();

        return lhs.Union(rhs.Start, rhs.End);
    }


    /// <summary>
    /// Merges two periods if the are overlapping.
    /// </summary>
    /// <param name="period"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns>If the last value of the tuple is true the periods where merged.</returns>
    public static Option<Period> Union(this Period period, DateTime start, DateTime end)
    {
        if (period.IsEmpty) return Option.None<Period>();
        if (!period.IsOverlapping(start, end)) return Option.None<Period>();

        var startMin = DateTimeHelper.Min(period.Start, start);
        var endMax = DateTimeHelper.Max(period.End, end);
        return Period.New(startMin, endMax);
    }

    public static IEnumerable<DayOfWeek> WeekDays(this Period period)
    {
        foreach (var day in Days(period))
            yield return day.Start.DayOfWeek;
    }

    /// <summary>
    /// returns a period for each week.
    /// </summary>
    /// <param name="period"></param>
    /// <param name="start"></param>
    /// <returns></returns>
    public static IEnumerable<Period> Weeks(this Period period, DayOfWeek start = DayOfWeek.Monday)
    {
        if (!DayOfWeekHelper.IsValidFirstDayOfWeek(start)) throw new ArgumentOutOfRangeException(nameof(start));
        if (period.End < period.Start.EndOfWeek(start, false))
        {
            yield return period;
            yield break;
        }
        var currentWeek = period.Start;
        var periodEnd = period.End.Date;
        while (currentWeek < period.End)
        {
            var end = currentWeek.EndOfWeek(start, false);
            
            if (end >= periodEnd) end = period.End;
            yield return Period.New(currentWeek, end);
            currentWeek = end.AddDays(1);
        }
    }

    /// <summary>
    /// returns a period for each year.
    /// </summary>
    /// <param name="period"></param>
    /// <returns></returns>
    public static IEnumerable<Period> Years(this Period period)
    {
        if (period.Start.Year == period.End.Year)
        {
            yield return period;
            yield break;
        }
        var currentYear = period.Start;
        while (currentYear < period.End)
        {
            var end = currentYear.EndOfYear();
            if (end > period.End) end = period.End;
            yield return Period.New(currentYear, end);
            currentYear = currentYear.Date.AddYears(1);
        }
    }
}

