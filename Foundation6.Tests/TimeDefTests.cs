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
            var month = 7;
            var day = 18;

            //2016.07.18
            var sut = TimeDef.FromDateOnly(new DateOnly(year, month, day));

            var flattened = new TimeDefFlattener().Flatten(sut)
                                                  .ToArray();

            var tdYear = flattened.OfType<TimeDef.Year>().Single();
            var tdMonth = flattened.OfType<TimeDef.Month>().Single();
            var tdDay = flattened.OfType<TimeDef.Day>().Single();

            var ands = flattened.OfType<TimeDef.And>().ToArray();
            Assert.AreEqual(2, ands.Length);

            var rootAnd = ands.First(a => a.Lhs is TimeDef.And);
            var and = (TimeDef.And)rootAnd.Lhs;
            
            Assert.AreEqual(and.Lhs, tdYear);
            Assert.AreEqual(and.Rhs, tdMonth);
            Assert.AreEqual(rootAnd.Rhs, tdDay);
        }

        [Test]
        public void Equals_Should_Return_When_Compare_Same_And()
        {
            var year = 2016;
            var month = Month.Jul;
            var day = 18;

            //2016.07.18
            var sut1 = new TimeDef.And(
                        new TimeDef.And(
                            new TimeDef.Year(new[] { year }),
                            new TimeDef.Month(new[] { month })),
                        new TimeDef.Day(new[] { day }));

            //2016.07.18
            var sut2 = new TimeDef.And(
                        new TimeDef.And(
                            new TimeDef.Year(new[] { year }),
                            new TimeDef.Month(new[] { month })),
                        new TimeDef.Day(new[] { day }));

            var result = sut1.Equals(sut2);
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_And_HasSameValue()
        {
            {
                // day of month: 2 && 3
                var sut1 = new TimeDef.And(new TimeDef.Day(new[] { 2 }), new TimeDef.Day(new[] { 3 }));
                var sut2 = new TimeDef.And(new TimeDef.Day(new[] { 2 }), new TimeDef.Day(new[] { 3 }));

                Assert.IsTrue(sut1.Equals(sut2));

            }
            {
                TimeDef sut1 = new TimeDef.And(new TimeDef.Day(new[] { 2 }), new TimeDef.Day(new[] { 3 }));
                TimeDef sut2 = new TimeDef.And(new TimeDef.Day(new[] { 2 }), new TimeDef.Day(new[] { 3 }));

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_DateSpan_HasSameValue()
        {
            var from = new DateOnly(2015, 1, 1);
            var to = new DateOnly(2015, 3, 1);

            {
                var sut1 = new TimeDef.DateSpan(from, to);
                var sut2 = new TimeDef.DateSpan(from, to);

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);

                Assert.IsTrue(sut2.Equals(sut1));
                Assert.IsTrue(sut2 == sut1);
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
                var sut1 = new TimeDef.Day(new[] { 2 });
                var sut2 = new TimeDef.Day(new[] { 2 });

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
            {
                TimeDef sut1 = new TimeDef.Day(new[] { 2 });
                TimeDef sut2 = new TimeDef.Day(new[] { 2 });

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
            var days2 = new TimeDef.Day(new[] { 2 });
            var days3 = new TimeDef.Day(new[] { 3 });

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
                //hour: 08:13
                var sut1 = new TimeDef.Hour(new[] { 8, 13 });
                var sut2 = new TimeDef.Hour(new[] { 8, 13 });

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
            {
                TimeDef sut1 = new TimeDef.Hour(new[] { 8, 13 });
                TimeDef sut2 = new TimeDef.Hour(new[] { 8, 13 });

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Hours_HasSameValue()
        {
            {
                //8 hours
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
                //minute: hour:15, hour:45
                var sut1 = new TimeDef.Minute(new[] { 15, 45 });
                var sut2 = new TimeDef.Minute(new[] { 15, 45 });

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
            {
                TimeDef sut1 = new TimeDef.Minute(new[] { 15, 45 });
                TimeDef sut2 = new TimeDef.Minute(new[] { 15, 45 });

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Minutes_HasSameValue()
        {
            {
                //30 minutes
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
                //includes April, July
                var sut1 = new TimeDef.Month(new[] { Month.Apr, Month.Jul });
                var sut2 = new TimeDef.Month(new[] { Month.Apr, Month.Jul });

                var validator = new TimeDefPeriodValidator();

                var start = new DateTime(2022, 6, 1);
                var end = new DateTime(2022, 6, 30);

                //include Monday and Wednesday
                var weekday = new TimeDef.Weekday(new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday });

                //include days 2022.06.01, 2022.06.30
                var month = new TimeDef.DateSpan(new DateOnly(2022, 6, 1), new DateOnly(2022, 6, 30));
                var and = new TimeDef.And(month, weekday);

                var valid = validator.Validate(and, Period.New(start, end));

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2); 
            }
            {
                TimeDef sut1 = new TimeDef.Month(new[] { Month.Apr, Month.Jul });
                TimeDef sut2 = new TimeDef.Month(new[] { Month.Apr, Month.Jul });

                Assert.IsTrue(sut1.Equals(sut2));
                Assert.IsTrue(sut1 == sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Months_HasSameValue()
        {
            var sut1 = new TimeDef.Months(6);
            var sut2 = new TimeDef.Months(6);

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut2.Equals(sut1));
            Assert.IsTrue(sut1 == sut2);
            Assert.IsTrue(sut2 == sut1);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Not_HasSameValue()
        {
            var day = new TimeDef.Day(new[] { 2 });

            var sut1 = new TimeDef.Not(day);
            var sut2 = new TimeDef.Not(day);

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut2.Equals(sut1));

            Assert.IsTrue(sut1 == sut2);
            Assert.IsTrue(sut2 == sut1);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Or_HasSameValue()
        {
            var day2 = new TimeDef.Day(new[] { 2 });
            var day3 = new TimeDef.Day(new[] { 3 });

            var sut1 = new TimeDef.Or(day2, day3);
            var sut2 = new TimeDef.Or(day2, day3);

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut2.Equals(sut1));
            Assert.IsTrue(sut1 == sut2);
            Assert.IsTrue(sut2 == sut1);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Timespan_HasSameValue()
        {
            var from = new TimeOnly(hour: 12, minute: 0);
            var to = new TimeOnly(hour: 12, minute: 0);

            var sut1 = new TimeDef.Timespan(from, to);
            var sut2 = new TimeDef.Timespan(from, to);

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut2.Equals(sut1));
            Assert.IsTrue(sut1 == sut2);
            Assert.IsTrue(sut2 == sut1);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Union_HasSameValue()
        {
            var day2 = new TimeDef.Day(new[] { 2 });
            var day3 = new TimeDef.Day(new[] { 3 });

            var sut1 = new TimeDef.Union(day2, day3);
            var sut2 = new TimeDef.Union(day2, day3);

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut2.Equals(sut1));
            Assert.IsTrue(sut1 == sut2);
            Assert.IsTrue(sut2 == sut1);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Weekday_HasSameValue()
        {
            var sut1 = new TimeDef.Weekday(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday });
            var sut2 = new TimeDef.Weekday(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday });

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut2.Equals(sut1));
            Assert.IsTrue(sut1 == sut2);
            Assert.IsTrue(sut2 == sut1);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_WeekOfMonth_HasSameValue()
        {
            var sut1 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, new[] { 2 });
            var sut2 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, new[] { 2 });

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut2.Equals(sut1));
            Assert.IsTrue(sut1 == sut2);
            Assert.IsTrue(sut2 == sut1);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Weeks_HasSameValue()
        {
            var sut1 = new TimeDef.Weeks(3, DayOfWeek.Monday);
            var sut2 = new TimeDef.Weeks(3, DayOfWeek.Monday);

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut2.Equals(sut1));
            Assert.IsTrue(sut1 == sut2);
            Assert.IsTrue(sut2 == sut1);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Year_HasSameValue()
        {
            var sut1 = new TimeDef.Year(new[] { 2015, 2016 });
            var sut2 = new TimeDef.Year(new[] { 2015, 2016 });

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut2.Equals(sut1));
            Assert.IsTrue(sut1 == sut2);
            Assert.IsTrue(sut2 == sut1);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_Years_HasSameValue()
        {
            var sut1 = new TimeDef.Years(3);
            var sut2 = new TimeDef.Years(3);

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut2.Equals(sut1));
            Assert.IsTrue(sut1 == sut2);
            Assert.IsTrue(sut2 == sut1);
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_And_HasSameValue()
        {
            var sut1 = new TimeDef.And(new TimeDef.Day(new[] { 2 }), new TimeDef.Day(new[] { 3 }));
            var sut2 = new TimeDef.And(new TimeDef.Day(new[] { 2 }), new TimeDef.Day(new[] { 3 }));

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_ComplexTimeDef_HasSameValue()
        {
            var year = 2016;
            var month = Month.Jul;
            var day = 18;

            var sut1 = new TimeDef.And(
                        new TimeDef.And(
                            new TimeDef.Year(new[] { year }),
                            new TimeDef.Month(new[] { month })),
                        new TimeDef.Day(new[] { day }));

            var sut2 = TimeDef.FromDateOnly(new DateOnly(year, (int)month, day));

            var result = sut1.GetHashCode().Equals(sut2.GetHashCode());
            Assert.IsTrue(result);
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
                var sut1 = new TimeDef.Day(new[] { 2 });
                var sut2 = new TimeDef.Day(new[] { 2 });

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode()); 
            }
            {
                TimeDef sut1 = new TimeDef.Day(new[] { 2 });
                TimeDef sut2 = new TimeDef.Day(new[] { 2 });

                Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
            }
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Days_HasSameValue()
        {
            {
                var sut1 = new TimeDef.Days(2);
                var sut2 = new TimeDef.Days(2);

                Assert.AreEqual(sut1, sut2); 
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
            var days2 = new TimeDef.Day(new[] { 2 });
            var days3 = new TimeDef.Day(new[] { 3 });

            {
                var sut1 = new TimeDef.Difference(days2, days3);
                var sut2 = new TimeDef.Difference(days2, days3);

                Assert.AreEqual(sut1, sut2); 
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
            var sut1 = new TimeDef.Hour(new[] { 8, 13 });
            var sut2 = new TimeDef.Hour(new[] { 8, 13 });

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Hours_HasSameValue()
        {
            var sut1 = new TimeDef.Hours(8);
            var sut2 = new TimeDef.Hours(8);

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Minute_HasSameValue()
        {
            var sut1 = new TimeDef.Minute(new[] { 15, 45 });
            var sut2 = new TimeDef.Minute(new[] { 15, 45 });

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Minutes_HasSameValue()
        {
            var sut1 = new TimeDef.Minutes(30);
            var sut2 = new TimeDef.Minutes(30);

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Month_HasSameValue()
        {
            var sut1 = new TimeDef.Month(new[] { Month.Apr, Month.Jul });
            var sut2 = new TimeDef.Month(new[] { Month.Apr, Month.Jul });

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Months_HasSameValue()
        {
            var sut1 = new TimeDef.Months(6);
            var sut2 = new TimeDef.Months(6);

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Not_HasSameValue()
        {
            var day = new TimeDef.Day(new[] { 2 });

            var sut1 = new TimeDef.Not(day);
            var sut2 = new TimeDef.Not(day);

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Or_HasSameValue()
        {
            var day2 = new TimeDef.Day(new[] { 2 });
            var day3 = new TimeDef.Day(new[] { 3 });

            var sut1 = new TimeDef.Or(day2, day3);
            var sut2 = new TimeDef.Or(day2, day3);

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Timespan_HasSameValue()
        {
            var from = new TimeOnly(hour: 12, minute: 0);
            var to = new TimeOnly(hour: 12, minute: 0);

            var sut1 = new TimeDef.Timespan(from, to);
            var sut2 = new TimeDef.Timespan(from, to);

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Union_HasSameValue()
        {
            var day2 = new TimeDef.Day(new[] { 2 });
            var day3 = new TimeDef.Day(new[] { 3 });

            var sut1 = new TimeDef.Union(day2, day3);
            var sut2 = new TimeDef.Union(day2, day3);

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Weekday_HasSameValue()
        {
            var sut1 = new TimeDef.Weekday(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday });
            var sut2 = new TimeDef.Weekday(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday });

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_WeekOfMonth_HasSameValue()
        {
            var sut1 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, new[] { 2 });
            var sut2 = new TimeDef.WeekOfMonth(DayOfWeek.Monday, new[] { 2 });

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Weeks_HasSameValue()
        {
            var sut1 = new TimeDef.Weeks(3, DayOfWeek.Monday);
            var sut2 = new TimeDef.Weeks(3, DayOfWeek.Monday);

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Year_HasSameValue()
        {
            var sut1 = new TimeDef.Year(new[] { 2015, 2016 });
            var sut2 = new TimeDef.Year(new[] { 2015, 2016 });

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_Should_ReturnSameHashCode_When_Years_HasSameValue()
        {
            var sut1 = new TimeDef.Years(3);
            var sut2 = new TimeDef.Years(3);

            Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }
    }
}
