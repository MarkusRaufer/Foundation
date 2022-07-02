using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation
{
    [TestFixture]
    public class ForTests
    {
        [Test]
        public void Returns_Should_Return5Elements_When_Take5()
        {
            var value = 1;

            var values = For.Collect(() => value++)
                            .Take(5)
                            .ToArray();

            var expected = Enumerable.Range(1, 5);
            Assert.IsTrue(expected.SequenceEqual(values));
        }

        [Test]
        public void StartAt_Should_Return5Elements_When_TakeUntilEqualsInclusive5()
        {
            var values = For.StartAt(() => 3).Collect(value => ++value) 
                                             .TakeUntil(x => x == 5, inclusive: true)
                                             .ToArray();

            var expected = Enumerable.Range(3, 3);
            Assert.IsTrue(expected.SequenceEqual(values));
        }
    }
}
