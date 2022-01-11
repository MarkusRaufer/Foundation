using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation
{
    [TestFixture]
    public class TimeDefTests
    {
        [Test]
        public void And()
        {
            var year = 2016;
            var month = Month.Jul;
            var day = 18;

            var sut = new TimeDef.And(
                        new TimeDef.And(
                            new TimeDef.Year(year),
                            new TimeDef.Month(month)),
                        new TimeDef.Day(day));

            if (sut.Lhs is not TimeDef.And @and)
            {
                Assert.Fail();
                return;
            }

            if (@and.Lhs is not TimeDef.Year tdYear)
            {
                Assert.Fail();
                return;
            }
            Assert.AreEqual(year, tdYear.Values.Single());

            if (@and.Rhs is not TimeDef.Month tdMonth)
            {
                Assert.Fail();
                return;
            }
            Assert.AreEqual(month, tdMonth.Values.Single());

            if (sut.Rhs is not TimeDef.Day tdDay)
            {
                Assert.Fail();
                return;
            }

            Assert.AreEqual(day, tdDay.Values.Single());
        }

        [Test]
        public void Equals_Should_Return_When_Compare_Same_And()
        {
            var year = 2016;
            var month = Month.Jul;
            var day = 18;

            var sut1 = new TimeDef.And(
                        new TimeDef.And(
                            new TimeDef.Year(year),
                            new TimeDef.Month(month)),
                        new TimeDef.Day(day));

            var sut2 = new TimeDef.And(
                        new TimeDef.And(
                            new TimeDef.Year(year),
                            new TimeDef.Month(month)),
                        new TimeDef.Day(day));

            var result = sut1.Equals(sut2);
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_And_HasSameValue()
        {
            {
                var sut1 = new TimeDef.And(new TimeDef.Day(2), new TimeDef.Day(3));
                var sut2 = new TimeDef.And(new TimeDef.Day(2), new TimeDef.Day(3));

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());

            }
            {
                TimeDef sut1 = new TimeDef.And(new TimeDef.Day(2), new TimeDef.Day(3));
                TimeDef sut2 = new TimeDef.And(new TimeDef.Day(2), new TimeDef.Day(3));

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_DateTimeSpan_HasSameValue()
        {
            var from = new DateTime(2015, 1, 1);
            var to = new DateTime(2015, 3, 1);

            {
                var sut1 = new TimeDef.DateTimeSpan(from, to);
                var sut2 = new TimeDef.DateTimeSpan(from, to);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
            {
                TimeDef sut1 = new TimeDef.DateTimeSpan(from, to);
                TimeDef sut2 = new TimeDef.DateTimeSpan(from, to);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Day_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Day(2);
                var sut2 = new TimeDef.Day(2);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
            {
                TimeDef sut1 = new TimeDef.Day(2);
                TimeDef sut2 = new TimeDef.Day(2);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Days_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Days(2);
                var sut2 = new TimeDef.Days(2);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
            {
                TimeDef sut1 = new TimeDef.Days(2);
                TimeDef sut2 = new TimeDef.Days(2);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Difference_HasSameValue()
        {
            var days2 = new TimeDef.Day(2);
            var days3 = new TimeDef.Day(3);

            {
                var sut1 = new TimeDef.Difference(days2, days3);
                var sut2 = new TimeDef.Difference(days2, days3);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
            {
                TimeDef sut1 = new TimeDef.Difference(days2, days3);
                TimeDef sut2 = new TimeDef.Difference(days2, days3);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Hour_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Hour(8, 13);
                var sut2 = new TimeDef.Hour(8, 13);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
            {
                TimeDef sut1 = new TimeDef.Hour(8, 13);
                TimeDef sut2 = new TimeDef.Hour(8, 13);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Hours_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Hours(8);
                var sut2 = new TimeDef.Hours(8);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
            {
                TimeDef sut1 = new TimeDef.Hours(8);
                TimeDef sut2 = new TimeDef.Hours(8);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Minute_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Minute(15, 45);
                var sut2 = new TimeDef.Minute(15, 45);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
            {
                TimeDef sut1 = new TimeDef.Minute(15, 45);
                TimeDef sut2 = new TimeDef.Minute(15, 45);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Minutes_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Minutes(30);
                var sut2 = new TimeDef.Minutes(30);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Minutes(30);
                TimeDef sut2 = new TimeDef.Minutes(30);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Month_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Month(Month.Apr, Month.Jul);
                var sut2 = new TimeDef.Month(Month.Apr, Month.Jul);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Month(Month.Apr, Month.Jul);
                TimeDef sut2 = new TimeDef.Month(Month.Apr, Month.Jul);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Months_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Months(6);
                var sut2 = new TimeDef.Months(6);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Months(6);
                TimeDef sut2 = new TimeDef.Months(6);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Not_HasSameValue()
        {
            var day = new TimeDef.Day(2);

            {
                var sut1 = new TimeDef.Not(day);
                var sut2 = new TimeDef.Not(day);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Not(day);
                TimeDef sut2 = new TimeDef.Not(day);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Or_HasSameValue()
        {
            var day2 = new TimeDef.Day(2);
            var day3 = new TimeDef.Day(3);

            {
                var sut1 = new TimeDef.Or(day2, day3);
                var sut2 = new TimeDef.Or(day2, day3);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Or(day2, day3);
                TimeDef sut2 = new TimeDef.Or(day2, day3);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Timespan_HasSameValue()
        {
            var from = new TimeOnly(hour: 12, minute: 0);
            var to = new TimeOnly(hour: 12, minute: 0);

            {
                var sut1 = new TimeDef.Timespan(from, to);
                var sut2 = new TimeDef.Timespan(from, to);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Timespan(from, to);
                TimeDef sut2 = new TimeDef.Timespan(from, to);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Union_HasSameValue()
        {
            var day2 = new TimeDef.Day(2);
            var day3 = new TimeDef.Day(3);

            {
                var sut1 = new TimeDef.Union(day2, day3);
                var sut2 = new TimeDef.Union(day2, day3);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Union(day2, day3);
                TimeDef sut2 = new TimeDef.Union(day2, day3);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Weekday_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Weekday(DayOfWeek.Monday, DayOfWeek.Wednesday);
                var sut2 = new TimeDef.Weekday(DayOfWeek.Monday, DayOfWeek.Wednesday);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Weekday(DayOfWeek.Monday, DayOfWeek.Wednesday);
                TimeDef sut2 = new TimeDef.Weekday(DayOfWeek.Monday, DayOfWeek.Wednesday);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_WeekOfMonth_HasSameValue()
        {
            {
                var sut1 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, 2);
                var sut2 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, 2);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, 2);
                TimeDef sut2 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, 2);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Weeks_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Weeks(3, DayOfWeek.Monday);
                var sut2 = new TimeDef.Weeks(3, DayOfWeek.Monday);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Weeks(3, DayOfWeek.Monday);
                TimeDef sut2 = new TimeDef.Weeks(3, DayOfWeek.Monday);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
            
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Year_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Year(2015, 2016);
                var sut2 = new TimeDef.Year(2015, 2016);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Year(2015, 2016);
                TimeDef sut2 = new TimeDef.Year(2015, 2016);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Years_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Years(3);
                var sut2 = new TimeDef.Years(3);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Years(3);
                TimeDef sut2 = new TimeDef.Years(3);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_And_HasSameValue()
        {
            {
                var sut1 = new TimeDef.And(new TimeDef.Day(2), new TimeDef.Day(3));
                var sut2 = new TimeDef.And(new TimeDef.Day(2), new TimeDef.Day(3));

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.And(new TimeDef.Day(2), new TimeDef.Day(3));
                TimeDef sut2 = new TimeDef.And(new TimeDef.Day(2), new TimeDef.Day(3));

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_DateTimeSpan_HasSameValue()
        {
            var from = new DateTime(2015, 1, 1);
            var to = new DateTime(2015, 3, 1);

            {
                var sut1 = new TimeDef.DateTimeSpan(from, to);
                var sut2 = new TimeDef.DateTimeSpan(from, to);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.DateTimeSpan(from, to);
                TimeDef sut2 = new TimeDef.DateTimeSpan(from, to);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Day_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Day(2);
                var sut2 = new TimeDef.Day(2);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Day(2);
                TimeDef sut2 = new TimeDef.Day(2);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Days_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Days(2);
                var sut2 = new TimeDef.Days(2);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Days(2);
                TimeDef sut2 = new TimeDef.Days(2);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Difference_HasSameValue()
        {
            var days2 = new TimeDef.Day(2);
            var days3 = new TimeDef.Day(3);

            {
                var sut1 = new TimeDef.Difference(days2, days3);
                var sut2 = new TimeDef.Difference(days2, days3);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Difference(days2, days3);
                TimeDef sut2 = new TimeDef.Difference(days2, days3);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Hour_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Hour(8, 13);
                var sut2 = new TimeDef.Hour(8, 13);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Hour(8, 13);
                TimeDef sut2 = new TimeDef.Hour(8, 13);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Hours_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Hours(8);
                var sut2 = new TimeDef.Hours(8);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Hours(8);
                TimeDef sut2 = new TimeDef.Hours(8);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Minute_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Minute(15, 45);
                var sut2 = new TimeDef.Minute(15, 45);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Minute(15, 45);
                TimeDef sut2 = new TimeDef.Minute(15, 45);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Minutes_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Minutes(30);
                var sut2 = new TimeDef.Minutes(30);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Minutes(30);
                TimeDef sut2 = new TimeDef.Minutes(30);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Month_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Month(Month.Apr, Month.Jul);
                var sut2 = new TimeDef.Month(Month.Apr, Month.Jul);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Month(Month.Apr, Month.Jul);
                TimeDef sut2 = new TimeDef.Month(Month.Apr, Month.Jul);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Months_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Months(6);
                var sut2 = new TimeDef.Months(6);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Months(6);
                TimeDef sut2 = new TimeDef.Months(6);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Not_HasSameValue()
        {
            var day = new TimeDef.Day(2);

            {
                var sut1 = new TimeDef.Not(day);
                var sut2 = new TimeDef.Not(day);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Not(day);
                TimeDef sut2 = new TimeDef.Not(day);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Or_HasSameValue()
        {
            var day2 = new TimeDef.Day(2);
            var day3 = new TimeDef.Day(3);

            {
                var sut1 = new TimeDef.Or(day2, day3);
                var sut2 = new TimeDef.Or(day2, day3);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Or(day2, day3);
                TimeDef sut2 = new TimeDef.Or(day2, day3);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Timespan_HasSameValue()
        {
            var from = new TimeOnly(hour: 12, minute: 0);
            var to = new TimeOnly(hour: 12, minute: 0);

            {
                var sut1 = new TimeDef.Timespan(from, to);
                var sut2 = new TimeDef.Timespan(from, to);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Timespan(from, to);
                TimeDef sut2 = new TimeDef.Timespan(from, to);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Union_HasSameValue()
        {
            var day2 = new TimeDef.Day(2);
            var day3 = new TimeDef.Day(3);

            {
                var sut1 = new TimeDef.Union(day2, day3);
                var sut2 = new TimeDef.Union(day2, day3);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Union(day2, day3);
                TimeDef sut2 = new TimeDef.Union(day2, day3);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Weekday_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Weekday(DayOfWeek.Monday, DayOfWeek.Wednesday);
                var sut2 = new TimeDef.Weekday(DayOfWeek.Monday, DayOfWeek.Wednesday);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Weekday(DayOfWeek.Monday, DayOfWeek.Wednesday);
                TimeDef sut2 = new TimeDef.Weekday(DayOfWeek.Monday, DayOfWeek.Wednesday);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_WeekOfMonth_HasSameValue()
        {
            {
                var sut1 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, 2);
                var sut2 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, 2);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, 2);
                TimeDef sut2 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, 2);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Weeks_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Weeks(3, DayOfWeek.Monday);
                var sut2 = new TimeDef.Weeks(3, DayOfWeek.Monday);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Weeks(3, DayOfWeek.Monday);
                TimeDef sut2 = new TimeDef.Weeks(3, DayOfWeek.Monday);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Year_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Year(2015, 2016);
                var sut2 = new TimeDef.Year(2015, 2016);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Year(2015, 2016);
                TimeDef sut2 = new TimeDef.Year(2015, 2016);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Years_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Years(3);
                var sut2 = new TimeDef.Years(3);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Years(3);
                TimeDef sut2 = new TimeDef.Years(3);

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }
    }
}
