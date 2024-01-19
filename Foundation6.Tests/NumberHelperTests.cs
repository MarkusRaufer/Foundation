using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation;

[TestFixture]
public class NumberHelperTests
{
    [Test]
    public void GetDigits_Should_ReturnAListOfDigits_When_UsingInt()
    {
        int number = 12345;

        var digits = NumberHelper.GetDigits(number, x => x).ToArray();

        digits.SequenceEqual(new[] { 1, 2, 3, 4, 5 }).Should().BeTrue();

    }
    [Test]
    public void GetDigits_Should_ReturnAListOfDigits_When_UsingLong()
    {
        var number = 12345L;

        var digits = NumberHelper.GetDigits(number, x => (int)x).ToArray();

        digits.SequenceEqual(new[] { 1, 2, 3, 4, 5 }).Should().BeTrue();
    }
}
