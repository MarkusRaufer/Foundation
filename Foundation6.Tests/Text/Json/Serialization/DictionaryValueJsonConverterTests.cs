using Shouldly;
using Foundation.Collections.Generic;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text.Json;

namespace Foundation.Text.Json.Serialization;

[TestFixture]
public class DictionaryValueJsonConverterTests
{
    [Test]
    public void Deserialze_Should_ReturnAValidDictionaryValue_When_Used_DictionaryValueJsonConverter()
    {
        var keyValues = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        var expected = DictionaryValue.New(keyValues);

        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new DictionaryValueJsonConverter<string, object>() }
        };

        var jsonDict = JsonSerializer.Serialize(keyValues);

        var json = JsonSerializer.Serialize(expected, jsonOptions);

        var deserialized = JsonSerializer.Deserialize<DictionaryValue<string, object>>(json, jsonOptions);

        var eq = expected.Equals(deserialized);
        eq.ShouldBeTrue();
    }

    [Test]
    public void SerialzeToJson_Should_ReturnJsonString_When_Called_JsonSerializer_Serialize()
    {
        var sut = HashSetValue.New([1, 2, 3]);

        var json = JsonSerializer.Serialize(sut);

        json.ShouldBe("[1,2,3]");
    }
}
