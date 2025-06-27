using NUnit.Framework;
using Shouldly;
using System;

namespace Foundation;

[TestFixture]
public class ObjectExtensionsTests
{
    [Test]
    public void EqualsNullable_Should_ReturnTrue_When_BothSidesAreNull()
    {
        object? obj1 = null;
        object? obj2 = null;
        Assert.IsTrue(obj1.EqualsNullable(obj2));
    }

    [Test]
    public void EqualsNullable_Should_ReturnTrue_When_Using_EqualsFunc_BothSidesAreNull()
    {
        object? obj1 = null;
        object? obj2 = null;
        Assert.IsTrue(obj1.EqualsNullable(obj2, (lhs, rhs) => lhs.Equals(rhs)));
    }

    [Test]
    public void IfTrue_Should_ReturnNull_When_ValueIsNull()
    {
        string? word = null;
        word.IfTrue(x => x.Length == 5).ShouldBeNull();
    }

    [Test]
    public void IfTrue_Should_ReturnNull_When_ValueIsNotNullAndPredicateFalse()
    {
        string? word = "test";
        word.IfTrue(x => x.Length == 10).ShouldBeNull();
    }

    [Test]
    public void IfTrue_Should_ReturnString_When_ValueIsNotNullAndPredicateTrue()
    {
        string? word = "test";
        word.IfTrue(x => x.Length == 4).ShouldBe(word);
    }

    [Test]
    public void OrDefault_Should_ReturnDefaultValue_When_ValueIsNull()
    {
        int? number = null;
        number.OrDefault(() => 123).ShouldBe(123);
    }

    [Test]
    public void OrDefault_Should_ReturnValue_When_ValueNotNull()
    {
        123.OrDefault(() => 11).ShouldBe(123);
    }

    [Test]
    public void OrDefault_Should_ReturnStringDefaultValue_When_IntValueIsNull()
    {
        int? number = null;
        number.OrDefault(x => $"{x}", () => "default").ShouldBe("default");
    }

    [Test]
    public void ThrowIfOutOfRange_Should_ReturnValue_When_InRange()
    {
        var min = 5;
        var max = 10;

        var value = 8;
        var number = value.ThrowIfOutOfRange(() => value < min || value > max);
        Assert.AreEqual(8, number);
    }

    [Test]
    public void ThrowIfOutOfRange_Should_ThrowsAnException_When_OutOfRange()
    {
        var min = 1;
        var max = 7;
        var value = 8;

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var number = value.ThrowIfOutOfRange(() => value < min || value > max);
        });
    }

    [Test]
    public void ThrowIfOutOfRange_Should_ThrowsAnException_When_OutOfRange_UseMinMax()
    {
        var min = 1;
        var max = 7;
        var value = 8;
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var number = value.ThrowIfOutOfRange(() => value < min || value > max, 1, max);
        });
    }

    [Test]
    public void ToKeyValue_Should_ReturnVariableNameWithPropertyNameWithValue_When_PropertyNameOnlyIsFalse()
    {
        var str = "123";

        var kv = str.Length.ToKeyValue(propertyNameOnly: false);

        kv.Key.ShouldBe("str.Length");
        kv.Value.ShouldBe(str.Length);
    }

    [Test]
    public void ToKeyValue_Should_ReturnPropertyNameWithValue_When_PropertyNameOnlyIsTrue()
    {
        var str = "123";

        var kv = str.Length.ToKeyValue();

        kv.Key.ShouldBe(nameof(str.Length));
        kv.Value.ShouldBe(str.Length);
    }

    [Test]
    public void ToResult_Should_ReturnAnErrorResult_When_ValueIsNull()
    {
        string? str = null;

        var expected = -1;

        var result = str.ToResult(Int32.Parse, () => expected);

        result.IsOk.ShouldBeFalse();
        result.TryGetError(out var error).ShouldBeTrue();
        error.ShouldBe(expected);
    }

    [Test]
    public void ToResult_Should_ReturnAnOkResult_When_ValueNotNull()
    {
        var str = "123";

        var result = str.ToResult(Int32.Parse, () => -1);

        var expected = Int32.Parse(str);
        result.IsOk.ShouldBeTrue();
        result.TryGetOk(out var ok).ShouldBeTrue();
        ok.ShouldBe(expected);
    }

    [Test]
    public void ToType_Should_ReturnStringDefaultValue_When_IntValueNotNull()
    {
        var str = "123";

        var kv = str.Length.ToKeyValue();

        var result = str.ToType(Int32.Parse);

        var expected = Int32.Parse(str);
        result.ShouldBe(expected);
    }

}
