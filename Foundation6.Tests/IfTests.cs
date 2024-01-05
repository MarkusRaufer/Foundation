using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Foundation;

[TestFixture]
public class IfTests
{
    [Test]
    public void If_EagerValue_Is_Should_ReturnElseValue_When_PredicateIsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(x).Is(x => x == 1).Then(x => expected).Else(x => 20);

        result.Should().Be(20);
    }

    [Test]
    public void If_EagerValue_Is_Should_ReturnThenValue_When_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(x).Is(x => x == 10).Then(x => expected).Else(x => 20);

        result.Should().Be(100);
    }

    [Test]
    public void If_EagerValue_NotNull_Should_ReturnElseValue_When_PredicateIsFalse()
    {
        string? value = null;
        string elseValue = "default";

        var result = If.Value(value).NotNull().Then(x => x).Else(() => elseValue);

        result.Should().Be(elseValue);
    }

    [Test]
    public void If_LazyValue_Not_Should_ReturnConvertedElseValue_When_PredicateIsTrue()
    {
        string? x = null;
        var expected = 1;

        var result = If.Value(() => x).NotNull().Then(x => int.Parse(x)).Else(() => expected);

        result.Should().Be(expected);
    }

    [Test]
    public void If_LazyValue_Not_Should_ReturnConvertedThenValue_When_PredicateIsTrue()
    {
        var x = "12";
        var expected = int.Parse(x);

        var result = If.Value(() => x).NotNull().Then(x => int.Parse(x)).Else(() => 1);

        result.Should().Be(expected);
    }

    [Test]
    public void If_LazyValue_Is_Should_ReturnElseValue_When_PredicateIsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(() => x).Is(x => x == 1).Then(x => expected).Else(x => 20);

        result.Should().Be(20);
    }

    [Test]
    public void If_LazyValue_Is_Should_ReturnThenValue_When_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(() => x).Is(x => x == 10).Then(x => expected).Else(x => 20);

        result.Should().Be(100);
    }

    [Test]
    public void If_LazyValue_Not_Should_ReturnThenValue_When_PredicateIsTrue()
    {
        var x = "test";

        var result = If.Value(() => x).NotNull().Then(x => x).Else(() => "default");

        result.Should().Be(x);
    }
}
