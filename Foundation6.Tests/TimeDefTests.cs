using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation;

[TestFixture]
public class TimeDefTests
{
    [Test]
    public void Equals_Should_Return_When_Compare_Same_And()
    {
        var year = 2016;
        var month = Month.Jul;
        var day = 18;

        //2016.07.18
        var sut1 = new TimeDef.And(
                    new TimeDef.And(
                        TimeDef.FromYear(year),
                        TimeDef.FromMonth(month)),
                    TimeDef.FromDay(day));

        //2016.07.18
        var sut2 = TimeDef.ChainByAnd(TimeDef.FromYear(year), 
                                      TimeDef.FromMonth(month),
                                      TimeDef.FromDay(day));

        Assert.IsTrue(sut1.Equals(sut2));
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_And_HasSameValue()
    {
        // day of month: 2 && 3
        var sut1 = new TimeDef.And(new TimeDef.Day(new[] { 2 }), new TimeDef.Day(new[] { 3 }));
        var sut2 = new TimeDef.And(TimeDef.FromDay(2), TimeDef.FromDay(3));

        Assert.IsTrue(sut1.Equals(sut2));
        Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
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
        //hour: 08:13
        var sut1 = new TimeDef.Hour(new[] { 8, 13 });
        var sut2 = new TimeDef.Hour(new[] { 8, 13 });

        Assert.IsTrue(sut1.Equals(sut2));
        Assert.IsTrue(sut2.Equals(sut1));
        Assert.IsTrue(sut1 == sut2);
        Assert.IsTrue(sut2 == sut1);
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_Hours_HasSameValue()
    {
        //8 hours
        var sut1 = new TimeDef.Hours(8);
        var sut2 = new TimeDef.Hours(8);

        Assert.IsTrue(sut1.Equals(sut2));
        Assert.IsTrue(sut2.Equals(sut1));
        Assert.IsTrue(sut1 == sut2);
        Assert.IsTrue(sut2 == sut1);
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_Minute_HasSameValue()
    {
        //minute: hour:15, hour:45
        var sut1 = new TimeDef.Minute(new[] { 15, 45 });
        var sut2 = new TimeDef.Minute(new[] { 15, 45 });

        Assert.IsTrue(sut1.Equals(sut2));
        Assert.IsTrue(sut2.Equals(sut1));
        Assert.IsTrue(sut1 == sut2);
        Assert.IsTrue(sut2 == sut1);
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_Minutes_HasSameValue()
    {
        //30 minutes
        var sut1 = new TimeDef.Minutes(30);
        var sut2 = new TimeDef.Minutes(30);

        Assert.IsTrue(sut1.Equals(sut2));
        Assert.IsTrue(sut2.Equals(sut1));
        Assert.IsTrue(sut1 == sut2);
        Assert.IsTrue(sut2 == sut1);
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_Month_HasSameValue()
    {
        {
            //includes April, July
            var sut1 = new TimeDef.Month(new[] { Month.Apr, Month.Jul });
            var sut2 = TimeDef.FromMonth(Month.Apr, Month.Jul);

            var validator = new TimeDefPeriodValidator();

            var start = new DateTime(2022, 6, 1);
            var end = new DateTime(2022, 6, 30);

            //include Monday and Wednesday
            var weekday = TimeDef.FromWeekday(DayOfWeek.Monday, DayOfWeek.Wednesday);

            //include days 2022.06.01, 2022.06.30
            var month = new TimeDef.DateSpan(From: new DateOnly(2022, 6, 1), To: new DateOnly(2022, 6, 30));
            var and = new TimeDef.And(month, weekday);

            var valid = validator.IsValid(and, Period.New(start, end));

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut1 == sut2); 
        }
        {
            TimeDef sut1 = TimeDef.FromMonth(Month.Apr, Month.Jul);
            TimeDef sut2 = TimeDef.FromMonth(Month.Apr, Month.Jul);

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
        var day2 = TimeDef.FromDay(2);
        var day3 = TimeDef.FromDay(3);

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
        var sut1 = TimeDef.FromWeekday(DayOfWeek.Monday, DayOfWeek.Wednesday);
        var sut2 = TimeDef.FromWeekday(DayOfWeek.Monday, DayOfWeek.Wednesday);

        Assert.IsTrue(sut1.Equals(sut2));
        Assert.IsTrue(sut2.Equals(sut1));
        Assert.IsTrue(sut1 == sut2);
        Assert.IsTrue(sut2 == sut1);
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_WeekOfMonth_HasSameValue()
    {
        var sut1 = TimeDef.FromWeekOfMonth(DayOfWeek.Monday, 2);
        var sut2 = TimeDef.FromWeekOfMonth(DayOfWeek.Monday, 2);

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
        var sut1 = TimeDef.FromYear(2015, 2016);
        var sut2 = TimeDef.FromYear(2015, 2016);

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
    public void FromDate_Should_ReturnTimeDef_When_ArgumentIsDateOnly()
    {
        {
            var sut = TimeDef.FromDate(2022, 3, 9);
            assert(sut);
        }
        {
            var sut = TimeDef.FromDate(2022, Month.Mar, 9);
            assert(sut);
        }

        void assert(TimeDef td)
        {
            var flattened = new TimeDefFlattener().Flatten(td)
                                                  .ToArray();

            var tdYear = flattened.OfType<TimeDef.Year>().Single();
            var tdMonth = flattened.OfType<TimeDef.Month>().Single();
            var tdDay = flattened.OfType<TimeDef.Day>().Single();

            var ands = flattened.OfType<TimeDef.And>().ToArray();
            Assert.AreEqual(2, ands.Length);

            {
                var and = ands[0];
                Assert.AreEqual(and.Lhs, ands[1]);
                Assert.AreEqual(and.Rhs, tdDay);
            }
            {
                var and = ands[1];
                Assert.AreEqual(and.Lhs, tdYear);
                Assert.AreEqual(and.Rhs, tdMonth);
            }
        }
    }

    [Test]
    public void FromDateOnly_Should_ReturnTimeDef_When_ArgumentIsDateOnly()
    {
        var sut = TimeDef.FromDateOnly(new DateOnly(2022, 3, 9));

        var flattened = new TimeDefFlattener().Flatten(sut)
                                              .ToArray();

        var tdYear = flattened.OfType<TimeDef.Year>().Single();
        var tdMonth = flattened.OfType<TimeDef.Month>().Single();
        var tdDay = flattened.OfType<TimeDef.Day>().Single();

        var ands = flattened.OfType<TimeDef.And>().ToArray();
        Assert.AreEqual(2, ands.Length);

        {
            var and = ands[0];
            Assert.AreEqual(and.Lhs, ands[1]);
            Assert.AreEqual(and.Rhs, tdDay);
        }
        {
            var and = ands[1];
            Assert.AreEqual(and.Lhs, tdYear);
            Assert.AreEqual(and.Rhs, tdMonth);
        }
    }

    [Test]
    public void FromDateTime_Should_ReturnTimeDef_When_ArgumentIsDateTime()
    {
        var sut = TimeDef.FromDateTime(new DateTime(2022, 3, 9, 8, 30, 0));

        var flattened = new TimeDefFlattener().Flatten(sut)
                                              .ToArray();

        var tdYear = flattened.OfType<TimeDef.Year>().Single();
        var tdMonth = flattened.OfType<TimeDef.Month>().Single();
        var tdDay = flattened.OfType<TimeDef.Day>().Single();
        var tdHour = flattened.OfType<TimeDef.Hour>().Single();
        var tdMinute = flattened.OfType<TimeDef.Minute>().Single();

        var ands = flattened.OfType<TimeDef.And>().ToArray();
        Assert.AreEqual(4, ands.Length);

        {
            var and = ands[0];
            Assert.AreEqual(and.Lhs, ands[1]);
            Assert.AreEqual(and.Rhs, tdMinute);
        }
        {
            var and = ands[1];
            Assert.AreEqual(and.Lhs, ands[2]);
            Assert.AreEqual(and.Rhs, tdHour);
        }
        {
            var and = ands[2];
            Assert.AreEqual(and.Lhs, ands[3]);
            Assert.AreEqual(and.Rhs, tdDay);
        }
        {
            var and = ands[3];
            Assert.AreEqual(and.Lhs, tdYear);
            Assert.AreEqual(and.Rhs, tdMonth);
        }
    }

    [Test]
    public void FromDay_Should_ReturnTimeDef_When_ArgumentIsFromFromDay()
    {
        var days = new[] { 1, 15 };
        var sut = TimeDef.FromDay(days);

        var tdDay = (TimeDef.Day)sut;

        Assert.AreEqual(new TimeDef.Day(days), tdDay);
    }

    [Test]
    public void FromHour_Should_ReturnTimeDef_When_ArgumentIsFromFromDay()
    {
        var hours = new[] { 8, 12 };
        var sut = TimeDef.FromHour(hours);

        var tdHour = (TimeDef.Hour)sut;

        Assert.AreEqual(new TimeDef.Hour(hours), tdHour);
    }

    [Test]
    public void FromMinute_Should_ReturnTimeDef_When_ArgumentIsFromFromDay()
    {
        var minutes = new[] { 15, 30, 45 };
        var sut = TimeDef.FromMinute(minutes);

        var tdMinute = (TimeDef.Minute)sut;

        Assert.AreEqual(new TimeDef.Minute(minutes), tdMinute);
    }

    [Test]
    public void FromMonth_Should_ReturnTimeDef_When_ArgumentIsFromFromDay()
    {
        var months = new[] { Month.Feb, Month.Jul, Month.Sep };
        var sut = new TimeDef.Month(months);

        Assert.AreEqual(new TimeDef.Month(months), sut);
    }

    [Test]
    public void FromTime_Should_ReturnTimeDef_When_ArgumentIsFromTimeOnly()
    {
        var hour = 8;
        var minute = 30;
        var sut = TimeDef.FromTime(hour, minute);

        var expected = new TimeDef.And(TimeDef.FromHour(hour), TimeDef.FromMinute(minute));
        var cmp = sut.CompareTo(expected);
        cmp.Should().Be(0);
    }

    [Test]
    public void FromTimeOnly_Should_ReturnTimeDef_When_ArgumentIsFromTimeOnly()
    {
        var hour = 8;
        var minute = 30;
        var sut = TimeDef.FromTimeOnly(new TimeOnly(hour, minute));

        var and = (TimeDef.And)sut;

        if (and.Lhs is not TimeDef.Hour tdHour)
        {
            Assert.Fail();
            return;
        }
        Assert.AreEqual(new TimeDef.Hour(new[] { hour }), tdHour);


        if (and.Rhs is not TimeDef.Minute tdMinute)
        {
            Assert.Fail();
            return;
        }
        Assert.AreEqual(new TimeDef.Minute(new[] { minute }), tdMinute);
    }

    [Test]
    public void FromWeekday_Should_ReturnTimeDef_When_ArgumentIsFromFromDay()
    {
        var weekdays = new[] { DayOfWeek.Monday, DayOfWeek.Wednesday };
        var sut = TimeDef.FromWeekday(weekdays);

        var tdWeekday = (TimeDef.Weekday)sut;

        Assert.AreEqual(new TimeDef.Weekday(weekdays), tdWeekday);
    }

    [Test]
    public void FromWeekOfMonth_Should_ReturnTimeDef_When_ArgumentIsFromFromDay()
    {
        var weeksOfMonth = new[] { 1, 3 };
        var sut = TimeDef.FromWeekOfMonth(DayOfWeek.Monday, weeksOfMonth);

        var tdWeekday = (TimeDef.WeekOfMonth)sut;

        Assert.AreEqual(new TimeDef.WeekOfMonth(DayOfWeek.Monday, weeksOfMonth), tdWeekday);
    }

    [Test]
    public void FromYear_Should_ReturnTimeDef_When_ArgumentIsFromFromDay()
    {
        var years = new[] { 1972, 2022 };
        var sut = TimeDef.FromYear(years);

        var tdHour = (TimeDef.Year)sut;

        Assert.AreEqual(new TimeDef.Year(years), tdHour);
    }

    [Test]
    public void GetHashCode_Should_ReturnSameHashCode_When_And_HasSameValue()
    {
        var sut1 = new TimeDef.And(new TimeDef.Month(new[] { Month.Apr }), new TimeDef.Day(new[] { 3 }));
        var sut2 = new TimeDef.And(new TimeDef.Month(new[] { Month.Apr }), new TimeDef.Day(new[] { 3 }));

        Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
    }

    [Test]
    public void GetHashCode_Should_ReturnSameHashCode_When_ComplexTimeDef_HasSameValue()
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
        var sut1 = TimeDef.FromMonth(Month.Apr, Month.Jul);
        var sut2 = TimeDef.FromMonth(Month.Apr, Month.Jul);

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
        var day = TimeDef.FromDay(2);

        var sut1 = new TimeDef.Not(day);
        var sut2 = new TimeDef.Not(day);

        Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
    }

    [Test]
    public void GetHashCode_Should_ReturnSameHashCode_When_Or_HasSameValue()
    {
        var day2 = TimeDef.FromDay(2);
        var day3 = TimeDef.FromDay(3); ;

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
        var day2 = TimeDef.FromDay(2);
        var day3 = TimeDef.FromDay(3);

        var sut1 = new TimeDef.Union(day2, day3);
        var sut2 = new TimeDef.Union(day2, day3);

        Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
    }

    [Test]
    public void GetHashCode_Should_ReturnSameHashCode_When_Weekday_HasSameValue()
    {
        var sut1 = TimeDef.FromWeekday(DayOfWeek.Monday, DayOfWeek.Wednesday);
        var sut2 = TimeDef.FromWeekday(DayOfWeek.Monday, DayOfWeek.Wednesday);

        Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
    }

    [Test]
    public void GetHashCode_Should_ReturnSameHashCode_When_WeekOfMonth_HasSameValue()
    {
        var sut1 = TimeDef.FromWeekOfMonth(DayOfWeek.Monday, 2);
        var sut2 = TimeDef.FromWeekOfMonth(DayOfWeek.Monday, 2);

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
        var sut1 = TimeDef.FromYear(2015..2016);
        var sut2 = TimeDef.FromYear(2015, 2016);

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
