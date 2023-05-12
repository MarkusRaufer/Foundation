using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Collections;

[TestFixture]
public class EnumerableConditionalsTests
{
    [Test]
    public void ExistsType_Should_BeFalse_When_NotAllExactTypesExist()
    {
        var objects = new object[] { "1", 2, 3.5, DateTime.Now.ToTimeOnly() };

        objects.ExistsType(false, new Type[] { typeof(DateOnly), typeof(TimeOnly) })
               .Should()
               .BeFalse();
    }

    [Test]
    public void ExistsType_Should_BeFalse_When_NotAllTypesExist_AssignableTypeIsFalse()
    {
        var number = new List<int> { 1, 2, 3 };

        var objects = new object[] { "1", 2, DateTime.Now.ToDateOnly(), number, DateTime.Now.ToTimeOnly() };

        objects.ExistsType(false, new Type[] { typeof(DateOnly), typeof(TimeOnly), typeof(System.Collections.IEnumerable) })
               .Should()
               .BeFalse();
    }

    [Test]
    public void ExistsType_Should_BeTrue_When_AllExactTypesExist()
    {
        var objects = new object[] { "1", 2, DateTime.Now.ToDateOnly(), 3.5, DateTime.Now.ToTimeOnly() };

        objects.ExistsType(false, new Type[] { typeof(DateOnly), typeof(TimeOnly) })
               .Should()
               .BeTrue();
    }

    [Test]
    public void ExistsType_Should_BeTrue_When_AllTypesExist_AssignableTypeIsTrue()
    {
        var number = new List<int> { 1, 2, 3 };

        var objects = new object[] { "1", 2, DateTime.Now.ToDateOnly(), number, DateTime.Now.ToTimeOnly() };

        objects.ExistsType(true, new Type[] { typeof(DateOnly), typeof(TimeOnly), typeof(System.Collections.IEnumerable) })
               .Should()
               .BeTrue();
    }
}
