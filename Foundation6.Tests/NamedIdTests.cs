using FluentAssertions;
using Foundation.Collections.Generic;
using Foundation.Text.Json;
using Foundation.Text.Json.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;

namespace Foundation;

[TestFixture]
public class NamedIdTests
{
    [Test]
    public void Equals_Should_ReturnsFalse_When_ValuesAreSame_But_NamesAreDifferent()
    {
        var value = 10;

        var sut1 = NamedId.New("name1", value);
        var sut2 = NamedId.New("name2", value);

        sut1.Equals(sut2).Should().BeFalse();
    }

    [Test]
    public void Equals_ReturnsFalse_When_SameNames_And_DifferentValues()
    {
        var sut1 = NamedId.New("x", 10);
        var sut2 = NamedId.New("x", 20);

        sut1.Equals(sut2).Should().BeFalse();
    }

    [Test]
    public void Equals_Should_ReturnsTrue_When_NamesAndValuesAreSame()
    {
        var value = 10;

        var sut1 = NamedId.New("x", value);
        var sut2 = NamedId.New("x", value);

        sut1.Equals(sut2).Should().BeTrue();
    }

    [Test]
    public void Equals_Should_ReturnsTrue_When_NamesAndValuesAreEqual_And_ValuesAreStrings()
    {
        var sut1 = NamedId.New("x", "test");
        var sut2 = NamedId.New("x", "test");

        sut1.Equals(sut2).Should().BeTrue();
    }

    [Test]
    public void EqualsOperator_Should_ReturnsFalse_When_ValuesAreSame_But_NamesAreDifferent()
    {
        var value = 10;

        var sut1 = NamedId.New("name1", value);
        var sut2 = NamedId.New("name2", value);

        var eq = sut1 == sut2;

        eq.Should().BeFalse();
    }

    [Test]
    public void EqualsOperator_Should_ReturnsTrue_When_NamesAndValuesAreEqual_And_ValuesAreStrings()
    {
        var sut1 = NamedId.New("x", "test");
        var sut2 = NamedId.New("x", "test");

        var eq = sut1 == sut2;

        eq.Should().BeTrue();
    }

    [Test]
    public void Compare_Should_Return0_When_NamesAndValuesAreSame()
    {
        var sut1 = NamedId.New("x", 10);
        var sut2 = NamedId.New("x", 10);

        var cmp = sut1.CompareTo(sut2);
        cmp.Should().Be(0);
    }

    [Test]
    public void Compare_Should_Return1_When_NamesAreSame_ValuesAreDifferent()
    {
        var sut1 = NamedId.New("x", 20);
        var sut2 = NamedId.New("x", 10);

        var cmp = sut1.CompareTo(sut2);
        cmp.Should().Be(1);
    }
    [Test]
    public void Compare_Should_Returns1_When_ValuesAreSameButNamesAreDifferent()
    {
        var value = 10;

        var sut1 = NamedId.New("name2", value);
        var sut2 = NamedId.New("name1", value);

        var cmp = sut1.CompareTo(sut2);
        cmp.Should().Be(1);
    }

    [Test]
    public void Compare_Should_ReturnsMinus1_When_ValuesAreSameButNamesAreDifferent()
    {
        var value = 10;

        var sut1 = NamedId.New("name1", value);
        var sut2 = NamedId.New("name2", value);

        var cmp = sut1.CompareTo(sut2);
        cmp.Should().Be(-1);
    }

    [Test]
    public void Compare_Should_ReturnMinus1_When_ValuesAreDifferent()
    {
        var sut1 = NamedId.New("x", 10);
        var sut2 = NamedId.New("x", 20);

        var cmp = sut1.CompareTo(sut2);
        cmp.Should().Be(-1);
    }

    [Test]
    public void Greater_Should_ReturnsFalse_When_ValuesAreSameButNamesAreDifferent()
    {
        var value = 10;

        var sut1 = NamedId.New("name1", value);
        var sut2 = NamedId.New("name2", value);

        var gt = sut1 > sut2;
        gt.Should().BeFalse();
    }

    [Test]
    public void Greater_Should_ReturnsFalse_When_NamesAreSame_And_ValuesAreDifferent()
    {
        var sut1 = NamedId.New("x", 10);
        var sut2 = NamedId.New("x", 20);

        var gt = sut1 > sut2;
        gt.Should().BeFalse();
    }


    [Test]
    public void Greater_Should_ReturnsTrue_When_ValuesAreSameButNamesAreDifferent()
    {
        var value = 10;

        var sut1 = NamedId.New("name2", value);
        var sut2 = NamedId.New("name1", value);

        var gt = sut1 > sut2;
        gt.Should().BeTrue();
    }

