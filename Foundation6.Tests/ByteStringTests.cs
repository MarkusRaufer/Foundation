using FluentAssertions;
using Foundation.Text.Json.Serialization;
using NUnit.Framework;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation;

[TestFixture]
public class ByteStringTests
{
    private class MyClass
    {
        [JsonConverter(typeof(ByteStringConverter))]
        public ByteString? Text { get; set; }
    }

    [Test]
    public void CompareTo_Should_Return32_When_FirstByteStringIsGreater()
    {
        var sut1 = new ByteString("test".ToByteArray());
        var sut2 = new ByteString("tesT".ToByteArray());

        var cmp = sut1.CompareTo(sut2);
        cmp.Should().Be(32); // the ordinal distance.
    }

    [Test]
    public void CompareTo_Should_ReturnMinus32_When_FirstByteStringIsSmaller()
    {
        var sut1 = new ByteString("Test".ToByteArray());
        var sut2 = new ByteString("test".ToByteArray());

        var cmp = sut1.CompareTo(sut2);
        cmp.Should().Be(-32); // the ordinal distance.

    }

    [Test]
    public void CompareTo_Should_Return0_When_ByteStringsAreEqual()
    {
        var sut1 = new ByteString("test".ToByteArray());
        var sut2 = new ByteString("test".ToByteArray());

        var cmp = sut1.CompareTo(sut2);
        cmp.Should().Be(0);
    }

    [Test]
    public void Concat_Should_ReturnAConcatenatedByteString_When_UsingAnArrayOfByteStrings()
    {
        var sut1 = new ByteString("12".ToByteArray());
        var sut2 = new ByteString("34".ToByteArray());
        var sut3 = new ByteString("56".ToByteArray());

        var concatenated = ByteString.Concat(new[] { sut1, sut2, sut3 });

        var expected = new ByteString("123456".ToByteArray());
        var eq = concatenated == expected;
        eq.Should().BeTrue();
    }


    [Test]
    public void Deserialze_Should_ReturnEqualByteString_When_Using_ByteStringConverterWithDeserialize()
    {
        var expected = new ByteString("test".ToByteArray());

        var json = JsonSerializer.Serialize(expected, new JsonSerializerOptions
        {
            Converters = { new ByteStringConverter() }
        });

        var sut = JsonSerializer.Deserialize<ByteString>(json, new JsonSerializerOptions
        {
            Converters = { new ByteStringConverter() }
        });

        var notnull = sut is not null;
        notnull.Should().BeTrue();

        var eq = sut! == expected;
        eq.Should().BeTrue();
    }

    [Test]
    public void Deserialze_Should_ReturnEqualByteString_When_Using_ByteStringConverterWithDeserializeAndSerialize()
    {
        var expected = new ByteString("test".ToByteArray());

        var json = JsonSerializer.Serialize(expected, new JsonSerializerOptions
        {
            Converters = { new ByteStringConverter() }
        });

        var sut = JsonSerializer.Deserialize<ByteString>(json, new JsonSerializerOptions
        {
            Converters = { new ByteStringConverter() }
        });

        var notnull = sut is not null;
        notnull.Should().BeTrue();

        var eq = sut! == expected;
        eq.Should().BeTrue();
    }

    [Test]
    public void Deserialze_Should_ReturnEqualMyClass_When_Using_ByteStringConverterAsAttribute()
    {
        var byteString = new ByteString("test".ToByteArray());

        var expected = new MyClass { Text = byteString };

        var json = JsonSerializer.Serialize(expected);

        var actual = JsonSerializer.Deserialize<MyClass>(json);

        actual.Should().NotBeNull();
        
        var eq = actual!.Text! == expected.Text;
        eq.Should().BeTrue();
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_BytesAreNotEqual()
    {
        var sut1 = new ByteString("Test".ToByteArray());
        var sut2 = new ByteString("test".ToByteArray());

        var eq = sut1 == sut2;

        eq.Should().BeFalse();
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_BytesAreEqual()
    {
        var sut1 = new ByteString("test".ToByteArray());
        var sut2 = new ByteString("test".ToByteArray());

        var eq = sut1 == sut2;

        eq.Should().BeTrue();
    }

    [Test]
    public void Greater_Should_ReturnTrue_When_FirstByteStringIsGreater()
    {
        var sut1 = new ByteString("test".ToByteArray());
        var sut2 = new ByteString("tesT".ToByteArray());

        var cmp = sut1 > sut2;
        cmp.Should().BeTrue();
    }

    [Test]
    public void Serialze_Should_ReturnValidJsonString_When_Using_SerializeWithoutConverter()
    {
        var sut = new ByteString("test".ToByteArray());

        var json = JsonSerializer.Serialize(sut);

        var expected = $"[{string.Join(",", sut)}]";
        json.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Smaller_Should_ReturnTrue_When_FirstByteStringIsSmaller()
    {
        var sut1 = new ByteString("Test".ToByteArray());
        var sut2 = new ByteString("test".ToByteArray());

        var cmp = sut1 < sut2;
        cmp.Should().BeTrue();
    }
}
