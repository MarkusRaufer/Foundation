using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Text.Json;

[TestFixture]
public class JsonTests
{
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
}
