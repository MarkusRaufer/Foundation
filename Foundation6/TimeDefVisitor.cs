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

public abstract class TimeDefVisitor
{
    private TriState _result;

    public virtual bool Visit(TimeDef timedef)
    {
        if (_result.State.TryGet(out bool state) && !state) return false;

        var result = timedef switch
        {
            TimeDef.And td => VisitAnd(td),
            TimeDef.DateSpan td => VisitDateSpan(td),
            TimeDef.DateTimeSpan td => VisitDateTimeSpan(td),
            TimeDef.Day td => VisitDay(td),
            TimeDef.Days td => VisitDays(td),
            TimeDef.Difference td => VisitDifference(td),
            TimeDef.Hour td => VisitHour(td),
            TimeDef.Hours td => VisitHours(td),
            TimeDef.Minute td => VisitMinute(td),
            TimeDef.Minutes td => VisitMinutes(td),
            TimeDef.Month td => VisitMonth(td),
            TimeDef.Months td => VisitMonths(td),
            TimeDef.Not td => VisitNot(td),
            TimeDef.Or td => VisitOr(td),
            TimeDef.Timespan td => VisitTimespan(td),
            TimeDef.Union td => VisitUnion(td),
            TimeDef.Weekday td => VisitWeekday(td),
            TimeDef.WeekOfMonth td => VisitWeekOfMonth(td),
            TimeDef.Weeks td => VisitWeeks(td),
            TimeDef.Year td => VisitYear(td),
            TimeDef.Years td => VisitYears(td),
            _ => false,
        };

        _result = new TriState(result);

        return result;
    }

    protected virtual bool VisitAnd(TimeDef.And td)
    {
        return Visit(td.Lhs) && Visit(td.Rhs);
    }

    protected virtual bool VisitDateSpan(TimeDef.DateSpan td)
    {
        return true;
    }

    protected virtual bool VisitDateTimeSpan(TimeDef.DateTimeSpan td)
    {
        return true;
    }

    protected virtual bool VisitDay(TimeDef.Day td)
    {
        return true;
    }

    protected virtual bool VisitDays(TimeDef.Days td)
    {
        return true;
    }

    protected virtual bool VisitDifference(TimeDef.Difference td)
    {
        Visit(td.Lhs);
        Visit(td.Rhs);

        return true;
    }

    protected virtual bool VisitHour(TimeDef.Hour td)
    {
        return true;
    }

    protected virtual bool VisitHours(TimeDef.Hours td)
    {
        return true;
    }

    protected virtual bool VisitMinute(TimeDef.Minute td)
    {
        return true;
    }

    protected virtual bool VisitMinutes(TimeDef.Minutes td)
    {
        return true;
    }

    protected virtual bool VisitMonth(TimeDef.Month td)
    {
        return true;
    }

    protected virtual bool VisitMonths(TimeDef.Months td)
    {
        return true;
    }

    protected virtual bool VisitNot(TimeDef.Not td)
    {
        return !Visit(td.TimeDef);
    }

    protected virtual bool VisitOr(TimeDef.Or td)
    {
        return Visit(td.Lhs) || Visit(td.Rhs);
    }

    protected virtual bool VisitTimespan(TimeDef.Timespan td)
    {
        return true;
    }

    protected virtual bool VisitUnion(TimeDef.Union td)
    {
        Visit(td.Lhs);
        Visit(td.Rhs);

        return true;
    }

    protected virtual bool VisitWeekday(TimeDef.Weekday td)
    {
        return true;
    }

    protected virtual bool VisitWeekOfMonth(TimeDef.WeekOfMonth td)
    {
        return true;
    }

    protected virtual bool VisitWeeks(TimeDef.Weeks td)
    {
        return true;
    }

    protected virtual bool VisitYear(TimeDef.Year td)
    {
        return true;
    }

    protected virtual bool VisitYears(TimeDef.Years td)
    {
        return true;
    }
}

