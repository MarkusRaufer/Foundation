﻿using FluentAssertions;
using Foundation.Collections.Generic;
using NUnit.Framework;
using System.Text.Json;

namespace Foundation.Text.Json.Serialization;

[TestFixture]
public class EnumerableConverterTests
{
    [Test]
    public void DeserialzeToJson_Should_ReturnJsonString_When_Called_JsonSerializer_Deserialize_With_HashSetValue()
    {
        var sut = HashSetValue.New([1, 2, 3]);

        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new EnumerableJsonConverter<int, HashSetValue<int>>(items => items.ToHashSetValue())
            }
        };

        var json = JsonSerializer.Serialize(sut, options);

        var deserialized = JsonSerializer.Deserialize<HashSetValue<int>>(json, options);

        sut.Should().BeEquivalentTo(deserialized);
    }

    [Test]
    public void DeserialzeToJson_Should_ReturnJsonString_When_Called_JsonSerializer_Deserialize_With_NonEmptySetValue()
    {
        var sut = NonEmptyHashSetValue.New([1, 2, 3]);

        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new EnumerableJsonConverter<int, NonEmptyHashSetValue<int>>(items => items.ToNonEmptyHashSetValue())
            }
        };

        var json = JsonSerializer.Serialize(sut, options);

        var deserialized = JsonSerializer.Deserialize<NonEmptyHashSetValue<int>>(json, options);

        sut.Should().BeEquivalentTo(deserialized);
    }

    [Test]
    public void SerialzeToJson_Should_ReturnJsonString_When_Called_JsonSerializer_Serialize()
    {
        var numbers = NonEmptyHashSetValue.New([1, 2, 3]);

        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new EnumerableJsonConverter<int, NonEmptyHashSetValue<int>>(items => items.ToNonEmptyHashSetValue())
            }
        };

        var json = JsonSerializer.Serialize(numbers, options);

        json.Should().Be("[1,2,3]");
    }
}
