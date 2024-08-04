using FluentAssertions;
using NUnit.Framework;

namespace Foundation;

[TestFixture]
public class EnumHelperTests
{
    [Test]
    public void ToString_Should_ReturnValidString_WhenUsingEnumType_And_EnumAsStringIsFalse()
    {
        var str = EnumHelper.ToString(Month.Jul, valueAsName: false);
        var expected = $"{7}";
        str.Should().Be(expected);
    }

    [Test]
    public void ToString_Should_ReturnValidString_WhenUsingEnumType_AsString()
    {
        var str = EnumHelper.ToString(Month.Jul, valueAsName: true);
        var expected = $"{Month.Jul}";
        str.Should().Be(expected);
    }
}
