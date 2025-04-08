using NUnit.Framework;
using Shouldly;

namespace Foundation;

[TestFixture]
public class EnumHelperTests
{
    [Test]
    public void ToString_Should_ReturnValidString_WhenUsingEnumType_And_EnumAsStringIsFalse()
    {
        var str = EnumHelper.ToString(Month.Jul, nameAsValue: false);
        var expected = $"{7}";
        str.ShouldBe(expected);
    }

    [Test]
    public void ToString_Should_ReturnValidString_WhenUsingEnumType_AsString()
    {
        var str = EnumHelper.ToString(Month.Jul, nameAsValue: true);
        var expected = $"{Month.Jul}";
        str.ShouldBe(expected);
    }
}
