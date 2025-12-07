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

public class TimeDefDateTimeValidator : TimeDefVisitor
{
    private DateTime _dateTime;

    public bool IsValid(TimeDef td, DateTime dateTime)
    {
        _dateTime = dateTime;
        return Visit(td);
    }

    protected override bool VisitDay(TimeDef.Day td)
    {
        return td.DaysOfMonth.Contains(_dateTime.Day);
    }

    protected override bool VisitHour(TimeDef.Hour td)
    {
        return td.HoursOfDay.Contains(_dateTime.Hour);
    }

    protected override bool VisitMinute(TimeDef.Minute td)
    {
        return td.MinutesOfHour.Contains(_dateTime.Minute);
    }

    protected override bool VisitMonth(TimeDef.Month td)
    {
        Month month = (Month)_dateTime.Month;

        return td.MonthsOfYear.Contains(month);
    }

    protected override bool VisitWeekday(TimeDef.Weekday td)
    {
        return td.DaysOfWeek.Contains(_dateTime.DayOfWeek);
    }

    protected override bool VisitYear(TimeDef.Year td)
    {
        return td.YearsOfPeriod.Contains(_dateTime.Year);
    }

    protected override bool VisitWeekOfMonth(TimeDef.WeekOfMonth td)
    {
        var weeks = _dateTime.WeeksOfMonth(_dateTime.Kind).ToArray();
        foreach (var week in td.Week)
        {
            if (week < 1 || week > weeks.Length) return false;

            var period = weeks[week - 1];
            if (!period.IsWithin(_dateTime)) return false;
        }
        return true; 
    }
}
