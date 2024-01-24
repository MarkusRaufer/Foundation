using FluentAssertions;
using NUnit.Framework;

namespace Foundation;

[TestFixture]
public class IfTests
{
    [Test]
    public void EagerValue_Should_ReturnElseValue_When_Is_PredicateIsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(x).Is(x => x == 1).Then(x => expected).Else(x => 20);

        result.Should().Be(20);
    }

    [Test]
    public void EagerValue_Should_ReturnThenValue_When_Is_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(x).Is(x => x == 10).Then(x => expected).Else(x => 20);

        result.Should().Be(100);
    }

    [Test]
    public void EagerTrue_Should_ReturnThenValue_When_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.True(x == 10).Then(() => expected).Else(() => 20);

        result.Should().Be(100);
    }


    [Test]
    public void EagerValue_NotNull_Should_ReturnElseValue_When_ValueIsNull()
    {
        string? value = null;
        string elseValue = "default";

        var result = If.Value(value).NotNull().Then(x => x).Else(() => elseValue);

        result.Should().Be(elseValue);
    }

    [Test]
    public void LazyValue_Should_ReturnElseValue_When_ValueIsNull()
    {
        string? x = null;
        var expected = 1;

        var result = If.Value(() => x).NotNull().Then(x => int.Parse(x)).Else(() => expected);

        result.Should().Be(expected);
    }

    [Test]
    public void LazyValue_Should_ReturnThenValue_When_ValueNotNull()
    {
        var x = "12";
        var expected = int.Parse(x);

        var result = If.Value(() => x).NotNull().Then(x => int.Parse(x)).Else(() => 1);

        result.Should().Be(expected);
    }

    [Test]
    public void LazyValue_Should_ReturnElseValue_When_PredicateIsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(() => x).Is(x => x == 1).Then(x => expected).Else(x => 20);

        result.Should().Be(20);
    }

    [Test]
    public void LazyValue_Should_ReturnThenValueInt_When_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(() => x).Is(x => x == 10).Then(x => expected).Else(x => 20);

        result.Should().Be(100);
    }

    [Test]
    public void LazyValue_Should_ReturnThenValueString_When_PredicateIsTrue()
    {
        var x = "test";

        var result = If.Value(() => x).NotNull().Then(x => x).Else(() => "default");

        result.Should().Be(x);
    }

    [Test]
    public void LazyTrue_Should_ReturnElseValue_When_PredicateIsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.True(() => x == 5).Then(() => expected).Else(() => 20);

        result.Should().Be(20);
    }

    [Test]
    public void LazyTrue_Should_ReturnThenValue_When_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.True(() => x == 10).Then(() => expected).Else(() => 20);

        result.Should().Be(100);
    }
}
