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

public class TimeDefRange : TimeDefVisitor
{
    public Option<TimeDef> Greatest { get; private set; }

    protected bool SetGreatest(TimeDef? td)
    {
        ArgumentNullException.ThrowIfNull(td);

        if (!Greatest.TryGet(out var timeDef) || timeDef.CompareTo(td) == -1)
            Greatest = Option.Some(td);

        return true;
    }

    protected bool SetSmallest(TimeDef td)
    {
        if (!Smallest.TryGet(out var timeDef) || timeDef.CompareTo(td) == 1)
            Smallest = Option.Some(td);

        return true;
    }

    public Option<TimeDef> Smallest { get; private set; }

    protected override bool VisitDateSpan(TimeDef.DateSpan td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitDateTimeSpan(TimeDef.DateTimeSpan td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitDay(TimeDef.Day td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitDays(TimeDef.Days td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitHour(TimeDef.Hour td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitHours(TimeDef.Hours td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitMinute(TimeDef.Minute td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitMinutes(TimeDef.Minutes td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitMonth(TimeDef.Month td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitMonths(TimeDef.Months td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitTimespan(TimeDef.Timespan td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitWeekday(TimeDef.Weekday td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitWeekOfMonth(TimeDef.WeekOfMonth td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitWeeks(TimeDef.Weeks td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitYear(TimeDef.Year td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }

    protected override bool VisitYears(TimeDef.Years td)
    {
        SetGreatest(td);
        SetSmallest(td);

        return true;
    }
}

