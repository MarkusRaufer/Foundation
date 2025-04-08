using NUnit.Framework;
using Shouldly;
using System;

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
        public void EndOfYear()
        {
            var dt = new DateTime(2013, 1, 1, 8, 0, 0);
            var expected = new DateTime(2014, 1, 1, 0, 0, 0);

            var endOfDay = dt.EndOfYear();

            expected.ShouldBeEquivalentTo(endOfDay);
        }
    }
}
