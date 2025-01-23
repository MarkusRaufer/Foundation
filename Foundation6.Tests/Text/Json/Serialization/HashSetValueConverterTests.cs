using FluentAssertions;
using Foundation.Collections.Generic;
using NUnit.Framework;
using System.Text.Json;

namespace Foundation.Text.Json.Serialization;

[TestFixture]
public class HashSetValueConverterTests
{
    [Test]
    public void DeserialzeToJson_Should_ReturnJsonString_When_Called_JsonSerializer_Serialize()
    {
        var sut = HashSetValue.New([1, 2, 3]);

        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new HashSetValueJsonConverter<int>() }
        };

        var json = JsonSerializer.Serialize(sut, jsonOptions);

        var deserialized = JsonSerializer.Deserialize<HashSetValue<int>>(json, jsonOptions);

        sut.Should().BeEquivalentTo(deserialized);
    }

    [Test]
    public void SerialzeToJson_Should_ReturnJsonString_When_Called_JsonSerializer_Serialize()
    {
        var sut = HashSetValue.New([1, 2, 3]);

        var json = JsonSerializer.Serialize(sut);

        json.Should().Be("[1,2,3]");
    }
}
