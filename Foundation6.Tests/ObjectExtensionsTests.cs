using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void ThrowIfOutOfRange_Should_ReturnValue_When_InRange()
        {
            var input = 5;
            var max = 10;

            var number = 8.ThrowIfOutOfRange(() => input <= max);
            Assert.AreEqual(8, number);
        }

        [Test]
        public void ThrowIfOutOfRange_Should_ThrowAnException_When_OutOfRange()
        {
            var input = 15;
            var max = 10;

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var number = 8.ThrowIfOutOfRange(() => input <= max);
            });
        }

        [Test]
        public void ThrowIfOutOfRange_Should_ThrowAnException_When_OutOfRange_UseMinMax()
        {
            var input = 15;
            var max = 10;

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var number = 8.ThrowIfOutOfRange(() => input <= max, 1, max);
            });
        }
    }
}
