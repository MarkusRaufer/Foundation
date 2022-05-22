using NUnit.Framework;
using System;

namespace Foundation
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void ThrowIfOutOfRange_Should_ReturnValue_When_InRange()
        {
            var min = 5;
            var max = 10;

            var value = 8;
            var number = value.ThrowIfOutOfRange(() => value < min || value > max);
            Assert.AreEqual(8, number);
        }

        [Test]
        public void ThrowIfOutOfRange_Should_ThrowsAnException_When_OutOfRange()
        {
            var min = 1;
            var max = 7;
            var value = 8;

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var number = value.ThrowIfOutOfRange(() => value < min || value > max);
            });
        }

        [Test]
        public void ThrowIfOutOfRange_Should_ThrowsAnException_When_OutOfRange_UseMinMax()
        {
            var min = 1;
            var max = 7;
            var value = 8;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var number = value.ThrowIfOutOfRange(() => value < min || value > max, 1, max);
            });
        }
    }
}
