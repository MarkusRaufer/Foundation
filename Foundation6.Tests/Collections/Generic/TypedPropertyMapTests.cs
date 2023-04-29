using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Foundation.Collections.Generic;

[TestFixture]
public class TypedPropertyMapTests
{
    [Test]
    public void Ctor_Should_HaveNoProperties_When_Created()
    {
        var objectType = "Person";
        var sut = new TypedPropertyMap(objectType);
        
        sut.Count.Should().Be(0);
        sut.ObjectType.Should().Be(objectType);
    }

    [Test]
    public void Add_Should_Have3Properties_When_Added3Properties()
    {
        var objectType = "Person";
        var sut = new TypedPropertyMap(objectType);

        var firstName = new KeyValuePair<string, object>("FirstName", "Peter");
        var lastName = new KeyValuePair<string, object>("LastName", "Pan");
        var birthday = new KeyValuePair<string, object>("Birthday", new DateOnly(1978, 4, 13));

        sut.Add(firstName);
        sut.Add(lastName);
        sut.Add(birthday);

        sut.ObjectType.Should().Be(objectType);
        sut.Count.Should().Be(3);
        {
            var exists = sut.TryGetValue(firstName.Key, out object? value);
            exists.Should().BeTrue();

            value.Should().NotBeNull();
            value.Should().BeSameAs(firstName.Value);
        }
        {
            var exists = sut.TryGetValue(lastName.Key, out object? value);
            exists.Should().BeTrue();

            value.Should().NotBeNull();
            value.Should().BeSameAs(lastName.Value);
        }
        {
            var exists = sut.TryGetValue(birthday.Key, out object? value);
            exists.Should().BeTrue();

            value.Should().NotBeNull();
            value.Should().BeSameAs(birthday.Value);
        }
    }

    [Test]
    public void Remove_Should_Have2Properties_When_Removed1From3Properties()
    {
        var objectType = "Person";
        var sut = new TypedPropertyMap(objectType);

        var firstName = new KeyValuePair<string, object>("FirstName", "Peter");
        var lastName = new KeyValuePair<string, object>("LastName", "Pan");
        var birthday = new KeyValuePair<string, object>("Birthday", new DateOnly(1978, 4, 13));

        sut.Add(firstName);
        sut.Add(lastName);
        sut.Add(birthday);

        sut.Remove(lastName);

        sut.ObjectType.Should().Be(objectType);
        sut.Count.Should().Be(2);
        {
            var exists = sut.TryGetValue(firstName.Key, out object? value);
            exists.Should().BeTrue();

            value.Should().NotBeNull();
            value.Should().BeSameAs(firstName.Value);
        }
        {
            var exists = sut.TryGetValue(birthday.Key, out object? value);
            exists.Should().BeTrue();

            value.Should().NotBeNull();
            value.Should().BeSameAs(birthday.Value);
        }
    }

    [Test]
    public void Replace_Should_HaveTheReplacedValue_When_IndexerToReplaceIsCalled()
    {
        var objectType = "Person";
        var sut = new TypedPropertyMap(objectType);

        var firstName = new KeyValuePair<string, object>("FirstName", "Peter");
        var lastName = new KeyValuePair<string, object>("LastName", "Pan");
        var birthday = new KeyValuePair<string, object>("Birthday", new DateOnly(1978, 4, 13));

        sut.Add(firstName);
        sut.Add(lastName);
        sut.Add(birthday);

        var newLastName = "Smith";
        sut[lastName.Key] = newLastName;

        sut.ObjectType.Should().Be(objectType);
        sut.Count.Should().Be(3);
        {
            var exists = sut.TryGetValue(firstName.Key, out object? value);
            exists.Should().BeTrue();

            value.Should().NotBeNull();
            value.Should().BeSameAs(firstName.Value);
        }
        {
            var exists = sut.TryGetValue(lastName.Key, out object? value);
            exists.Should().BeTrue();

            value.Should().NotBeNull();
            value.Should().BeSameAs(newLastName);
        }
        {
            var exists = sut.TryGetValue(birthday.Key, out object? value);
            exists.Should().BeTrue();

            value.Should().NotBeNull();
            value.Should().BeSameAs(birthday.Value);
        }
    }
}
