using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

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

        sut.Should().NotBeNull();
        sut.Should().BeEquivalentTo(expected);
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

        var expected = new DictionaryValue<string, int>(new Dictionary<string, int>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3
        });

        sut.Should().NotBeNull();
        sut.Should().BeEquivalentTo(expected);

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

        var expected = new DictionaryValue<string, int>(new Dictionary<string, int>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3
        });

        sut.Should().NotBeNull();
        sut.Should().BeEquivalentTo(expected);
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

        sut2.Count.Should().Be(sut.Count);
        sut2.Should().Contain(new KeyValuePair<string, object>("one", 10));
        sut2.Should().Contain(new KeyValuePair<string, object>("two", 2));
        sut2.Should().Contain(new KeyValuePair<string, object>("three", 3));
    }
}
