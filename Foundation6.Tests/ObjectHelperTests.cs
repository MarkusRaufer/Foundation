using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation;

[TestFixture]
public class ObjectHelperTests
{
    [Test]
    public void ToString_Should_ReturnValidString_WhenUsingEnumType_And_EnumAsStringIsFalse()
    {
        var str = ObjectHelper.ToString(Month.Jul, valueAsName: false);
        var expected = $"{7}";
        str.Should().Be(expected);
    }

    [Test]
    public void ToString_Should_ReturnValidString_WhenUsingEnumType_AsString()
    {
        var str = ObjectHelper.ToString(Month.Jul, valueAsName: true);
        var expected = $"{Month.Jul}";
        str.Should().Be(expected);
    }
}
