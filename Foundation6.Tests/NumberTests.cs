using FluentAssertions;
using NUnit.Framework;

namespace Foundation;

[TestFixture]
public class NumberTests
{
    [Test]
    public void CastOperator_Should_ShouldHaveDecimalValue_When_AssigningDecimal()
    {
        var expected = 12345M;

        Number sut = expected;

        sut.TryGet(out decimal number).Should().BeTrue();
        number.Should().Be(expected);
    }

    [Test]
    public void TryGet_Should_ShouldReturnFalse_When_AssigningInt()
    {
        var expected = 12345M;

        Number sut = expected;

        sut.TryGet(out int number).Should().BeFalse();
    }

    [Test]
    public void TryGet_Should_ShouldReturnTrue_When_AssigningDecimal()
    {
        var expected = 12345M;

        Number sut = expected;

        sut.TryGet(out decimal number).Should().BeTrue();
        number.Should().Be(expected);
    }
}
