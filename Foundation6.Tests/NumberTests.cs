using FluentAssertions;
using NUnit.Framework;

namespace Foundation;

[TestFixture]
public class NumberTests
{
    [Test]
    public void CastOperator_Should_HaveCharValue_When_AssigningChar()
    {
        var expected = '7';

        Number sut = expected;

        sut.TryGet(out char number).Should().BeTrue();
        number.Should().Be(expected);
    }

    [Test]
    public void CastOperator_Should_HaveDecimalValue_When_AssigningDecimal()
    {
        var expected = 12345M;

        Number sut = expected;

        sut.TryGet(out decimal number).Should().BeTrue();
        number.Should().Be(expected);
    }

    [Test]
    public void TryGet_Should_ReturnFalse_When_AssigningInt()
    {
        var expected = 12345M;

        Number sut = expected;

        sut.TryGet(out int _).Should().BeFalse();
    }

    [Test]
    public void TryGet_Should_ReturnTrue_When_AssigningDecimal()
    {
        var expected = 12345M;

        Number sut = expected;

        sut.TryGet(out decimal number).Should().BeTrue();
        number.Should().Be(expected);
    }
}
