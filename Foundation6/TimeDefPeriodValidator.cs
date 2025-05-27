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

        public bool IsValid(TimeDef td, Period period)
        {
            _period = period;

            return Visit(td);
        }

        protected override bool VisitDateSpan(TimeDef.DateSpan td)
        {
            var period = Period.New(td.From, td.To);
            return period.IsBetween(_period);
        }

        protected override bool VisitDateTimeSpan(TimeDef.DateTimeSpan td)
        {
            return _period.IsBetween(td.From, td.To);
        }

        protected override bool VisitDays(TimeDef.Days td)
        {
            return td.Quantity == _period.Duration.TotalDays;
        }

        protected override bool VisitHours(TimeDef.Hours td)
        {
            return td.Quantity == _period.Duration.TotalHours;
        }

        protected override bool VisitMinutes(TimeDef.Minutes td)
        {
            return td.Quantity == _period.Duration.TotalMinutes;
        }

        protected override bool VisitMonths(TimeDef.Months td)
        {
            return td.Quantity == _period.Months().Count();
        }

        protected override bool VisitTimespan(TimeDef.Timespan td)
        {
            return _period.IsBetween(td.From, td.To);
        }

        protected override bool VisitWeeks(TimeDef.Weeks td)
        {
            return td.Quantity == _period.Weeks().Count();
        }

        protected override bool VisitYears(TimeDef.Years td)
        {
            return td.Quantity == _period.Years().Count();
        }
    }
}
