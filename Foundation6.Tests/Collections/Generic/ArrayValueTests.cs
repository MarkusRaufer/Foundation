using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class ArrayValueTests
{
    [Test]
    public void Cast_Should_ImplicitlyCastToEquatableArray_When_CastFrom_Array_To_EquatableArray()
    {
        var array = new int[] { 1, 2, 3 };

        ArrayValue<int> sut = array;

        Assert.IsTrue(array.SequenceEqual(sut));
    }

    [Test]
    public void Cast_Should_ImplicitlyCastToEquatableArray_When_EquatableArrayIsMethodArgument()
    {
        var ints = new int[] { 1, 2, 3 };

        bool method(ArrayValue<int> sut)
        {
            return ints.SequenceEqual(sut);
        }

        Assert.IsTrue(method(ints));
    }

    [Test]
    public void Cast_Should_ImplicitlyCastToArray_When_CastFrom_EquatableArray_To_Array()
    {
        var array = new int[] { 1, 2, 3 };

        int[] ints = ArrayValue.New(array);

        Assert.IsTrue(array.SequenceEqual(ints));
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_SameSize_SameValues_DifferentPositions_OfElementsAreSame()
    {
        var arr1 = new int[] { 1, 2, 3 };
        var arr2 = new int[] { 3, 2, 1 };

        Assert.IsFalse(arr1.Equals(arr2));

        var eqArr1 = ArrayValue.New(arr1);
        var eqArr2 = ArrayValue.New(arr2);

        Assert.IsFalse(eqArr1.Equals(eqArr2));
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_SameSize_SameValues_SamePositions_OfElementsAreSame()
    {
        var arr1 = new int[] { 1, 2, 3 };
        var arr2 = new int[] { 1, 2, 3 };

        Assert.IsFalse(arr1.Equals(arr2));

        var eqArr1 = ArrayValue.New(arr1);
        var eqArr2 = ArrayValue.New(arr2);

        Assert.IsTrue(eqArr1.Equals(eqArr2));
    }

    [Test]
    public void New_Should_ReturnAnEquatableArrayWith3Elements_When_ArrayArgumentHas3Elements()
    {
        var array = new int[] { 1, 2, 3 };

        var eqArray = ArrayValue.New(array);

        Assert.AreEqual(array.Length, eqArray.Length);
    }
}
