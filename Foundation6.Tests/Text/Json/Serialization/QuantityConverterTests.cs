﻿using FluentAssertions;
using NUnit.Framework;
using System.Text.Json;

namespace Foundation.Text.Json.Serialization;

[TestFixture]
public class QuantityConverterTests
{
    [Test]
    public void Deserialize_Should_ReturnQuantity_When_JsonContainsQuantity()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new QuantityConverter() }
        };

        var expected = Quantity.New("kg", 12.34M);
        var json = JsonSerializer.Serialize(expected, options);

        //Act
        var quantity = JsonSerializer.Deserialize<Quantity?>(json, options);

        //ClassicAssert
        quantity.Should().Be(expected);
    }

    [Test]
    public void Serialize_Should_ReturnValidJson_When_QuantityWithoutGenericParametersIsUsed()
    {
        //Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new QuantityConverter() }
        };

        var quantity = Quantity.New("kg", 12.34M);

        //Act
        var json = JsonSerializer.Serialize(quantity, options);

        //ClassicAssert
        var expected = $$"""{"Unit":"kg","Value":12.34}""";
        json.Should().Be(expected);
    }
}
