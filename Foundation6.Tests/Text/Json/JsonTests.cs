using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Text.Json;

[TestFixture]
public class JsonTests
{
    [Test]
    public void ToJson_Should_ReturnValidString_WhenUsingDouble()
    {
        var str = Json.ToJson(123.45);
        str.Should().Be("123.45");
    }

    [Test]
    public void ToJson_Should_ReturnValidString_WhenUsingEnumType_And_EnumAsStringIsFalse()
    {
        var str = Json.ToJson(Month.Jul, enumAsName: false);
        var expected = $"{7}";
        str.Should().Be(expected);
    }

    [Test]
    public void ToJson_Should_ReturnValidString_WhenUsingEnumType_AsString()
    {
        var str = Json.ToJson(Month.Jul);                
        var expected = $"{Month.Jul}";
        str.Should().Be(expected);
    }

    [Test]
    public void ToJson_Should_ReturnValidString_WhenUsingInt()
    {
        var str = Json.ToJson(123);
        str.Should().Be("123");
    }

    [Test]
    public void ToJson_Should_ReturnValidString_WhenUsingIdGuid()
    {
        var guid = Guid.NewGuid();
        var str = Json.ToJson(Id.New(guid));
        
        var expected = $"\"{guid}\"";
        str.Should().Be(expected);
    }

    [Test]
    public void ToJson_Should_ReturnValidString_WhenUsingIdInt()
    {
        var str = Json.ToJson(Id.New(123));
        str.Should().Be("123");
    }

    [Test]
    public void ToJsonProperties_Should_ReturnValidString_WhenUsingIdInt()
    {
        var birthday = new DateTime(2020, 9, 15, 19, 16, 9);
        var properties = new Dictionary<string, object?>()
        {
            { "Name", "Peter" },
            { "Age", 14 },
            { "BirthDay", birthday },
            { "Height", 167.8 },
        };
        
        var json = Json.ToJsonProperties(properties).ToDictionary(x => x.Key, x => x.Value);
        json["Name"].Should().Be("\"Peter\"");
        json["Age"].Should().Be("14");
        json["BirthDay"].Should().Be("\"2020-09-15T19:16:09\"");
        json["Height"].Should().Be("167.8");
    }
}
