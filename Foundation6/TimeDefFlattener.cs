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

public class TimeDefFlattener : TimeDefVisitor
{
    private readonly ICollection<TimeDef> _timeDefs;

    public TimeDefFlattener()
    {
        _timeDefs = new List<TimeDef>();
    }

    public void Clear() => _timeDefs.Clear();

    public IEnumerable<TimeDef> Flatten(TimeDef timeDef)
    {
        Visit(timeDef);

        return TimeDefs;
    }

    public IEnumerable<TimeDef> TimeDefs => _timeDefs;

    public override bool Visit(TimeDef timedef)
    {
        return base.Visit(timedef);
    }

    protected override bool VisitAnd(TimeDef.And td)
    {
        _timeDefs.Add(td);

        return base.VisitAnd(td);
    }
    protected override bool VisitDateSpan(TimeDef.DateSpan td)
    {
        _timeDefs.Add(td);

        return base.VisitDateSpan(td);
    }

    protected override bool VisitDateTimeSpan(TimeDef.DateTimeSpan td)
    {
        _timeDefs.Add(td);

        return base.VisitDateTimeSpan(td);
    }

    protected override bool VisitDay(TimeDef.Day td)
    {
        _timeDefs.Add(td);

        return base.VisitDay(td);
    }

    protected override bool VisitDays(TimeDef.Days td)
    {
        _timeDefs.Add(td);

        return base.VisitDays(td);
    }

    protected override bool VisitDifference(TimeDef.Difference td)
    {
        _timeDefs.Add(td);

        return base.VisitDifference(td);
    }

    protected override bool VisitHour(TimeDef.Hour td)
    {
        _timeDefs.Add(td);

        return base.VisitHour(td);
    }

    protected override bool VisitHours(TimeDef.Hours td)
    {
        _timeDefs.Add(td);

        return base.VisitHours(td);
    }

    protected override bool VisitMinute(TimeDef.Minute td)
    {
        _timeDefs.Add(td);

        return base.VisitMinute(td);
    }

    protected override bool VisitMinutes(TimeDef.Minutes td)
    {
        _timeDefs.Add(td);

        return base.VisitMinutes(td);
    }

    protected override bool VisitMonth(TimeDef.Month td)
    {
        _timeDefs.Add(td);

        return base.VisitMonth(td);
    }

    protected override bool VisitMonths(TimeDef.Months td)
    {
        _timeDefs.Add(td);

        return base.VisitMonths(td);
    }

    protected override bool VisitNot(TimeDef.Not td)
    {
        _timeDefs.Add(td);

        return base.VisitNot(td);
    }

    protected override bool VisitOr(TimeDef.Or td)
    {
        _timeDefs.Add(td);

        return base.VisitOr(td);
    }
    protected override bool VisitTimespan(TimeDef.Timespan td)
    {
        _timeDefs.Add(td);

        return base.VisitTimespan(td);
    }

    protected override bool VisitUnion(TimeDef.Union td)
    {
        _timeDefs.Add(td);

        return base.VisitUnion(td);
    }

    protected override bool VisitWeekday(TimeDef.Weekday td)
    {
        _timeDefs.Add(td);

        return base.VisitWeekday(td);
    }

    protected override bool VisitWeekOfMonth(TimeDef.WeekOfMonth td)
    {
        _timeDefs.Add(td);

        return base.VisitWeekOfMonth(td);
    }

    protected override bool VisitWeeks(TimeDef.Weeks td)
    {
        _timeDefs.Add(td);

        return base.VisitWeeks(td);
    }

    protected override bool VisitYear(TimeDef.Year td)
    {
        _timeDefs.Add(td);

        return base.VisitYear(td);
    }

    protected override bool VisitYears(TimeDef.Years td)
    {
        _timeDefs.Add(td);

        return base.VisitYears(td);
    }
}
