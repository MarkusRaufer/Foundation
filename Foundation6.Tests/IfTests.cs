using FluentAssertions;
using NUnit.Framework;
using System;

namespace Foundation;

[TestFixture]
public class IfTests
{
    [Test]
    public void EagerValue_Should_ReturnElseValue_When_Is_PredicateIsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(x).Is(x => x == 1, _ => expected).Else(x => 20);

        result.Should().Be(20);
    }

    [Test]
    public void EagerValue_Should_ReturnThenValue_When_Is_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(x).Is(x => x == 10, _ => expected).Else(x => 20);

        result.Should().Be(100);
    }

    [Test]
    public void EagerValue_Should_ReturnValue2_When_Is_PredicateIsFalse_And_ElseIf_IsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(x)
                       .Is(x => x == 1, x => x)
                       .ElseIf(x => x == 10, _ => expected)
                       .Else(_ => 50);

        result.Should().Be(expected);
    }

    [Test]
    public void EagerValue_Should_ReturnValue2_When_Is_PredicateIsFalse_And_ElseIf_IsTrue()
    {
        var x = 10;

        var result = If.Value(x).Is(x => x == 1, x => x)
                       .ElseIf(x => x == 10, x => x * 10)
                       .Else(_ => 22);

        var expected = 100;
        result.Should().Be(expected);
    }

    [Test]
    public void EagerTrue_Should_ReturnThenValue_When_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.True(() => x == 10, () => expected).Else(() => 20);

        result.Should().Be(100);
    }

    [Test]
    public void EagerValue_NotNull_Should_ReturnElseValue_When_ValueIsNull()
    {
        string? value = null;
        string elseValue = "default";

        var result = If.Value(value).NotNull(x => x).Else(() => elseValue);

        result.Should().Be(elseValue);
    }

    [Test]
    public void If_Type_Should_Return5_When_ValueIsInt()
    {
        int result = If.Type<int>(5)
                       .ElseIfType(6)
                       .Else(() => default);

        result.Should().Be(5);
    }

    [Test]
    public void If_Type_Should_Return5AsString_When_ValueIsInt()
    {
        var result = If.Type<int, string?>(5, x => x.ToString())
                       .ElseIfType(6, x => x.ToString())
                       .Else(() => null);

        result.Should().Be("5");
    }

    [Test]
    public void If_Type_Should_Return6_When_ValueIsStringButAlternativeIsInt()
    {
        var result = If.Type<int>("5")
                       .ElseIfType(6)
                       .Else(() => default);

        result.Should().Be(6);

    }
    [Test]
    public void If_Type_Should_Return6AsString_When_ValueIsStringButAlternativeIsInt()
    {
        var result = If.Type<int, string?>("5", x => x.ToString())
                       .ElseIfType(6, x => x.ToString())
                       .Else(() => null);

        result.Should().Be("6");
    }

    [Test]
    public void If_Type_Should_Return6AsString_When_ValueIsDateTimeButAlternativeIsInt()
    {
        var dt = new DateTime(2020, 1, 2, 3, 4, 5);

        var result = If.Type<int, string?>(dt, x => x.ToString())
                       .ElseIfType(6, x => x.ToString())
                       .Else(() => null);

        result.Should().Be("6");
    }

    [Test]
    public void If_Type_Should_ReturnNull_When_NoTypeIsCompatible()
    {
        int result = If.Type<int>(DateTime.Now)
                       .ElseIfType("6")
                       .Else(() => 0);

        result.Should().Be(0);
    }

    [Test]
    public void If_Type_Should_ReturnNull_When_NoTypeIsOfResultType()
    {
        var result = If.Type<int, string?>("5", x => x.ToString())
                       .ElseIfType(DateTime.Now.ToDateOnly(), x => x.ToString())
                       .ElseIfType(DateTime.Now, x => x.ToString())
                       .Else(() => null);

        result.Should().BeNull();
    }

    [Test]
    public void LazyValue_Should_ReturnElseValue_When_ValueIsNull()
    {
        string? x = null;
        var expected = 1;

        var result = If.Value(() => x).NotNull(x => int.Parse(x)).Else(() => expected);

        result.Should().Be(expected);
    }

    [Test]
    public void LazyValue_Should_ReturnThenValue_When_ValueNotNull()
    {
        var x = "12";
        var expected = int.Parse(x);

        var result = If.Value(() => x).NotNull(int.Parse).Else(() => 1);

        result.Should().Be(expected);
    }

    [Test]
    public void LazyValue_Should_ReturnElseValue_When_PredicateIsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(() => x).Is(x => x == 1, _ => expected).Else(x => 20);

        result.Should().Be(20);
    }

    [Test]
    public void LazyValue_Should_ReturnThenValueInt_When_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(() => x).Is(x => x == 10, _ => expected).Else(x => 20);

        result.Should().Be(100);
    }

    [Test]
    public void LazyValue_Should_ReturnThenValueString_When_PredicateIsTrue()
    {
        var x = "test";

        var result = If.Value(() => x).NotNull(x => x).Else(() => "default");

        result.Should().Be(x);
    }

    [Test]
    public void LazyTrue_Should_ReturnElseValue_When_PredicateIsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.True(() => x == 5, () => expected).Else(() => 20);

        result.Should().Be(20);
    }

    [Test]
    public void LazyTrue_Should_ReturnThenValue_When_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.True(() => x == 10, () => expected).Else(() => 20);

        result.Should().Be(100);
    }
}
