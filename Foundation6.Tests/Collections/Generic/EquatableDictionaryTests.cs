namespace Foundation.Collections.Generic;

using FluentAssertions;
using Foundation.ComponentModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



[TestFixture]
public class EquatableDictionaryTests
{
    [Test]
    public void Ctor_Should_HaveElements_When_Ctor_With_Elements()
    {
        var properties = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();
        var sut = new EquatableDictionary<string, object>(properties);

        sut.Count.Should().Be(properties.Length);
        sut.Should().Contain(properties[0]);
        sut.Should().Contain(properties[1]);
        sut.Should().Contain(properties[2]);
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_MapsHaveSameKeyValues()
    {
        var properties = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();
        var map1 = new EquatableDictionary<string, object>(properties);
        var map2 = new EquatableDictionary<string, object>(properties);

        map1.Equals(map2).Should().BeTrue();
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_MapsHaveDifferentKeys()
    {
        var properties1 = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();
        var properties2 = Pair.CreateMany<string, object>(("First", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();

        var map1 = new EquatableDictionary<string, object>(properties1);
        var map2 = new EquatableDictionary<string, object>(properties2);

        map1.Equals(map2).Should().BeFalse();
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_MapsHaveDifferentValues()
    {
        var properties1 = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();
        var properties2 = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Duck"), ("Age", 16)).ToArray();

        var map1 = new EquatableDictionary<string, object>(properties1);
        var map2 = new EquatableDictionary<string, object>(properties2);

        map1.Equals(map2).Should().BeFalse();
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_MapsHaveDifferentKeyValues()
    {
        var properties = Pair.CreateMany<string, object>(("FirstName", "Peter"), ("LastName", "Pan"), ("Age", 16)).ToArray();

        var map1 = new EquatableDictionary<string, object>(properties);
        var map2 = new EquatableDictionary<string, object>(properties);

        map2["LastName"] = "Duck";
        map1.Equals(map2).Should().BeFalse();
    }
}
