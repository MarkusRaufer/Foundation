using FluentAssertions;
using NUnit.Framework;

namespace Foundation;

[TestFixture]
public class MoneyTests
{
    [Test]
    public void CompareTo_Should_ReturnGreater_When_FirstAmountIsGreater()
    {
        var name = "US";
        var amount1 = 20M;
        var amount2 = 10M;

        var sut1 = Money.New(name, amount1);
        var sut2 = Money.New(name, amount2);

        {
            var cmp = sut1.CompareTo(sut2);

            cmp.Should().Be(1);
        }
        {
            var greater = sut1 > sut2;
            greater.Should().BeTrue();
        }

    }

    [Test]
    public void CompareTo_Should_ReturnGreater_When_RegionInfosAreDifferent()
    {
        var name1 = "US";
        var name2 = "de-de";
        var amount = 20M;

        var sut1 = Money.New(name1, amount);
        var sut2 = Money.New(name2, amount);

        {
            var cmp = sut1.CompareTo(sut2);

            cmp.Should().Be(1);
        }
        {
            var greater = sut1 > sut2;
            greater.Should().BeTrue();
        }
    }

    [Test]
    public void CompareTo_Should_ReturnSmaller_When_FirstAmountIsSmaller()
    {
        var name = "US";
        var amount1 = 10M;
        var amount2 = 20M;

        var sut1 = Money.New(name, amount1);
        var sut2 = Money.New(name, amount2);

        {
            var cmp = sut1.CompareTo(sut2);

            cmp.Should().Be(-1);
        }
        {
            var smaller = sut1 < sut2;
            smaller.Should().BeTrue();
        }
    }

    [Test]
    public void CompareTo_Should_ReturnZero_When_BothAreSame()
    {
        var name = "US";
        var amount = 10M;

        var sut1 = Money.New(name, amount);
        var sut2 = Money.New(name, amount);

        {
            var cmp = sut1.CompareTo(sut2);

            cmp.Should().Be(0);
        }
        {
            var eq = sut1 <= sut2;
            eq.Should().BeTrue();
        }
        {
            var eq = sut1 >= sut2;
            eq.Should().BeTrue();
        }
    }


    [Test]
    public void Equals_Should_ReturnFalse_When_AmountIsDifferent()
    {
        var name = "US";
        var amount1 = 10M;
        var amount2 = 20M;

        var sut1 = Money.New(name, amount1);
        var sut2 = Money.New(name, amount2);

        sut1.Equals(sut2).Should().BeFalse();

        {
            var eq = sut1 == sut2;
            eq.Should().BeFalse();
        }
        {
            var eq = sut1 != sut2;
            eq.Should().BeTrue();
        }
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_RegionInfoIsDifferent()
    {
        var name1 = "US";
        var name2 = "de-de";
        var amount = 10M;

        var sut1 = Money.New(name1, amount);
        var sut2 = Money.New(name2, amount);
        
        sut1.Equals(sut2).Should().BeFalse();
        {
            var eq = sut1 == sut2;
            eq.Should().BeFalse();
        }
        {
            var eq = sut1 != sut2;
            eq.Should().BeTrue();
        }
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_BothAreEqual()
    {
        var name = "US";
        var amount = 123.45M;

        var sut1 = Money.New(name, amount);
        var sut2 = Money.New(name, amount);

        sut1.Equals(sut2).Should().BeTrue();
        {
            var eq = sut1 == sut2;
            eq.Should().BeTrue();
        }
        {
            var eq = sut1 != sut2;
            eq.Should().BeFalse();
        }
    }

    [Test]
    public void New_Should_ReturnMoney_When_NameIsUsed()
    {
        var name = "US";
        var amount = 123.45M;

        var sut = Money.New(name, amount);

        sut.RegionInfo.Name.Should().Be(name);
        sut.Amount.Should().Be(amount);
    }

}
