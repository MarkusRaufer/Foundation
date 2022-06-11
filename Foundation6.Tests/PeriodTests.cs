using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation
{
    [TestFixture]
    public class PeriodTests
    {
        // ReSharper disable InconsistentNaming
        
        [Test]
        public void Days_3Days()
        {
            DateTime newDate(int day) => new DateTime(2015, 7, day);

            var startDate = newDate(1);
            const int duration = 3;
            var period = Period.New(startDate, startDate.Add(TimeSpan.FromDays(duration)));

            var days = period.Days().ToList();
            Assert.AreEqual(duration, days.Count);

            var oneDay = TimeSpan.FromDays(1);

            var date = startDate;
            Assert.AreEqual(date, days[0].Start);
            Assert.AreEqual(date.Add(oneDay), days[0].End);

            date += oneDay;
            Assert.AreEqual(date, days[1].Start);
            Assert.AreEqual(date.Add(oneDay), days[1].End);

            date += oneDay;
            Assert.AreEqual(date, days[2].Start);
            Assert.AreEqual(date.Add(oneDay), days[2].End);
        }


        [Test]
        public void Days_3FullDays()
        {
            DateTime newDate(int day, int hour) => new DateTime(2015, 1, day, hour, 0, 0);

            var period = Period.New(newDate(1, 8), newDate(3, 17));

            var periods = period.Days().ToList();
            Assert.AreEqual(3, periods.Count);
            Assert.AreEqual(newDate(1, 8), periods[0].Start);
            Assert.AreEqual(newDate(2, 0), periods[0].End);
            Assert.AreEqual(newDate(2, 0), periods[1].Start);
            Assert.AreEqual(newDate(3, 0), periods[1].End);
            Assert.AreEqual(newDate(3, 0), periods[2].Start);
            Assert.AreEqual(newDate(3, 17), periods[2].End);
        }

        [Test]
        public void Days_1Month()
        {
            DateTime newDate(int month, int day) => new DateTime(2016, month, day);

            var period = Period.New(newDate(3, 1), newDate(4, 1));

            var periods = period.Days().ToList();
            Assert.AreEqual(31, periods.Count);

            var first = periods.First();
            Assert.AreEqual(newDate(3, 1), first.Start);
            var last = periods.Last();
            Assert.AreEqual(newDate(4, 1), last.End);
        }

        [Test]
        public void Duration()
        {
            var dt1 = new DateTime(2015, 1, 1);
            var dt2 = new DateTime(2015, 1, 3);

            var period = Period.New(dt1, dt2);

            Assert.AreEqual(dt2 - dt1, period.Duration);
        }

        [Test]
        public void Except_Equal()
        {
            var dt1 = new DateTime(2015, 1, 1);
            var dt2 = new DateTime(2015, 1, 3);

            var period1 = Period.New(dt1, dt2);
            var period2 = Period.New(dt1, dt2);

            var diff = period1.Except(period2).ToList();

            Assert.AreEqual(0, diff.Count);
        }

        [Test]
        public void Except_LeftStartSmaller_EndEqual()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 3));

            var diff = period1.Except(period2).ToList();

            Assert.AreEqual(1, diff.Count);
            var period = diff.First();
            Assert.AreEqual(period.Start, new DateTime(2015, 1, 1));
            Assert.AreEqual(period.End, new DateTime(2015, 1, 2));
        }

        [Test]
        public void Except_LeftStartGreater_EndEqual()
        {
            var period1 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));

            var diff = period1.Except(period2).ToList();

            Assert.AreEqual(0, diff.Count);
        }

        [Test]
        public void Except_StartEqual_LeftEndSmaller()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 2));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));
        
            var diff = period1.Except(period2).ToList();

            Assert.AreEqual(0, diff.Count);
        }

        [Test]
        public void Except_StartEqual_LeftEndGreater()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 2));

            var diff = period1.Except(period2).ToList();

            Assert.AreEqual(1, diff.Count);
            var period = diff.First();
            Assert.AreEqual(period.Start, new DateTime(2015, 1, 2));
            Assert.AreEqual(period.End, new DateTime(2015, 1, 3));
        }

        [Test]
        public void Except_LeftStartGreater_LeftEndGreater()
        {
            var period1 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 2));

            var diff = period1.Except(period2).ToList();

            Assert.AreEqual(1, diff.Count);
            var period = diff.First();
            Assert.AreEqual(period.Start, new DateTime(2015, 1, 2));
            Assert.AreEqual(period.End, new DateTime(2015, 1, 3));
        }

        [Test]
        public void Except_LeftStartSmaller_LeftEndGreater()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 4));
            var period2 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 3));

            var diff = period1.Except(period2).ToList();

            Assert.AreEqual(2, diff.Count);
            var first = diff.First();
            Assert.AreEqual(first.Start, new DateTime(2015, 1, 1));
            Assert.AreEqual(first.End, new DateTime(2015, 1, 2));

            var second = diff.Last();
            Assert.AreEqual(second.Start, new DateTime(2015, 1, 3));
            Assert.AreEqual(second.End, new DateTime(2015, 1, 4));
        }

        [Test]
        public void Hours()
        {
            var start = new DateTime(2016, 3, 1, 23, 30, 0);
            var end = new DateTime(2016, 3, 2, 2, 45, 0);

            var sut = Period.New(start, end);
            var hours = sut.Hours().ToArray();
            Assert.AreEqual(4, hours.Length);

            var period = Period.New(start, new DateTime(2016, 3, 2, 0, 0, 0));
            Assert.AreEqual(period, hours[0]);

            period = Period.New(period.End, period.End + TimeSpan.FromHours(1));
            Assert.AreEqual(period, hours[1]);

            period = Period.New(period.End, period.End + TimeSpan.FromHours(1));
            Assert.AreEqual(period, hours[2]);

            period = Period.New(period.End, end);
            Assert.AreEqual(period, hours[3]);
        }

        [Test]
        public void Intersect_NoIntersection()
        {
            DateTime newDate(int day) => new DateTime(2015, 1, day);

            var period1 = Period.New(newDate(10), newDate(13));
            var period2 = Period.New(newDate(8), newDate(9));
            Assert.AreEqual(Opt.None<Period>(), period1.Intersect(period2));
        }

        [Test]
        public void Intersect_LeftIntersection()
        {
            DateTime newDate(int day) => new DateTime(2015, 1, day);

            var period1 = Period.New(newDate(10), newDate(13));
            var period2 = Period.New(newDate(11), newDate(14));

            Assert.AreEqual(new Period(newDate(11), newDate(13)), period1.Intersect(period2).OrThrow());
            Assert.AreEqual(new Period(newDate(11), newDate(13)), period2.Intersect(period1).OrThrow());
        }

        [Test]
        public void Intersect_LeftIntersection_RhsStartEqualsLhsEnd()
        {
            DateTime newDate(int day) => new DateTime(2015, 1, day);

            var period1 = Period.New(newDate(10), newDate(13));
            var period2 = Period.New(newDate(13), newDate(14));

            Assert.AreEqual(new Period(newDate(13), newDate(13)), period1.Intersect(period2).OrThrow());
            Assert.AreEqual(new Period(newDate(13), newDate(13)), period2.Intersect(period1).OrThrow());
        }

        [Test]
        public void Intersect_RightIntersection()
        {
            DateTime newDate(int day) => new DateTime(2015, 1, day);

            var period1 = Period.New(newDate(10), newDate(13));
            var period2 = Period.New(newDate(8), newDate(12));

            Assert.AreEqual(new Period(newDate(10), newDate(12)), period1.Intersect(period2).OrThrow());
            Assert.AreEqual(new Period(newDate(10), newDate(12)), period2.Intersect(period1).OrThrow());
        }

        [Test]
        public void Intersect_RightIntersection_RhsEndEqualsLhsStart()
        {
            DateTime newDate(int day) => new DateTime(2015, 1, day);

            var period1 = Period.New(newDate(10), newDate(13));
            var period2 = Period.New(newDate(8), newDate(13));

            Assert.AreEqual(new Period(newDate(10), newDate(13)), period1.Intersect(period2).OrThrow());
            Assert.AreEqual(new Period(newDate(10), newDate(13)), period2.Intersect(period1).OrThrow());
        }

        [Test]
        public void Intersect_TotalIntersection()
        {
            DateTime newDate(int day) => new DateTime(2015, 1, day);

            var period1 = Period.New(newDate(10), newDate(13));
            var period2 = Period.New(newDate(8), newDate(14));

            Assert.AreEqual(new Period(newDate(10), newDate(13)), period1.Intersect(period2).OrThrow());
            Assert.AreEqual(new Period(newDate(10), newDate(13)), period2.Intersect(period1).OrThrow());
        }

        [Test]
        public void Intersect_WithList()
        {
            DateTime newDate(int day) => new DateTime(2015, 1, day);

            var period1 = Period.New(newDate(1), newDate(10));
            var period2 = Period.New(newDate(2), newDate(4));
            var period3 = Period.New(newDate(3), newDate(6));
            var period4 = Period.New(newDate(8), newDate(9));

            var intersected = period1.Intersect(new [] { period2, period3, period4 }).ToList();
            Assert.AreEqual(3, intersected.Count);

            Assert.AreEqual(new Period(newDate(2), newDate(4)), intersected[0]);
            Assert.AreEqual(new Period(newDate(3), newDate(6)), intersected[1]);
            Assert.AreEqual(new Period(newDate(8), newDate(9)), intersected[2]);
        }

        [Test]
        [TestCase(20, 23, 20)]
        [TestCase(20, 23, 21)]
        [TestCase(20, 23, 23)]
        public void IsBetween_ReturnsTrue(int startDay, int endDay, int checkDay)
        {
            var start = new DateTime(2013, 10, startDay);
            var end = new DateTime(2013, 10, endDay);
            var period = Period.New(start, end);

            Assert.IsTrue(period.IsBetween(new DateTime(2013, 10, checkDay)));
        }

        [Test]
        [TestCase(20, 23, 19)]
        [TestCase(20, 23, 24)]
        public void IsBetween_ReturnsFalse(int startDay, int endDay, int checkDay)
        {
            var start = new DateTime(2013, 10, startDay);
            var end = new DateTime(2013, 10, endDay);
            var period = Period.New(start, end);

            Assert.IsFalse(period.IsBetween(new DateTime(2013, 10, checkDay)));
        }

        [Test]
        public void IsBetween_List_1Overlapping()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 6));

            Assert.IsTrue(period1.IsBetween(EnumerableHelper.AsEnumerable(period2)));
        }

        [Test]
        public void IsBetween_List_2Overlapping()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 6));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));
            var period3 = Period.New(new DateTime(2015, 1, 3), new DateTime(2015, 1, 6));

            Assert.IsTrue(period1.IsBetween(EnumerableHelper.AsEnumerable(period2, period3)));
        }

        [Test]
        public void IsBetween_List_3Overlapping_2NonOverlapping()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 6));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 2));
            var period3 = Period.New(new DateTime(2015, 1, 8), new DateTime(2015, 1, 9));
            var period4 = Period.New(new DateTime(2015, 1, 4), new DateTime(2015, 1, 6));
            var period5 = Period.New(new DateTime(2015, 1, 9), new DateTime(2015, 1, 10));
            var period6 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 4));

            Assert.IsTrue(period1.IsBetween(EnumerableHelper.AsEnumerable(period2, period3, period4, period5, period6)));
        }

        [Test]
        [TestCase(10, 13, 8, 9, false)]
        [TestCase(10, 13, 9, 10, true)]
        [TestCase(10, 13, 9, 14, true)]
        [TestCase(10, 13, 11, 13, true)]
        [TestCase(10, 13, 13, 14, true)]
        [TestCase(10, 13, 14, 15, false)]
        public void IsOverlapping(int startDay1, int endDay1, int startDay2, int endDay2, bool isValid)
        {
            var period1 = Period.New(new DateTime(2015, 1, startDay1), new DateTime(2015, 1, endDay1));
            var period2 = Period.New(new DateTime(2015, 1, startDay2), new DateTime(2015, 1, endDay2));

            Assert.AreEqual(isValid, period1.IsOverlapping(period2));
        }

        [Test]
        [TestCase(20, 23, 21)]
        [TestCase(20, 23, 22)]
        public void IsWithin_ReturnsTrue(int startDay, int endDay, int checkDay)
        {
            var start = new DateTime(2013, 10, startDay);
            var end = new DateTime(2013, 10, endDay);
            var period = Period.New(start, end);

            Assert.IsTrue(period.IsWithin(new DateTime(2013, 10, checkDay)));
        }

        [Test]
        [TestCase(20, 23, 20)]
        [TestCase(20, 23, 23)]
        public void IsWithin_ReturnsFalse(int startDay, int endDay, int checkDay)
        {
            var start = new DateTime(2013, 10, startDay);
            var end = new DateTime(2013, 10, endDay);
            var period = Period.New(start, end);

            Assert.IsFalse(period.IsWithin(new DateTime(2013, 10, checkDay)));
        }

        [Test]
        public void Minutes()
        {
            var start = new DateTime(2016, 3, 1, 23, 58, 15);
            var end = new DateTime(2016, 3, 2, 0, 3, 47);

            var sut = Period.New(start, end);
            var minutes = sut.Minutes().ToArray();
            Assert.AreEqual(6, minutes.Length);

            var period = Period.New(start, new DateTime(2016, 3, 1, 23, 59, 0));
            Assert.AreEqual(period, minutes[0]);

            period = Period.New(period.End, period.End + TimeSpan.FromMinutes(1));
            Assert.AreEqual(period, minutes[1]);

            period = Period.New(period.End, period.End + TimeSpan.FromMinutes(1));
            Assert.AreEqual(period, minutes[2]);

            period = Period.New(period.End, period.End + TimeSpan.FromMinutes(1));
            Assert.AreEqual(period, minutes[3]);

            period = Period.New(period.End, period.End + TimeSpan.FromMinutes(1));
            Assert.AreEqual(period, minutes[4]);

            period = Period.New(period.End, end);
            Assert.AreEqual(period, minutes[5]);
        }

        [Test]
        public void Months_3Months()
        {
            DateTime newDate(int month, int day, int hour) => new DateTime(2015, month, day, hour, 0, 0);

            var period = Period.New(newDate(1, 1, 8), newDate(3, 5, 17));

            var periods = period.Months().ToArray();
            Assert.AreEqual(3, periods.Length);
            Assert.AreEqual(newDate(1, 1, 8), periods[0].Start);
            Assert.AreEqual(newDate(2, 1, 0), periods[0].End);
            Assert.AreEqual(newDate(2, 1, 0), periods[1].Start);
            Assert.AreEqual(newDate(3, 1, 0), periods[1].End);
            Assert.AreEqual(newDate(3, 1, 0), periods[2].Start);
            Assert.AreEqual(newDate(3, 5, 17), periods[2].End);
        }

        [Test]
        public void New_WithStartAndEndTime()
        {
            var start = new DateTime(2013, 10, 20);
            var end = new DateTime(2013, 10, 23);
            var period = Period.New(start, end);

            Assert.AreEqual(end - start, period.Duration);
            Assert.AreEqual(start, period.Start);
            Assert.AreEqual(end, period.End);
        }

        [Test]
        public void New_WithStartAndEndTime_InvalidDateTimes_ThrowsException()
        {
            var start = new DateTime(2013, 10, 20);
            var end = new DateTime(2013, 10, 19);
            Assert.Throws<ArgumentException>(() => Period.New(start, end));
        }

        [Test]
        public void New_WithStartAndDuration_DirectionForward()
        {
            var start = new DateTime(2013, 10, 20);
            var duration = TimeSpan.FromDays(3);
            var period = Period.New(start, duration, Period.Direction.Forward);

            Assert.AreEqual(duration, period.Duration);
            Assert.AreEqual(start, period.Start);
            Assert.AreEqual(new DateTime(2013, 10, 23), period.End);
        }

        [Test]
        public void New_WithStartAndDuration_DirectionBackward()
        {
            var end = new DateTime(2013, 10, 20);
            var duration = TimeSpan.FromDays(3);
            var period = Period.New(end, duration, Period.Direction.Backward);

            Assert.AreEqual(duration, period.Duration);
            Assert.AreEqual(new DateTime(2013, 10, 17), period.Start);
            Assert.AreEqual(end, period.End);
        }

        [Test]
        public void Operator_Add()
        {
            var start = new DateTime(2016, 3, 1, 8, 0, 0);
            var end = start + TimeSpan.FromHours(2);
            var sut = Period.New(start, end);
            var period = sut + TimeSpan.FromHours(2);
            var expectedEnd = end + TimeSpan.FromHours(2);
            Assert.AreEqual(new Period(start, expectedEnd), period);
        }

        [Test]
        public void Operator_Equals()
        {
            var start = new DateTime(2016, 3, 1, 8, 0, 0);
            var end = start + TimeSpan.FromHours(2);
            var p1 = Period.New(start, end);
            var p2 = Period.New(start, end);
            Assert.IsTrue(p1 == p2);
        }

        [Test]
        public void Operator_Greater()
        {
            var start = new DateTime(2016, 3, 1, 8, 0, 0);
            var end = start + TimeSpan.FromHours(2);

            //p1's end is greater than p2's end.
            var p1 = Period.New(start, end + TimeSpan.FromHours(3));
            var p2 = Period.New(start, end);
            Assert.IsTrue(p1 > p2);

            //p1's start is greater than p2's start.
            p1 = Period.New(start + TimeSpan.FromHours(1), end);
            p2 = Period.New(start, end);
            Assert.IsTrue(p1 > p2);

            //p1's start is greater than p2's start and
            //p1's end is smaller than p2's end.
            p1 = Period.New(start + TimeSpan.FromHours(1), end - TimeSpan.FromHours(1));
            p2 = Period.New(start, end);
            Assert.IsTrue(p1 > p2);
        }

        [Test]
        public void Operator_GreaterOrEqual()
        {
            var start = new DateTime(2016, 3, 1, 8, 0, 0);
            var end = start + TimeSpan.FromHours(2);

            //p1 equals p2.
            var p1 = Period.New(start, end);
            var p2 = Period.New(start, end);
            Assert.IsTrue(p1 >= p2);

            //p1's end is greater than p2's end.
            p1 = Period.New(start, end + TimeSpan.FromHours(3));
            p2 = Period.New(start, end);
            Assert.IsTrue(p1 >= p2);

            //p1's start is greater than p2's start.
            p1 = Period.New(start + TimeSpan.FromHours(1), end);
            p2 = Period.New(start, end);
            Assert.IsTrue(p1 >= p2);

            //p1's start is greater than p2's start and
            //p1's end is smaller than p2's end.
            p1 = Period.New(start + TimeSpan.FromHours(1), end - TimeSpan.FromHours(1));
            p2 = Period.New(start, end);
            Assert.IsTrue(p1 >= p2);
        }

        [Test]
        public void Operator_Less()
        {
            var start = new DateTime(2016, 3, 1, 8, 0, 0);
            var end = start + TimeSpan.FromHours(2);

            //p1's end is smaller than p2's end.
            var p1 = Period.New(start, end);
            var p2 = Period.New(start, end + TimeSpan.FromHours(3));
            Assert.IsTrue(p1 < p2);

            //p1's start is smaller than p2's start.
            p1 = Period.New(start, end);
            p2 = Period.New(start + TimeSpan.FromHours(1), end);
            Assert.IsTrue(p1 < p2);

            //p1's start is smaller than p2's start and
            //p1's end is greater than p2's end.
            p1 = Period.New(start, end);
            p2 = Period.New(start + TimeSpan.FromHours(1), end - TimeSpan.FromHours(1));
            Assert.IsTrue(p1 < p2);
        }

        [Test]
        public void Operator_LessOrEqual()
        {
            var start = new DateTime(2016, 3, 1, 8, 0, 0);
            var end = start + TimeSpan.FromHours(2);

            //p1 [-----------]
            //p2 [-----------]
            var p1 = Period.New(start, end);
            var p2 = Period.New(start, end);
            Assert.IsTrue(p1 <= p2);

            //p1 [--------]
            //p2 [-----------]
            p1 = Period.New(start, end);
            p2 = Period.New(start, end + TimeSpan.FromHours(3));
            Assert.IsTrue(p1 <= p2);

            //p1 [-----------]
            //p2    [--------]
            p1 = Period.New(start, end);
            p2 = Period.New(start + TimeSpan.FromHours(1), end);
            Assert.IsTrue(p1 <= p2);

            //p1 [-----------]
            //p2    [-----]
            p1 = Period.New(start, end);
            p2 = Period.New(start + TimeSpan.FromHours(1), end - TimeSpan.FromHours(1));
            Assert.IsTrue(p1 <= p2);

            //p1    [--------]
            //p2 [-----------]
            p1 = Period.New(start + TimeSpan.FromHours(1), end);
            p2 = Period.New(start, end);
            Assert.IsFalse(p1 <= p2);

            //p1 [-----------]
            //p2 [--------]
            p1 = Period.New(start, end);
            p2 = Period.New(start, end - TimeSpan.FromHours(1));
            Assert.IsFalse(p1 <= p2);
        }

        [Test]
        public void Operator_NotEqual()
        {
            var start = new DateTime(2016, 3, 1, 8, 0, 0);
            var end = start + TimeSpan.FromHours(2);
            var p1 = Period.New(start, end);
            var p2 = Period.New(new DateTime(2016, 3, 1, 9, 0, 0), end);
            Assert.IsTrue(p1 != p2);
        }
        [Test]
        public void SymmetricDifference_Equal()
        {
            var dt1 = new DateTime(2015, 1, 1);
            var dt2 = new DateTime(2015, 1, 3);

            var period1 = Period.New(dt1, dt2);
            var period2 = Period.New(dt1, dt2);

            var diff = period1.SymmetricDifference(period2).ToList();

            Assert.AreEqual(0, diff.Count);
        }

        [Test]
        public void SymmetricDifference_LeftStartGreater_EndEqual()
        {
            var period1 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));

            var diff = period1.SymmetricDifference(period2).ToArray();

            Assert.AreEqual(1, diff.Length);

            var first = diff.First();
            Assert.AreEqual(first.Start, new DateTime(2015, 1, 1));
            Assert.AreEqual(first.End, new DateTime(2015, 1, 2));
        }

        [Test]
        public void SymmetricDifference_LeftStartGreater_LeftEndSmaller()
        {
            var period1 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 4));

            var diff = period1.SymmetricDifference(period2).ToArray();

            Assert.AreEqual(2, diff.Length);
            var first = diff.First();
            Assert.AreEqual(first.Start, new DateTime(2015, 1, 1));
            Assert.AreEqual(first.End, new DateTime(2015, 1, 2));

            var second = diff.Last();
            Assert.AreEqual(second.Start, new DateTime(2015, 1, 3));
            Assert.AreEqual(second.End, new DateTime(2015, 1, 4));
        }

        [Test]
        public void SymmetricDifference_LeftStartSmaller_EndEqual()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 3));

            var diff = period1.SymmetricDifference(period2).ToList();

            Assert.AreEqual(1, diff.Count);
            var period = diff.First();
            Assert.AreEqual(period.Start, new DateTime(2015, 1, 1));
            Assert.AreEqual(period.End, new DateTime(2015, 1, 2));
        }

        [Test]
        public void SymmetricDifference_LeftStartSmaller_LeftEndGreater()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 4));
            var period2 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 3));

            var diff = period1.SymmetricDifference(period2).ToList();

            Assert.AreEqual(2, diff.Count);
            var first = diff.First();
            Assert.AreEqual(first.Start, new DateTime(2015, 1, 1));
            Assert.AreEqual(first.End, new DateTime(2015, 1, 2));

            var second = diff.Last();
            Assert.AreEqual(second.Start, new DateTime(2015, 1, 3));
            Assert.AreEqual(second.End, new DateTime(2015, 1, 4));
        }

        [Test]
        public void SymmetricDifference_StartEqual_LeftEndGreater()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 2));

            var diff = period1.SymmetricDifference(period2).ToList();

            Assert.AreEqual(1, diff.Count);
            var period = diff.First();
            Assert.AreEqual(period.Start, new DateTime(2015, 1, 2));
            Assert.AreEqual(period.End, new DateTime(2015, 1, 3));
        }

        [Test]
        public void SymmetricDifference_StartEqual_LeftEndSmaller()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 2));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));

            var diff = period1.SymmetricDifference(period2).ToList();

            Assert.AreEqual(1, diff.Count);
            var period = diff.First();
            Assert.AreEqual(period.Start, new DateTime(2015, 1, 2));
            Assert.AreEqual(period.End, new DateTime(2015, 1, 3));
        }

        [Test]
        public void Union_LeftStartGreater_LeftEndGreater()
        {
            var period1 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 4));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));

            var merged = period1.Union(period2);

            Assert.AreEqual(merged.OrThrow().Start, new DateTime(2015, 1, 1));
            Assert.AreEqual(merged.OrThrow().End, new DateTime(2015, 1, 4));
        }

        [Test]
        public void Union_LeftStartGreater_LeftEndSmaller()
        {
            var period1 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 4));

            var merged = period1.Union(period2);

            Assert.AreEqual(merged.OrThrow().Start, new DateTime(2015, 1, 1));
            Assert.AreEqual(merged.OrThrow().End, new DateTime(2015, 1, 4));
        }

        [Test]
        public void Union_LeftStartSmaller_LeftEndGreater()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 4));
            var period2 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 3));

            var merged = period1.Union(period2);

            Assert.AreEqual(merged.OrThrow().Start, new DateTime(2015, 1, 1));
            Assert.AreEqual(merged.OrThrow().End, new DateTime(2015, 1, 4));
        }

        [Test]
        public void Union_LeftStartSmaller_LeftEndSmaller()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 2), new DateTime(2015, 1, 4));

            var merged = period1.Union(period2);

            Assert.AreEqual(merged.OrThrow().Start, new DateTime(2015, 1, 1));
            Assert.AreEqual(merged.OrThrow().End, new DateTime(2015, 1, 4));
        }

        [Test]
        public void Union_StartEqual_LeftEndGreater()
        {
            var period1 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 3));
            var period2 = Period.New(new DateTime(2015, 1, 1), new DateTime(2015, 1, 2));

            var merged = period1.Union(period2);

            Assert.AreEqual(merged.OrThrow().Start, new DateTime(2015, 1, 1));
            Assert.AreEqual(merged.OrThrow().End, new DateTime(2015, 1, 3));
        }

        [Test]
        public void Weeks_1Month()
        {
            var start = new DateTime(2015, 7, 1, 8, 0, 0);
            var end = new DateTime(2015, 8, 2, 18, 0, 0);
            var period = Period.New(start, end);
            DateTime newDate(int month, int day) => new DateTime(2015, month, day);

            var periods = period.Weeks().ToArray();
            Assert.AreEqual(5, periods.Length);
            Assert.AreEqual(start, periods[0].Start);
            Assert.AreEqual(newDate(7,  6), periods[0].End);
            Assert.AreEqual(TimeSpan.FromDays(4) + TimeSpan.FromHours(16), periods[0].Duration);

            Assert.AreEqual(newDate(7,  6), periods[1].Start);
            Assert.AreEqual(newDate(7, 13), periods[1].End);
            Assert.AreEqual(TimeSpan.FromDays(7), periods[1].Duration);

            Assert.AreEqual(newDate(7, 13), periods[2].Start);
            Assert.AreEqual(newDate(7, 20), periods[2].End);
            Assert.AreEqual(TimeSpan.FromDays(7), periods[2].Duration);

            Assert.AreEqual(newDate(7, 20), periods[3].Start);
            Assert.AreEqual(newDate(7, 27), periods[3].End);
            Assert.AreEqual(TimeSpan.FromDays(7), periods[3].Duration);

            Assert.AreEqual(newDate(7, 27), periods[4].Start);
            Assert.AreEqual(end, periods[4].End);
            Assert.AreEqual(TimeSpan.FromDays(6) + TimeSpan.FromHours(18), periods[4].Duration);
        }

        [Test]
        public void Weeks_2MonthsWithTime()
        {
            Func<int, int, int, DateTime> newDate = (month, day, hour) => new DateTime(2015, month, day, hour, 0, 0);
            var start = newDate(7, 1, 8);
            var end = newDate(8, 4, 18);
            var period = Period.New(start, end);

            var periods = period.Weeks().ToList();
            Assert.AreEqual(6, periods.Count);
            Assert.AreEqual(start, periods[0].Start);
            Assert.AreEqual(newDate(7, 6, 0), periods[0].End);

            Assert.AreEqual(newDate(7, 6, 0), periods[1].Start);
            Assert.AreEqual(newDate(7, 13, 0), periods[1].End);

            Assert.AreEqual(newDate(7, 13, 0), periods[2].Start);
            Assert.AreEqual(newDate(7, 20, 0), periods[2].End);

            Assert.AreEqual(newDate(7, 20, 0), periods[3].Start);
            Assert.AreEqual(newDate(7, 27, 0), periods[3].End);

            Assert.AreEqual(newDate(7, 27, 0), periods[4].Start);
            Assert.AreEqual(newDate(8, 3, 0), periods[4].End);

            Assert.AreEqual(newDate(8, 3, 0), periods[5].Start);
            Assert.AreEqual(end, periods[5].End);
        }

        [Test]
        public void Years_3Years()
        {
            Func<int, int, int, DateTime> newDate = (year, month, day) => new DateTime(year, month, day, 0, 0, 0);

            var period = Period.New(newDate(2013, 1, 1).AddHours(8), newDate(2015, 7, 5).AddHours(5));

            var periods = period.Years().ToList();
            Assert.AreEqual(3, periods.Count);
            Assert.AreEqual(newDate(2013, 1, 1).AddHours(8), periods[0].Start);
            Assert.AreEqual(newDate(2014, 1, 1), periods[0].End);
            Assert.AreEqual(newDate(2014, 1, 1), periods[1].Start);
            Assert.AreEqual(newDate(2015, 1, 1), periods[1].End);
            Assert.AreEqual(newDate(2015, 1, 1), periods[2].Start);
            Assert.AreEqual(newDate(2015, 7, 5).AddHours(5), periods[2].End);
        }

        // ReSharper restore InconsistentNaming
    }
}
