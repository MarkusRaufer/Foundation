using NUnit.Framework;
using System.Collections.Generic;

namespace Foundation.Collections.Generic;

[TestFixture]
public class DictionaryValueTests
{
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
        var sut2 = new DictionaryValue<string, object>(keyValues2);

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
}
