using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation
{
    [TestFixture]
    public class DayOfWeekHelperTests
    {
        [Test]
        public void AllDaysOfWeek_Should_ReturnAllDaysOfWeek_StartedFromMonday_When_Start_Monday()
        {
            var days = DayOfWeekHelper.AllDaysOfWeek(DayOfWeek.Monday).ToArray();

            Assert.AreEqual(7, days.Length);
            Assert.AreEqual(DayOfWeek.Monday, days[0]);
            Assert.AreEqual(DayOfWeek.Tuesday, days[1]);
            Assert.AreEqual(DayOfWeek.Wednesday, days[2]);
            Assert.AreEqual(DayOfWeek.Thursday, days[3]);
            Assert.AreEqual(DayOfWeek.Friday, days[4]);
            Assert.AreEqual(DayOfWeek.Saturday, days[5]);
            Assert.AreEqual(DayOfWeek.Sunday, days[6]);
        }

        [Test]
        public void AllDaysOfWeek_Should_ReturnAllDaysOfWeek_StartedFromSunday_When_Start_Sunday()
        {
            var days = DayOfWeekHelper.AllDaysOfWeek().ToArray();

            Assert.AreEqual(7, days.Length);
            Assert.AreEqual(DayOfWeek.Sunday, days[0]);
            Assert.AreEqual(DayOfWeek.Monday, days[1]);
            Assert.AreEqual(DayOfWeek.Tuesday, days[2]);
            Assert.AreEqual(DayOfWeek.Wednesday, days[3]);
            Assert.AreEqual(DayOfWeek.Thursday, days[4]);
            Assert.AreEqual(DayOfWeek.Friday, days[5]);
            Assert.AreEqual(DayOfWeek.Saturday, days[6]);
        }

        [Test]
        public void CycleDaysOfWeek_Should_Returns6Days_StartedFromWednesday_When_Start_Wednesday_Take6()
        {
            var numberOfDays = 6;
            var days = DayOfWeekHelper.CycleDaysOfWeek(DayOfWeek.Wednesday)
                                      .Take(numberOfDays)
                                      .ToArray();

            Assert.AreEqual(numberOfDays, days.Length);
            Assert.AreEqual(DayOfWeek.Wednesday, days[0]);
            Assert.AreEqual(DayOfWeek.Thursday, days[1]);
            Assert.AreEqual(DayOfWeek.Friday, days[2]);
            Assert.AreEqual(DayOfWeek.Saturday, days[3]);
            Assert.AreEqual(DayOfWeek.Sunday, days[4]);
            Assert.AreEqual(DayOfWeek.Monday, days[5]);
        }
    }
}
