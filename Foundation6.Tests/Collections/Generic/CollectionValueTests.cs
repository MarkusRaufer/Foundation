using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class CollectionValueTests
{
    [Test]
    public void Cast_Should_ImplicitlyCastToEquatableArray_When_CastFrom_Array_To_EquatableArray()
    {
        var array = new int[] { 1, 2, 3 };

        CollectionValue<int> sut = array;

        Assert.IsTrue(array.SequenceEqual(sut));
    }

    [Test]
    public void Cast_Should_ImplicitlyCastToEquatableArray_When_EquatableBagIsMethodArgument()
    {
        var ints = new int[] { 1, 2, 3 };

        bool method(CollectionValue<int> sut)
        {
            return ints.SequenceEqual(sut);
        }

        Assert.IsTrue(method(ints));
    }

    [Test]
    public void Cast_Should_ImplicitlyCastToArray_When_CastFrom_EquatableBag_To_Array()
    {
        var array = new int[] { 1, 2, 3 };

        int[] ints = CollectionValue.New(array);

        Assert.IsTrue(array.SequenceEqual(ints));
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_SameSize_SameValues_DifferentPositions_OfElementsAreSame()
    {
        var arr1 = new int[] { 1, 2, 3 };
        var arr2 = new int[] { 3, 2, 1 };

        Assert.IsFalse(arr1.Equals(arr2));

        var eqArr1 = CollectionValue.New(arr1);
        var eqArr2 = CollectionValue.New(arr2);

        Assert.IsTrue(eqArr1.Equals(eqArr2));
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_SameSize_SameValues_SamePositions_OfElementsAreSame()
    {
        var arr1 = new int[] { 1, 2, 3 };
        var arr2 = new int[] { 1, 2, 3 };

        Assert.IsFalse(arr1.Equals(arr2));

        var eqArr1 = CollectionValue.New(arr1);
        var eqArr2 = CollectionValue.New(arr2);

        Assert.IsTrue(eqArr1.Equals(eqArr2));
    }

    [Test]
    public void GetHashCode_Should_ReturnSameHashCode_When_SameSizes_SameValues_DifferentPositions()
    {
        var eqArr1 = CollectionValue.New(1, 2, 3);
        var eqArr2 = CollectionValue.New(3, 2, 1);

        Assert.AreEqual(eqArr1.GetHashCode(), eqArr2.GetHashCode());
    }

    [Test]
    public void GetHashCode_Should_ReturnSameHashCode_When_SameSizes_SameValues_SamePositions()
    {
        var eqArr1 = CollectionValue.New(1, 2, 3);
        var eqArr2 = CollectionValue.New(1, 2, 3);

        Assert.AreEqual(eqArr1.GetHashCode(), eqArr2.GetHashCode());
    }

    [Test]
    public void New_Should_ReturnAnEquatableArrayWith3Elements_When_ArrayArgumentHas3Elements()
    {
        var array = new int[] { 1, 2, 3 };

        var eqArray = ArrayValue.New(array);

        Assert.AreEqual(array.Length, eqArray.Length);
    }
}
