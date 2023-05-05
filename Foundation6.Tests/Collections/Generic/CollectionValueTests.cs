using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class CollectionValueTests
{
    [Test]
    public void Cast_Should_ImplicitlyCastArrayToCollectionValue_When_AssigningTo_CollectionValue()
    {
        {
            CollectionValue<int> sut = new int[] { };
            
            sut.Should().NotBeNull();
            sut.Count.Should().Be(0);
        }
        {
            var array = new int[] { 1, 2, 3 };

            CollectionValue<int> sut = array;

            sut.Should().NotBeNull();
            sut.Count.Should().Be(array.Length);
        }
    }

    [Test]
    public void Cast_Should_HaveTheValuesOfArray_When_Calling_Method()
    {
        var numbers = new int[] { 1, 2, 3 };

        bool method(CollectionValue<int> sut)
        {
            return numbers.SequenceEqual(sut);
        }

        method(numbers).Should().BeTrue();
    }

    [Test]
    public void Cast_Should_ImplicitlyCastToArray_When_AssigningCollectionValueToArray()
    {
        var array = new int[] { 1, 2, 3 };

        int[] numbers = CollectionValue.New(array);

        array.SequenceEqual(numbers).Should().BeTrue();
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_SameSize_SameValues_DifferentPositions_OfElements()
    {
        var arr1 = new int[] { 1, 2, 3 };
        var arr2 = new int[] { 3, 2, 1 };

        arr1.Equals(arr2).Should().BeFalse();

        var sut1 = CollectionValue.New(arr1);
        var sut2 = CollectionValue.New(arr2);

        sut1.Equals(sut2).Should().BeTrue();
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_SameSize_SameValues_SamePositions_OfElements()
    {
        var arr1 = new int[] { 1, 2, 3 };
        var arr2 = new int[] { 1, 2, 3 };

        arr1.Equals(arr2).Should().BeFalse();

        var sut1 = CollectionValue.New(arr1);
        var sut2 = CollectionValue.New(arr2);

        sut1.Equals(sut2).Should().BeTrue();
    }

    [Test]
    public void GetHashCode_Should_ReturnSameHashCode_When_SameSizes_SameValues_DifferentPositions_OfElements()
    {
        var sut1 = CollectionValue.New(1, 2, 3);
        var sut2 = CollectionValue.New(3, 2, 1);

        sut1.GetHashCode().Should().Be(sut2.GetHashCode());
    }

    [Test]
    public void GetHashCode_Should_ReturnSameHashCode_When_SameSizes_SameValues_SamePositions_OfElements()
    {
        var sut1 = CollectionValue.New(1, 2, 3);
        var sut2 = CollectionValue.New(1, 2, 3);

        sut1.GetHashCode().Should().Be(sut2.GetHashCode());
    }

    [Test]
    public void New_Should_ReturnCollectionValue_When_Called()
    {
        var array = new int[] { 1, 2, 3 };

        var sut = CollectionValue.New(array);

        sut.Should().NotBeNull();
    }

    [Test]
    public void New_Should_ReturnCollectionValueWith3Elements_When_ArrayArgumentHas3Elements()
    {
        var array = new int[] { 1, 2, 3 };

        var sut = CollectionValue.New(array);

        sut.Count.Should().Be(array.Length);
        sut.SequenceEqual(array).Should().BeTrue();
    }
}