    [Test]
    public void Greater_Should_ReturnsTrue_When_NamesAreSame_And_ValuesAreDifferent()
    {
        var sut1 = NamedId.New("x", 20);
        var sut2 = NamedId.New("x", 10);

        var gt = sut1 > sut2;
        gt.Should().BeTrue();
    }

    [Test]
    public void GreaterOrEqual_Should_ReturnsTrue_When_NamesAndValuesAreSame()
    {
        var sut1 = NamedId.New("x", 10);
        var sut2 = NamedId.New("x", 10);

        var gt = sut1 >= sut2;
        gt.Should().BeTrue();
    }

    [Test]
    public void GreaterOrEqual_Should_ReturnsTrue_When_NamesAreSame_And_ValuesAreDifferent()
    {
        var sut1 = NamedId.New("x", 20);
        var sut2 = NamedId.New("x", 10);

        var gt = sut1 >= sut2;
        gt.Should().BeTrue();
    }

    [Test]
    public void Serialize_ReturnsValidJsonString_When_ValueIsDateOnly()
    {
        var name = "BirthDay";
        var value = new DateOnly(2001, 2, 3);
        var sut = NamedId.NewId(name, value);

        var json = JsonSerializer.Serialize(sut, new JsonSerializerOptions
        {
            Converters = { new NamedIdJsonConverter() }
        });

        var expected = $@"{{""Name"":""{name}"",""Type"":{{""FullName"":""System.DateOnly""}},""Value"":""{value:yyyy-MM-dd}""}}";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_ReturnsValidJsonString_When_ValueIsDateTime()
    {
        var name = "BirthDay";
        var value = new DateTime(2001, 2, 3, 4, 5 , 6);
        var sut = NamedId.NewId(name, value);

        var json = JsonSerializer.Serialize(sut, new JsonSerializerOptions
        {
            Converters = { new NamedIdJsonConverter() }
        });

        var expected = $@"{{""Name"":""{name}"",""Type"":{{""FullName"":""System.DateTime""}},""Value"":""{value:yyyy-MM-ddTHH:mm:ss}""}}";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_ReturnsValidJsonString_When_ValueIsDecimal()
    {
        var name = "Number";
        var value = 12.34M;
        var sut = NamedId.NewId(name, value);

        var json = JsonSerializer.Serialize(sut, new JsonSerializerOptions
        {
            Converters = { new NamedIdJsonConverter() }
        });

        var expected = string.Create(CultureInfo.InvariantCulture, $@"{{""Name"":""{name}"",""Type"":{{""FullName"":""System.Decimal""}},""Value"":{value}}}");
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_ReturnsValidJsonString_When_ValueIsGuid()
    {
        var name = "MyId";
        var value = Guid.NewGuid();
        var sut = NamedId.NewId(name, value);

        var json = JsonSerializer.Serialize(sut, new JsonSerializerOptions
        {
            Converters = { new NamedIdJsonConverter() }
        });

        var expected = $@"{{""Name"":""{name}"",""Type"":{{""FullName"":""System.Guid""}},""Value"":""{value}""}}";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_ReturnsValidJsonString_When_ValueIsString()
    {
        var name = "Name";
        var value = "John";
        var sut = NamedId.NewId(name, value);

        var json = JsonSerializer.Serialize(sut, new JsonSerializerOptions
        {
            Converters = { new NamedIdJsonConverter() }
        });

        var expected = $@"{{""Name"":""{name}"",""Type"":{{""FullName"":""System.String""}},""Value"":""{value}""}}";
        json.Should().Be(expected);
    }

    [Test]
    public void Smaller_Should_ReturnsFalse_When_ValuesAreSameButNamesAreDifferent()
    {
        var value = 10;

        var sut1 = NamedId.New("name2", value);
        var sut2 = NamedId.New("name1", value);

        var st = sut1 < sut2;
        st.Should().BeFalse();
    }

    [Test]
    public void Smaller_Should_ReturnsTrue_When_ValuesAreSameButNamesAreDifferent()
    {
        var value = 10;

        var sut1 = NamedId.New("name1", value);
        var sut2 = NamedId.New("name2", value);

        var gt = sut1 < sut2;
        gt.Should().BeTrue();
    }

    [Test]
    public void SmallerOrEqual_Should_ReturnsTrue_When_ValuesAreSameButNamesAreDifferent()
    {
        var value = 10;

        var sut1 = NamedId.New("name1", value);
        var sut2 = NamedId.New("name2", value);

        var gt = sut1 <= sut2;
        gt.Should().BeTrue();
    }

    [Test]
    public void SmallerOrEqual_Should_ReturnsTrue_When_NamesAreSame_And_ValuesAreDifferent()
    {
        var sut1 = NamedId.New("x", 10);
        var sut2 = NamedId.New("x", 20);

        var gt = sut1 <= sut2;
        gt.Should().BeTrue();
    }
}
