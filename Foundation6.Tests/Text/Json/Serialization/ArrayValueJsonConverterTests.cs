using FluentAssertions;
using Foundation.Collections.Generic;
using NUnit.Framework;
using System.Text.Json;

namespace Foundation.Text.Json.Serialization;

[TestFixture]
public class ArrayValueJsonConverterTests
{
    [Test]
    public void Deserialze_Should_ReturnAValidHashSetValue_When_UsedHashSetValueJsonConverter()
    {
        var sut = ArrayValue.New([1, 2, 3]);

        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new ArrayValueJsonConverter<int>() }
        };

        var json = JsonSerializer.Serialize(sut, jsonOptions);

        var deserialized = JsonSerializer.Deserialize<ArrayValue<int>>(json, jsonOptions);

        sut.Should().BeEquivalentTo(deserialized);
    }

    [Test]
    public void Serialze_Should_ReturnValidJsonString_When_UsingStandardJsonSerializer()
    {
        var sut = ArrayValue.New([1, 2, 3]);

        var json = JsonSerializer.Serialize(sut);

        json.Should().Be("[1,2,3]");
    }
}
