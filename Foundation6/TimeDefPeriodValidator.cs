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
﻿using Foundation;

namespace Foundation
{
    public class TimeDefPeriodValidator : TimeDefVisitor
    {
        private Period _period;
        private readonly IDictionary<string, bool>_results;

        public TimeDefPeriodValidator()
        {
            _results = new Dictionary<string, bool>();
        }

        public bool Validate(TimeDef td, Period period)
        {
            _period = period;

            _results.Clear();

            Visit(td);

            return _results.Values.All(x => x);
        }

        protected override bool VisitAnd(TimeDef.And td)
        {
            var results = _results.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            base.VisitAnd(td);

            var diff = _results.Except(results);

            var result = diff.All(kvp => kvp.Value);
            _results[nameof(TimeDef.And)] = result;

            return result;
        }

#if NET6_0_OR_GREATER
        protected override bool VisitDateSpan(TimeDef.DateSpan td)
        {
            var period = Period.New(td.From, td.To);
            var result = period.IsBetween(_period);
            _results[nameof(TimeDef.DateSpan)] = result;

            return result;
        }
#endif

        protected override bool VisitDateTimeSpan(TimeDef.DateTimeSpan td)
        {
            var result = _period.IsBetween(td.From, td.To);
            _results[nameof(TimeDef.DateTimeSpan)] = result;


            return result;
        }

        protected override bool VisitDay(TimeDef.Day td)
        {
            var result = td.DaysOfMonth.All(day => _period.ContainsDay(day));
            _results[nameof(TimeDef.Day)] = result;

            return result;
        }

        protected override bool VisitDays(TimeDef.Days td)
        {
            var result = td.Quantity <= _period.Days().Count();
            _results[nameof(TimeDef.Days)] = result;

            return result;
        }

        protected override bool VisitHour(TimeDef.Hour td)
        {
            var result = td.HoursOfDay.All(hour => _period.ContainsHour(hour));
            _results[nameof(TimeDef.Hour)] = result;

            return result;
        }

        protected override bool VisitHours(TimeDef.Hours td)
        {
            var result = td.Quantity <= _period.Hours().Count();
            _results[nameof(TimeDef.Hours)] = result;

            return result;
        }

        protected override bool VisitMinute(TimeDef.Minute td)
        {
            var result = td.MinutesOfHour.All(minute => _period.ContainsMinute(minute));
            _results[nameof(TimeDef.Minute)] = result;

            return result;
        }

        protected override bool VisitMinutes(TimeDef.Minutes td)
        {
            var result = td.Quantity <= _period.Minutes().Count();
            _results[nameof(TimeDef.Minutes)] = result;

            return result;
        }

        protected override bool VisitMonth(TimeDef.Month td)
        {
            var result = td.MonthsOfYear.All(month => _period.ContainsMonth((int)month));
            _results[nameof(TimeDef.Month)] = result;

            return result;
        }

        protected override bool VisitMonths(TimeDef.Months td)
        {
            var result = td.Quantity <= _period.Months().Count();
            _results[nameof(TimeDef.Months)] = result;

            return result;
        }

#if NET6_0_OR_GREATER
        protected override bool VisitTimespan(TimeDef.Timespan td)
        {
            var result = _period.IsBetween(td.From, td.To);
            _results[nameof(TimeDef.Timespan)] = result;

            return result;
        }
#endif

        protected override bool VisitWeekday(TimeDef.Weekday td)
        {
            var result = td.DaysOfWeek.All(weekday => _period.ContainsWeekday(weekday));
            _results[nameof(TimeDef.Weekday)] = result;

            return result;
        }

        protected override bool VisitWeeks(TimeDef.Weeks td)
        {
            var result = td.Quantity <= _period.Weeks().Count();
            _results[nameof(TimeDef.Weeks)] = result;

            return result;
        }

        protected override bool VisitYear(TimeDef.Year td)
        {
            var result = td.YearsOfPeriod.All(year => _period.ContainsYear(year));
            _results[nameof(TimeDef.Year)] = result;

            return result;
        }

        protected override bool VisitYears(TimeDef.Years td)
        {
            var result = td.Quantity <= _period.Years().Count();
            _results[nameof(TimeDef.Years)] = result;

            return result;
        }
    }
}
