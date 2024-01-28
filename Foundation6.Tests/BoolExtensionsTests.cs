using FluentAssertions;
using NUnit.Framework;
using System;

namespace Foundation
{
    [TestFixture]
    public class BoolExtensionsTests
    {
        [Test]
        public void ThrowIfFalse_Should_ThrowException_When_False()
        {
            var sut = false;
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.ThrowIfFalse());
        }

        [Test]
        public void ThrowIfFalse_Should_ReturnValue_When_True()
        {
            var sut = true;
            sut.Should().BeTrue();
        }
    }
}
