using Foundation.Text.Json.Serialization;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Text.Json;

namespace Foundation.Collections.Generic;

[TestFixture]
public class DictionaryValueTests
{
    [Test]
    public void Ctor_Should_ReturnValidDictionaryValue_When_Using_Tuples()
    {
        var sut = new DictionaryValue<string, int>(
        [
            ("one", 1),
            ("two", 2),
            ("three", 3)
        ]);

        var expected = new DictionaryValue<string, int>(new Dictionary<string, int>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3
        });

        sut.ShouldNotBeNull();
        sut.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public void Deserialze_Should_ReturnValidJsonString_When_UsingDictionaryValueJsonConverter()
    {
        // Arrange
        var keyValues = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        var sut = new DictionaryValue<string, object>(keyValues);
        var json = JsonSerializer.Serialize(sut);
        var options = new JsonSerializerOptions
        {
            Converters = { new DictionaryValueJsonConverter<string, object>() }
        };

        // Act
        var deserialized = JsonSerializer.Deserialize<DictionaryValue<string, object>>(json, options);

        // Assert
        sut.ShouldBe(deserialized);
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_KeysAreDifferent()
    {
        var keyValues1 = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        var keyValues2 = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "four", 3 }
        };

        var sut1 = new DictionaryValue<string, object>(keyValues1);
        var sut2 = DictionaryValue.New(keyValues2);

        Assert.AreNotEqual(sut1, sut2);
        Assert.AreNotEqual(sut1.GetHashCode(), sut2.GetHashCode());
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_KeysAndValuesAreDifferent()
    {
        var keyValues1 = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        var keyValues2 = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "four", 4 }
        };

        var sut1 = new DictionaryValue<string, object>(keyValues1);
        var sut2 = new DictionaryValue<string, object>(keyValues2);

        Assert.AreNotEqual(sut1, sut2);
        Assert.AreNotEqual(sut1.GetHashCode(), sut2.GetHashCode());
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_ValuesAreDifferent()
    {
        var keyValues1 = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        var keyValues2 = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 4 }
        };

        var sut1 = new DictionaryValue<string, object>(keyValues1);
        var sut2 = new DictionaryValue<string, object>(keyValues2);

        Assert.AreNotEqual(sut1, sut2);
        Assert.AreNotEqual(sut1.GetHashCode(), sut2.GetHashCode());
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_KeysAndValuesAreSame()
    {
        var keyValues1 = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        var keyValues2 = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        var sut1 = new DictionaryValue<string, object>(keyValues1);
        var sut2 = new DictionaryValue<string, object>(keyValues2);

        Assert.AreEqual(sut1, sut2);
        Assert.AreEqual(sut1.GetHashCode(), sut2.GetHashCode());
    }

    [Test]
    public void New_Should_ReturnDictionaryValue_When_Using_Tuples()
    {
        var sut = DictionaryValue.New<string, object>(("one", 1), ("two", 2), ("three", 3));

        var expected = new DictionaryValue<string, object>(new Dictionary<string, object>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3
        });

        sut.ShouldNotBeNull();
        sut.ShouldBeEquivalentTo(expected);

    }
    [Test]
    public void New_Should_ReturnDictionaryValue_When_Using_TupleList()
    {
        var sut = DictionaryValue.New<string, object>(
        [
            ("one",   1),
            ("two",   2),
            ("three", 3)
        ]);

        var expected = new DictionaryValue<string, object>(new Dictionary<string, object>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3
        });

        sut.ShouldNotBeNull();
        sut.ShouldBe(expected);
    }

    [Test]
    public void NewWith_Should_ReturnDictionaryWithNewValues_When_ReplacementsHaveNewValues()
    {
        var keyValues1 = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        var keyValues2 = new Dictionary<string, object>
        {
            { "one", 10 },
            { "two", 2 },
            { "four", 40 }
        };

        var sut = new DictionaryValue<string, object>(keyValues1);

        var sut2 = sut.NewWith(keyValues2);

        sut2.Count.ShouldBe(sut.Count);
        sut2.ShouldContain(new KeyValuePair<string, object>("one", 10));
        sut2.ShouldContain(new KeyValuePair<string, object>("two", 2));
        sut2.ShouldContain(new KeyValuePair<string, object>("three", 3));
    }

    [Test]
    public void Serialze_Should_ReturnAValidJsonString_When_CalledJsonSerializerSerialize()
    {
        // Arrange
        var keyValues = new Dictionary<string, object>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 }
        };

        var sut = new DictionaryValue<string, object>(keyValues);

        // Act
        var json = JsonSerializer.Serialize(sut);

        // Assert
        json.ShouldBe("""{"one":1,"two":2,"three":3}""");
    }
}
