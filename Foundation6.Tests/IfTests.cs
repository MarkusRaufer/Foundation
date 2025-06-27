using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;

namespace Foundation;

[TestFixture]
public class IfTests
{
    [Test]
    public void True_Should_ReturnValue_When_CallIsLazy_And_OptionIsSome()
    {
        var dictionary = Enumerable.Range(0, 10).ToDictionary(x => x, x => x.ToString());
        
        var result = If.True(() => dictionary.TryGetValue(5, out var value).ToOption(value), (x) => x).Else((x) => "20");

        result.ShouldBe("5");
    }

    [Test]
    public void Value_Should_ReturnElseValue_When_CallIsEager_And_Is_PredicateIsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(x).Is(x => x == 1, _ => expected).Else(x => 20);

        result.ShouldBe(20);
    }

    [Test]
    public void Value_Should_ReturnThenValue_When_CallIsEager_And_Is_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(x).Is(x => x == 10, _ => expected).Else(x => 20);

        result.ShouldBe(100);
    }

    [Test]
    public void Value_Should_ReturnValue2_When_CallIsEager_And_Is_PredicateIsFalse_And_ElseIf_IsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(x)
                       .Is(x => x == 1, x => x)
                       .ElseIf(x => x == 10, _ => expected)
                       .Else(_ => 50);

        result.ShouldBe(expected);
    }

    [Test]
    public void Value_Should_ReturnValue2_When_CallIsEager_And_Is_PredicateIsFalse_And_ElseIf_IsTrue()
    {
        var x = 10;

        var result = If.Value(x).Is(x => x == 1, x => x)
                       .ElseIf(x => x == 10, x => x * 10)
                       .Else(_ => 22);

        var expected = 100;
        result.ShouldBe(expected);
    }

    [Test]
    public void True_Should_ReturnThenValue_When_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.True(() => x == 10, () => expected).Else(() => 20);

        result.ShouldBe(100);
    }

    [Test]
    public void Value_NotNull_Should_ReturnElseValue_When_CallIsEager_And_ValueIsNull()
    {
        string? value = null;
        string elseValue = "default";

        var result = If.Value(value).NotNull(x => x).Else(() => elseValue);

        result.ShouldBe(elseValue);
    }

    [Test]
    public void Type_Should_Return5_When_ValueIsInt()
    {
        int result = If.Type<int>(5)
                       .ElseIfType(6)
                       .Else(() => default);

        result.ShouldBe(5);
    }

    [Test]
    public void Type_Should_Return5AsString_When_ValueIsInt()
    {
        var result = If.Type<int, string?>(5, x => x.ToString())
                       .ElseIfType(6, x => x.ToString())
                       .Else(() => null);

        result.ShouldBe("5");
    }

    [Test]
    public void Type_Should_Return6_When_ValueIsStringButAlternativeIsInt()
    {
        var result = If.Type<int>("5")
                       .ElseIfType(6)
                       .Else(() => default);

        result.ShouldBe(6);

    }
    [Test]
    public void Type_Should_Return6AsString_When_ValueIsStringButAlternativeIsInt()
    {
        var result = If.Type<int, string?>("5", x => x.ToString())
                       .ElseIfType(6, x => x.ToString())
                       .Else(() => null);

        result.ShouldBe("6");
    }

    [Test]
    public void Type_Should_Return6AsString_When_ValueIsDateTimeButAlternativeIsInt()
    {
        var dt = new DateTime(2020, 1, 2, 3, 4, 5);

        var result = If.Type<int, string?>(dt, x => x.ToString())
                       .ElseIfType(6, x => x.ToString())
                       .Else(() => null);

        result.ShouldBe("6");
    }

    [Test]
    public void Type_Should_ReturnNull_When_NoTypeIsCompatible()
    {
        int result = If.Type<int>(DateTime.Now)
                       .ElseIfType("6")
                       .Else(() => 0);

        result.ShouldBe(0);
    }

    [Test]
    public void Type_Should_ReturnNull_When_NoTypeIsOfResultType()
    {
        var result = If.Type<int, string?>("5", x => x.ToString())
                       .ElseIfType(DateTime.Now.ToDateOnly(), x => x.ToString())
                       .ElseIfType(DateTime.Now, x => x.ToString())
                       .Else(() => null);

        result.ShouldBeNull();
    }

    [Test]
    public void Value_Should_ReturnElseValue_When_ValueIsNull()
    {
        string? x = null;
        var expected = 1;

        var result = If.Value(() => x).NotNull(x => int.Parse(x)).Else(() => expected);

        result.ShouldBe(expected);
    }

    [Test]
    public void Value_Should_ReturnThenValue_When_ValueNotNull()
    {
        var x = "12";
        var expected = int.Parse(x);

        var result = If.Value(() => x).NotNull(int.Parse).Else(() => 1);

        result.ShouldBe(expected);
    }

    [Test]
    public void Value_Should_ReturnElseValue_When_PredicateIsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(() => x).Is(x => x == 1, _ => expected).Else(x => 20);

        result.ShouldBe(20);
    }

    [Test]
    public void Value_Should_ReturnThenValueInt_When_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.Value(() => x).Is(x => x == 10, _ => expected).Else(x => 20);

        result.ShouldBe(100);
    }

    [Test]
    public void Value_Should_ReturnThenValueString_When_PredicateIsTrue()
    {
        var x = "test";

        var result = If.Value(() => x).NotNull(x => x).Else(() => "default");

        result.ShouldBe(x);
    }

    [Test]
    public void True_Should_ReturnElseValue_When_PredicateIsFalse()
    {
        var x = 10;
        var expected = 100;

        var result = If.True(() => x == 5, () => expected).Else(() => 20);

        result.ShouldBe(20);
    }

    [Test]
    public void True_Should_ReturnThenValue_When_CallIsLazy_And_PredicateIsTrue()
    {
        var x = 10;
        var expected = 100;

        var result = If.True(() => x == 10, () => expected).Else(() => 20);

        result.ShouldBe(100);
    }
}
