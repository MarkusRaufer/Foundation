using FluentAssertions;
using Foundation.Text.Json.Serialization;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Text.Json;

namespace Foundation;

[TestFixture]
public class IdTests
{
    [Test]
    public void Deserialize_ToJson_ShouldReturnId_When_Using_DateTime()
    {
        var value = new DateTime(2001, 2, 3, 4, 5, 6);
        var sut1 = Id.New(value);

        var json = $@"{{""Type"":""System.DateTime"",""Value"":""{value:yyyy-MM-ddTHH:mm:ss}""}}";

        var sut2 = JsonSerializer.Deserialize<Id>(json, new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        });

        sut1.Should().Be(sut2);
    }

    [Test]
    public void Deserialize_ToJson_ShouldReturnId_When_Using_DateOnly()
    {
        var value = new DateOnly(2001, 2, 3);
        var sut1 = Id.New(value);

        var json = $@"{{""Type"":""System.DateOnly"",""Value"":""{value:yyyy-MM-dd}""}}";

        var sut2 = JsonSerializer.Deserialize<Id>(json, new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        });

        sut1.Should().Be(sut2);
    }

    [Test]
    public void Deserialize_ToJson_ShouldReturnId_When_Using_Decimal()
    {
        var value = 12.34M;
        var sut1 = Id.New(value);

        var json = string.Create(CultureInfo.InvariantCulture, $@"{{""Type"":""System.Decimal"",""Value"":{value}}}");

        var sut2 = JsonSerializer.Deserialize<Id>(json, new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        });

        sut1.Should().Be(sut2);
    }

    [Test]
    public void Deserialize_ToJson_ShouldReturnId_When_Using_Guid()
    {
        var value = Guid.NewGuid();
        var sut1 = Id.New(value);

        var json = $@"{{""Type"":""System.Guid"",""Value"":""{value}""}}";

        var sut2 = JsonSerializer.Deserialize<Id>(json, new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        });

        sut1.Should().Be(sut2);
    }

    [Test]
    public void Deserialize_ToJson_ShouldReturnId_When_Using_Int32()
    {
        var value = 12;
        var sut1 = Id.New(value);

        var json = $@"{{""Type"":""System.Int32"",""Value"":{value}}}";

        var sut2 = JsonSerializer.Deserialize<Id>(json, new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        });

        sut1.Should().Be(sut2);
    }

    [Test]
    public void Equals_ReturnsFalse_When_SameType_DifferentIntValues()
    {
        var sut1 = Id.New(10);
        var sut2 = Id.New(20);

        sut1.Equals(sut2).Should().BeFalse();
    }

    [Test]
    public void Equals_ReturnsFalse_When_SameType_DifferentDecimalValues()
    {
        var sut1 = Id.New(10M);
        var sut2 = Id.New(20M);

        sut1.Equals(sut2).Should().BeFalse();
    }

    [Test]
    public void Equals_ReturnsFalse_When_SameType_DifferentStringValues()
    {
        var sut1 = Id.New("10");
        var sut2 = Id.New("20");

        sut1.Equals(sut2).Should().BeFalse();
    }

    [Test]
    public void Equals_ReturnsFalse_When_DifferentTypes_And_SameValues()
    {
        var value = 10;

        var sut1 = Id.New(value);
        var sut2 = Id.New((char)value);

        sut1.Equals(sut2).Should().BeFalse();
    }

    [Test]
    public void Equals_Should_ReturnsTrue_When_SameTypes_And_SameDecimalValues()
    {
        var value = 10M;

        var sut1 = Id.New(value);
        var sut2 = Id.New(value);

        sut1.Equals(sut2).Should().BeTrue();
    }

    [Test]
    public void Equals_Should_ReturnsTrue_When_SameTypes_And_SameGuidValues()
    {
        var value = Guid.NewGuid();

        var sut1 = Id.New(value);
        var sut2 = Id.New(value);

        sut1.Equals(sut2).Should().BeTrue();
    }

    [Test]
    public void Equals_Should_ReturnsTrue_When_SameTypes_And_SameIntValues()
    {
        var value = 10;

        var sut1 = Id.New(value);
        var sut2 = Id.New(value);

        sut1.Equals(sut2).Should().BeTrue();
    }

    [Test]
    public void Equals_Should_ReturnsTrue_When_SameTypes_And_SameStringValues()
    {
        var sut1 = Id.New("10");
        var sut2 = Id.New("10");

        sut1.Equals(sut2).Should().BeTrue();
    }

    [Test]
    public void Serialize_ToJson_ShouldReturnValidString_When_Using_DateTime()
    {
        var value = new DateTime(2001, 2, 3, 4, 5, 6);
        var sut1 = Id.New(value);

        var json = JsonSerializer.Serialize(sut1, new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        });

        var expected = $@"{{""Type"":""System.DateTime"",""Value"":""{value:yyyy-MM-ddTHH:mm:ss}""}}";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_ToJson_ShouldReturnValidString_When_Using_DateOnly()
    {
        var value = new DateOnly(2001, 2, 3);
        var sut1 = Id.New(value);

        var json = JsonSerializer.Serialize(sut1, new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        });

        var expected = $@"{{""Type"":""System.DateOnly"",""Value"":""{value:yyyy-MM-dd}""}}";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_ToJson_ShouldReturnValidString_When_Using_Decimal()
    {
        var value = 12.34M;
        var sut1 = Id.New(value);

        var json = JsonSerializer.Serialize(sut1, new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        });

        var expected = string.Create(CultureInfo.InvariantCulture, $@"{{""Type"":""System.Decimal"",""Value"":{value}}}");
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_ToJson_ShouldReturnValidString_When_Using_Int()
    {
        var value = 12;
        var sut1 = Id.New(value);

        var json = JsonSerializer.Serialize(sut1, new JsonSerializerOptions
        {
            Converters = {new IdJsonConverter() }
        });
        
        var expected = $@"{{""Type"":""System.Int32"",""Value"":{value}}}";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_ToJson_ShouldReturnValidString_When_Using_String()
    {
        var value = "test";
        var sut1 = Id.New(value);

        var json = JsonSerializer.Serialize(sut1, new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        });

        var expected = $@"{{""Type"":""System.String"",""Value"":""{value}""}}";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_ToJson_ShouldReturnValidString_When_Using_TimeOnly()
    {
        var value = new TimeOnly(1, 2, 3, 4);
        var sut1 = Id.New(value);

        var json = JsonSerializer.Serialize(sut1, new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        });

        var expected = $@"{{""Type"":""System.TimeOnly"",""Value"":""{value:HH:mm:ss}""}}";
        json.Should().Be(expected);
    }
}

