using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;

namespace Foundation
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void EndOfDay()
        {
            var dt = new DateTime(2000, 1, 2, 3, 4, 5);
            var expected = new DateTime(2000, 1, 3, 0, 0, 0);

            var endOfDay = dt.EndOfDay();

            expected.ShouldBeEquivalentTo(endOfDay);
        }

        [Test]
        public void EndOfWeek_Should_ReturnLastDayOfMonth_When_WeekendIsAtAnotherMonthAndStartIsMonday()
        {
            var dt = new DateTime(2025, 5, 31);
            var expected = new DateTime(2025, 5, 31);

            var endOfWeek = dt.EndOfWeek(DayOfWeek.Monday);

            endOfWeek.ShouldBe(expected);
        }

        [Test]
        public void EndOfWeek_Should_Return1stDayOfNextMonth_When_WeekendIsAtAnotherMonthAndStartIsMondayAndWithinMonthIsFalse()
        {
            var dt = new DateTime(2025, 5, 31);
            var expected = new DateTime(2025, 6, 1);

            var endOfWeek = dt.EndOfWeek(DayOfWeek.Monday, withinMonth: false);

            endOfWeek.ShouldBe(expected);
        }

        [Test]
        public void EndOfWeek_Should_Return1WeekOfNextMonth_When_WeekendIsAtAnotherMonthAndStartIsMondayAndWithinMonthIsFalse()
        {
            var dt = new DateTime(2025, 6, 30);
            var expected = new DateTime(2025, 7, 6);

            var endOfWeek = dt.EndOfWeek(DayOfWeek.Monday, withinMonth: false);

            endOfWeek.ShouldBe(expected);
        }

        [Test]
        public void EndOfWeek_Should_ReturnLastDayOfMonth_When_WeekendIsAtAnotherMonthAndStartIsSunday()
        {
            var dt = new DateTime(2025, 5, 31);
            var expected = new DateTime(2025, 5, 31);

            var endOfWeek = dt.EndOfWeek(DayOfWeek.Sunday);

            endOfWeek.ShouldBe(expected);
        }

        [Test]
        public void EndOfWeek_Should_ReturnTheThe7thDay_When_DateIsStartOfWeekStartIsSunday()
        {
            var dt = new DateTime(2025, 5, 4);
            var expected = new DateTime(2025, 5, 10);

            var endOfWeek = dt.EndOfWeek(DayOfWeek.Sunday);

            endOfWeek.ShouldBe(expected);
        }

        [Test]
        public void EndOfWeek_ShouldReturnSameDay_When_DateIsWeekendAndStartIsSunday()
        {
            var dt = new DateTime(2025, 5, 3);
            var expected = new DateTime(2025, 5, 3);

            var endOfWeek = dt.EndOfWeek(DayOfWeek.Sunday);

            endOfWeek.ShouldBe(expected);
        }

        [Test]
        public void EndOfWeek_ShouldReturnSameDay_When_DateIsWeekendAndStartIsMonday()
        {
            var dt = new DateTime(2025, 5, 4);
            var expected = new DateTime(2025, 5, 4);

            var endOfWeek = dt.EndOfWeek(DayOfWeek.Monday);

            endOfWeek.ShouldBe(expected);
        }

        [Test]
        public void EndOfWeek_ShouldReturnNextDay_When_DateIsOneDayBeforeWeekendAndStartIsMonday()
        {
            var dt = new DateTime(2025, 5, 3);
            var expected = new DateTime(2025, 5, 4);

            var endOfWeek = dt.EndOfWeek(DayOfWeek.Monday);

            endOfWeek.ShouldBe(expected);
        }

        [Test]
        public void EndOfYear()
        {
            var dt = new DateTime(2013, 1, 1, 8, 0, 0);
            var expected = new DateTime(2014, 1, 1, 0, 0, 0);

            var endOfDay = dt.EndOfYear();

            expected.ShouldBeEquivalentTo(endOfDay);
        }

        [Test]
        public void WeeksOfMonth()
        {
            // Arrange
            var dt = new DateTime(2025, 5, 3, 0, 0, 0, DateTimeKind.Utc);

            // Act
            var weeks = dt.WeeksOfMonth().ToArray();

            // Assert
            weeks.Length.ShouldBe(5);
            {
                var week = weeks[0];
                week.Start.ShouldBe(getDate(1));
                week.End.ShouldBe(getDate(4));
            }
            {
                var week = weeks[1];
                week.Start.ShouldBe(getDate(5));
                week.End.ShouldBe(getDate(11));
            }
            {
                var week = weeks[2];
                week.Start.ShouldBe(getDate(12));
                week.End.ShouldBe(getDate(18));
            }
            {
                var week = weeks[3];
                week.Start.ShouldBe(getDate(19));
                week.End.ShouldBe(getDate(25));
            }
            {
                var week = weeks[4];
                week.Start.ShouldBe(getDate(26));
                week.End.ShouldBe(getDate(31));
            }

            static DateTime getDate(int day) => new DateTime(2025, 5, day);
        }
    }
}
