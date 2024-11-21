using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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

        [Test]
        public void ToOption_Should_ReturnOptionSome_When_True()
        {
            var dicionary = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
            };
            
            var option = dicionary.TryGetValue(2, out var value).ToOption(value);
            option.IsSome.Should().BeTrue();

            option.TryGet(out var foundValue).Should().BeTrue();
            foundValue.Should().Be(value);
        }
    }
}
