namespace Foundation.Collections.Generic;

using Foundation.ComponentModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



[TestFixture]
public class EquatableMapTests
{
    [Test]
    public void Ctor_Should_HaveElements_When_Ctor_With_Elements()
    {
        var properties = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();
        var sut = new MapValue<string, object>(properties);

        Assert.AreEqual(properties.Length, sut.Count);
        CollectionAssert.Contains(sut, properties[0]);
        CollectionAssert.Contains(sut, properties[1]);
        CollectionAssert.Contains(sut, properties[2]);
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_MapsHaveSameKeyValues()
    {
        var properties = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();
        var map1 = new MapValue<string, object>(properties);
        var map2 = new MapValue<string, object>(properties);

        Assert.IsTrue(map1.Equals(map2));
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_MapsHaveDifferentKeys()
    {
        var properties1 = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();
        var properties2 = Pair.CreateMany<string, object>(("First", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();

        var map1 = new MapValue<string, object>(properties1);
        var map2 = new MapValue<string, object>(properties2);

        Assert.IsFalse(map1.Equals(map2));
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_MapsHaveDifferentValues()
    {
        var properties1 = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();
        var properties2 = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Duck"), ("Age", 16)).ToArray();

        var map1 = new MapValue<string, object>(properties1);
        var map2 = new MapValue<string, object>(properties2);

        Assert.IsFalse(map1.Equals(map2));
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_MapsHaveDifferentKeyValues()
    {
        var properties = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();

        var map1 = new MapValue<string, object>(properties);
        var map2 = new MapValue<string, object>(properties);

        map2["LastName"] = "Duck";
        Assert.IsFalse(map1.Equals(map2));
    }
}

