using Foundation.Collections.Generic;
using NUnit.Framework;
using Shouldly;
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

        var exists = objects.ExistsType(false, new Type[] { typeof(DateOnly), typeof(TimeOnly) });
        exists.ShouldBeFalse();
    }

    [Test]
    public void ExistsType_Should_BeFalse_When_NotAllTypesExist_AssignableTypeIsFalse()
    {
        var number = new List<int> { 1, 2, 3 };

        var objects = new object[] { "1", 2, DateTime.Now.ToDateOnly(), number, DateTime.Now.ToTimeOnly() };

        var exists = objects.ExistsType(false, new Type[] { typeof(DateOnly), typeof(TimeOnly), typeof(System.Collections.IEnumerable) });
        exists.ShouldBeFalse();
    }

    [Test]
    public void ExistsType_Should_BeTrue_When_AllExactTypesExist()
    {
        var objects = new object[] { "1", 2, DateTime.Now.ToDateOnly(), 3.5, DateTime.Now.ToTimeOnly() };

        var exists = objects.ExistsType(false, new Type[] { typeof(DateOnly), typeof(TimeOnly) });
        exists.ShouldBeTrue();
    }

    [Test]
    public void ExistsType_Should_BeTrue_When_AllTypesExist_AssignableTypeIsTrue()
    {
        var number = new List<int> { 1, 2, 3 };

        var objects = new object[] { "1", 2, DateTime.Now.ToDateOnly(), number, DateTime.Now.ToTimeOnly() };

        var exists = objects.ExistsType(true, new Type[] { typeof(DateOnly), typeof(TimeOnly), typeof(System.Collections.IEnumerable) });
        exists.ShouldBeTrue();
    }


    [Test]
    public void OfType_Should_BeTrue_When_AllTypesExist_AssignableTypeIsTrue()
    {
        var numbers = new List<int> { 1, 2, 3 };

        var date = DateTime.Now.ToDateOnly();
        var time = DateTime.Now.ToTimeOnly();
        var objects = new object[] { "1", 2, date, numbers, time };

        var filtered = objects.OfType(true, [typeof(DateOnly), typeof(TimeOnly), typeof(System.Collections.IEnumerable)])
                              .ToObjectArray();
        filtered.Length.ShouldBe(4);
        filtered[0].ShouldBe("1");
        filtered[1].ShouldBe(date);
        filtered[2].ShouldBe(numbers);
        filtered[3].ShouldBe(time);
    }

    [Test]
    public void OrEmpty_Should_ReturnAnEmptyList_When_ItemsIsNull()
    {
        // Arrange
        IEnumerable<int>? numbers = null;

        // Act
        var result = numbers.OrEmpty().ToArray();

        // Assert

        result.Length.ShouldBe(0);
        
    }
}
