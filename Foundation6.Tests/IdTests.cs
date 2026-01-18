using Foundation.Text.Json.Serialization;
using NUnit.Framework;
using Shouldly;
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

        sut1.ShouldBe(sut2);
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

        sut1.ShouldBe(sut2);
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

        sut1.ShouldBe(sut2);
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

        sut1.ShouldBe(sut2);
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

        sut1.ShouldBe(sut2);
    }

    [Test]
    public void Equals_ReturnsFalse_When_SameType_DifferentIntValues()
    {
        var sut1 = Id.New(10);
        var sut2 = Id.New(20);

        sut1.Equals(sut2).ShouldBeFalse();
    }

    [Test]
    public void Equals_ReturnsFalse_When_SameType_DifferentDecimalValues()
    {
        var sut1 = Id.New(10M);
        var sut2 = Id.New(20M);

        sut1.Equals(sut2).ShouldBeFalse();
    }

    [Test]
    public void Equals_ReturnsFalse_When_SameType_DifferentStringValues()
    {
        var sut1 = Id.New("10");
        var sut2 = Id.New("20");

        sut1.Equals(sut2).ShouldBeFalse();
    }

    [Test]
    public void Equals_ReturnsFalse_When_DifferentTypes_And_SameValues()
    {
        var value = 10;

        var sut1 = Id.New(value);
        var sut2 = Id.New((char)value);

        sut1.Equals(sut2).ShouldBeFalse();
    }

    [Test]
    public void Equals_Should_ReturnsTrue_When_SameTypes_And_SameDecimalValues()
    {
        var value = 10M;

        var sut1 = Id.New(value);
        var sut2 = Id.New(value);

        sut1.Equals(sut2).ShouldBeTrue();
    }

    [Test]
    public void Equals_Should_ReturnsTrue_When_SameTypes_And_SameGuidValues()
    {
        var value = Guid.NewGuid();

        var sut1 = Id.New(value);
        var sut2 = Id.New(value);

        sut1.Equals(sut2).ShouldBeTrue();
    }

    [Test]
    public void Equals_Should_ReturnsTrue_When_SameTypes_And_SameIntValues()
    {
        var value = 10;

        var sut1 = Id.New(value);
        var sut2 = Id.New(value);

        sut1.Equals(sut2).ShouldBeTrue();
    }

    [Test]
    public void Equals_Should_ReturnsTrue_When_SameTypes_And_SameStringValues()
    {
        var sut1 = Id.New("10");
        var sut2 = Id.New("10");

        sut1.Equals(sut2).ShouldBeTrue();
    }

    [Test]
    public void GreaterThan_ReturnsTrue_When_DifferentTypes_And_SameValues()
    {
        var value = 10;

        var sut1 = Id.New(value);
        var sut2 = Id.New((char)value);

        var cmp = sut1 > sut2;
        cmp.ShouldBeTrue();
    }

    [Test]
    public void GreaterThan_ReturnsTrue_When_SameTypes_And_FirstValueIsGreater()
    {
        // Arrange
        var value1 = 10;
        var value2 = 5;

        // Act
        var sut1 = Id.New(value1);
        var sut2 = Id.New(value2);

        // Assert
        var cmp = sut1 > sut2;
        cmp.ShouldBeTrue();
    }

    [Test]
    public void GreaterThan_ReturnsFalse_When_SameTypes_And_FirstValueIsSmaller()
    {
        // Arrange
        var value1 = 5;
        var value2 = 8;

        // Act
        var sut1 = Id.New(value1);
        var sut2 = Id.New(value2);

        // Assert
        var cmp = sut1 > sut2;
        cmp.ShouldBeFalse();
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

        var expected = $@"{{""Type"":""{value.GetType().AssemblyQualifiedName}"",""Value"":""{value:yyyy-MM-ddTHH:mm:ss}""}}";
        json.ShouldBe(expected);
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

        var expected = $@"{{""Type"":""{value.GetType().AssemblyQualifiedName}"",""Value"":""{value:yyyy-MM-dd}""}}";
        json.ShouldBe(expected);
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

        var expected = string.Create(CultureInfo.InvariantCulture, $@"{{""Type"":""{value.GetType().AssemblyQualifiedName}"",""Value"":{value}}}");
        json.ShouldBe(expected);
    }

    [Test]
    public void Serialize_ToJson_ShouldReturnValidString_When_Using_Guid()
    {
        var value = SameGuid.New(1);
        var sut1 = Id.New(value);

        var json = JsonSerializer.Serialize(sut1, new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        });

        var expected = $@"{{""Type"":""{value.GetType().AssemblyQualifiedName}"",""Value"":""{value}""}}";
        json.ShouldBe(expected);
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
        
        var expected = $@"{{""Type"":""{value.GetType().AssemblyQualifiedName}"",""Value"":{value}}}";
        json.ShouldBe(expected);
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

        var expected = $@"{{""Type"":""{value.GetType().AssemblyQualifiedName}"",""Value"":""{value}""}}";
        json.ShouldBe(expected);
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

        var expected = $@"{{""Type"":""{value.GetType().AssemblyQualifiedName}"",""Value"":""{value:HH:mm:ss}""}}";
        json.ShouldBe(expected);
    }

    [Test]
    public void SmallerThan_ReturnsFalse_When_DifferentTypes_And_SameValues()
    {
        var value = 10;

        var sut1 = Id.New(value);
        var sut2 = Id.New((char)value);

        var cmp = sut1 < sut2;
        cmp.ShouldBeFalse();
    }

    [Test]
    public void SmallerThan_ReturnsTrue_When_SameTypes_And_FirstValueIsGreater()
    {
        // Arrange
        var value1 = 5;
        var value2 = 10;

        // Act
        var sut1 = Id.New(value1);
        var sut2 = Id.New(value2);

        // Assert
        var cmp = sut1 < sut2;
        cmp.ShouldBeTrue();
    }

    [Test]
    public void SmallerThan_ReturnsFalse_When_SameTypes_And_FirstValueIsGreater()
    {
        // Arrange
        var value1 = 10;
        var value2 = 5;

        // Act
        var sut1 = Id.New(value1);
        var sut2 = Id.New(value2);

        // Assert
        var cmp = sut1 < sut2;
        cmp.ShouldBeFalse();
    }
}

