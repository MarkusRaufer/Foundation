using NUnit.Framework;
using System;

namespace Foundation
{
    [TestFixture]
    public class TimeSpanTests
    {
        [Test]
        public void ToIso8601PeriodString_Should_ReturnIso8601String_When_NegativeTimeSpan()
        {
            var start = new DateTime(2015, 1, 1);
            var end = new DateTime(2016, 2, 3, 4, 5, 6, 789);
            var ts = start - end;

            var iso = ts.ToIso8601Period();
            Assert.AreEqual("-P398DT4H5M6S789F", iso);
        }

        [Test]
        public void ToIso8601PeriodString_Should_ReturnIso8601String_When_PositiveTimeSpan()
        {
            var start = new DateTime(2015, 1, 1);
            var end = new DateTime(2016, 2, 3, 4, 5, 6, 789);
            var ts = end - start;

            var iso = ts.ToIso8601Period();
            Assert.AreEqual("P398DT4H5M6S789F", iso);
        }
    }
}
