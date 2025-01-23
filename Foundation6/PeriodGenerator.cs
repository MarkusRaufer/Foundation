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
﻿using static Foundation.TimeDef;

namespace Foundation;

public class PeriodGenerator : IPeriodGenerator
{
    private readonly Func<Period, IEnumerable<Period>> _generator;

    public PeriodGenerator(Func<Period, IEnumerable<Period>> generator)
    {
        _generator = generator ?? throw new ArgumentNullException(nameof(generator));
    }

    public IEnumerable<Period> GeneratePeriods(Period period)
    {
        return _generator(period);
    }

    public static IEnumerable<Period> GeneratePeriods(TimeDef td, Period period)
    {
        var range = new TimeDefRange();
        range.Visit(td);

        if (range.Smallest.IsNone) return Enumerable.Empty<Period>();

        return range.Smallest.OrThrow() switch
        {
            Day _ => period.Days(),
            Days _ => period.Days(),
            Hour _ => period.Hours(),
            Hours _ => period.Hours(),
            Minute _ => period.Minutes(),
            Minutes _ => period.Minutes(),
            TimeDef.Month _ => period.Months(),
            Months _ => period.Months(),
            Timespan _ => period.Minutes(),
            Weekday _ => period.Days(),
            WeekOfMonth _ => period.Weeks(),
            Weeks _ => period.Weeks(),
            Year _ => period.Years(),
            Years _ => period.Years(),
            _ => Enumerable.Empty<Period>()
        };
    }
}

public class PeriodGenerator2
{
    public IEnumerable<Period> GeneratePeriods<T>(SpanTimeDef<T> span, Period? limit = null)
    {
        if(span is SpanTimeDef<DateOnly> dateSpan)
        {
            var period = Period.New(dateSpan.From, dateSpan.To);
            return period.Days();
        }

        if (span is SpanTimeDef<TimeOnly> timeSpan)
        {
            if (null == limit) return Enumerable.Empty<Period>();


            var period = Period.New(timeSpan.From, timeSpan.To);

            var range = new TimeDefRange();
            range.Visit(timeSpan);

            if (!range.Smallest.TryGet(out var smallest)) return Enumerable.Empty<Period>();

            return PeriodHelper.Chop(limit.Value, smallest);
        }

        if (span is SpanTimeDef<DateTime> dateTimeSpan)
        {
            var period = Period.New(dateTimeSpan.From, dateTimeSpan.To);
            return period.Days();
        }

        return [];
    }
}