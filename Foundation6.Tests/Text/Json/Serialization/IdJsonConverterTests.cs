using FluentAssertions;
using NUnit.Framework;
using System.Text.Json;

namespace Foundation.Text.Json.Serialization;

[TestFixture]
public class IdJsonConverterTests
{
    [Test]
    public void Deserialize_Should_ReturnADecimal_When_String_ContainsADecimal()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var number = 12.34M;
        var expectedId = Id.New(number);
        var json = JsonSerializer.Serialize(expectedId, options);

        //Act
        var id = JsonSerializer.Deserialize<Id>(json, options);

        //Assert
        id.Should().Be(expectedId);
    }


    [Test]
    public void Deserialize_Should_ReturnADouble_When_String_ContainsADouble()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var number = 12.34;
        var expectedId = Id.New(number);
        var json = JsonSerializer.Serialize(expectedId, options);

        //Act
        var id = JsonSerializer.Deserialize<Id>(json, options);

        //Assert
        id.Should().Be(expectedId);
    }

    [Test]
    public void Deserialize_Should_ReturnAGuid_When_String_ContainsAGuid()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var guid = SameGuid.New(1);
        var expectedId = Id.New(guid);
        var json = JsonSerializer.Serialize(expectedId, options);

        //Act
        var id = JsonSerializer.Deserialize<Id>(json, options);

        //Assert
        id.Should().Be(expectedId);
    }

    [Test]
    public void Deserialize_Should_ReturnALong_When_String_ContainsAnLong()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var guid = SameGuid.New(1);
        var expectedNumber = Id.New(guid);
        var json = JsonSerializer.Serialize(expectedNumber, options);

        //Act
        var number = JsonSerializer.Deserialize<Id>(json, options);

        //Assert
        number.Should().Be(expectedNumber);
    }

    [Test]
    public void Deserialize_Should_ReturnAnInt_When_String_ContainsAnInt()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var number = 5;
        var expectedId = Id.New(number);
        var json = JsonSerializer.Serialize(expectedId, options);

        //Act
        var id = JsonSerializer.Deserialize<Id>(json, options);

        //Assert
        id.Should().Be(expectedId);
    }

    [Test]
    public void Deserialize_Should_ReturnAString_When_String_ContainsAString()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var str = "I12345";
        var expectedId = Id.New(str);
        var json = JsonSerializer.Serialize(expectedId, options);

        //Act
        var id = JsonSerializer.Deserialize<Id>(json, options);

        //Assert
        id.Should().Be(expectedId);
    }

    [Test]
    public void Serialize_Should_ReturnAJsonString_When_Id_ContainsADecimal()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var number = 12.34M;
        var id = Id.New(number);

        //Act
        var json = JsonSerializer.Serialize(id, options);

        //Assert
        var expected = $$"""{"Type":{"FullName":"System.Decimal"},"Value":{{number.ToInvariantString()}}}""";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_Should_ReturnAJsonString_When_Id_ContainsADouble()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var number = 12.34;
        var id = Id.New(number);

        //Act
        var json = JsonSerializer.Serialize(id, options);

        //Assert
        var expected = $$"""{"Type":{"FullName":"System.Double"},"Value":{{number.ToInvariantString()}}}""";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_Should_ReturnAJsonString_When_Id_ContainsAGuid()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var guid = SameGuid.New(1);
        var expectedId = Id.New(guid);

        //Act
        var json = JsonSerializer.Serialize(expectedId, options);

        //Assert
        var expected = $$"""{"Type":{"FullName":"System.Guid"},"Value":"{{guid}}"}""";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_Should_ReturnAJsonString_When_Id_ContainsAnInt()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var number = 5;
        var id = Id.New(number);

        //Act
        var json = JsonSerializer.Serialize(id, options);

        //Assert
        var expected = $$"""{"Type":{"FullName":"System.Int32"},"Value":{{number}}}""";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_Should_ReturnAJsonString_When_Id_ContainsALong()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var number = 5L;
        var expectedId = Id.New(number);

        //Act
        var json = JsonSerializer.Serialize(expectedId, options);

        //Assert
        var expected = $$"""{"Type":{"FullName":"System.Int64"},"Value":{{number}}}""";
        json.Should().Be(expected);
    }

    [Test]
    public void Serialize_Should_ReturnAJsonString_When_Id_ContainsAString()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new IdJsonConverter() }
        };

        var str = "I12345";
        var expectedId = Id.New(str);

        //Act
        var json = JsonSerializer.Serialize(expectedId, options);

        //Assert
        var expected = $$"""{"Type":{"FullName":"System.String"},"Value":"{{str}}"}""";
        json.Should().Be(expected);
    }
}
