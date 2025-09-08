using FluentAssertions;
using Foundation.Linq.Expressions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        int[] sut = ArrayValue.New(array);

        Assert.IsTrue(array.SequenceEqual(sut));
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_SameSize_SameValues_DifferentPositions_OfElementsAreSame()
    {
        var arr1 = new int[] { 1, 2, 3 };
        var arr2 = new int[] { 3, 2, 1 };

        Assert.IsFalse(arr1.Equals(arr2));

        var sut1 = ArrayValue.New(arr1);
        var sut2 = ArrayValue.New(arr2);

        Assert.IsFalse(sut1.Equals(sut2));
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_SameSize_SameValues_SamePositions_OfElementsAreSame()
    {
        var arr1 = new int[] { 1, 2, 3 };
        var arr2 = new int[] { 1, 2, 3 };

        Assert.IsFalse(arr1.Equals(arr2));

        var sut1 = ArrayValue.New(arr1);
        var sut2 = ArrayValue.New(arr2);

        Assert.IsTrue(sut1.Equals(sut2));
    }

    [Test]
    public void Find_Should_Return4_When_ArrayContains4()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };

        var sut = ArrayValue.New(array);

        var result = sut.Find(x => x == 4);
        result.Should().Be(4);
    }

    [Test]
    public void FindIndex_Should_ReturnIndex3_When_Searching4_And_ArrayContains4()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };

        var sut = ArrayValue.New(array);

        var index = sut.FindIndex(x => x == 4);
        index.Should().Be(3);
    }

    [Test]
    public void FindIndex_Should_ReturnIndex7_When_Searching4_And_StartIndexIs4_And_ArrayContains4()
    {
        var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 4, 8, 9 };

        var sut = ArrayValue.New(array);

        var index = sut.FindIndex(4, x => x == 4);
        index.Should().Be(7);
    }

    [Test]
    public void New_Should_ReturnAnEquatableArrayWith3Elements_When_ArrayArgumentHas3Elements()
    {
        var array = new int[] { 1, 2, 3 };

        var sut = ArrayValue.New(array);

        Assert.AreEqual(array.Length, sut.Length);
    }

    [Test]
    public void Serialize_Should_ReturnAnEquatableArrayWith3Elements_When_ArrayArgumentHas3Elements()
    {
        
        var array = new int[] { 1, 2, 3 };

        var mc = new MyTest<int>(array);
        var options = new JsonSerializerOptions
        {
            IncludeFields = true
        };
        var j = JsonSerializer.Serialize(mc);

        var ds = JsonSerializer.Deserialize<MyTest<int>>(j);


        var sut = ArrayValue.New(array);

        var json = JsonSerializer.Serialize(sut);

        var deserialized = JsonSerializer.Deserialize<ArrayValue<int>>(json);

        Assert.AreEqual(array.Length, sut.Length);
    }
}
internal class MyTest<T>
{
    [JsonInclude]
    private T[] _array;

    [JsonConstructor]
    public MyTest(T[] array)
    {
        _array = array;
    }

    [JsonIgnore]
    public ReadOnlySpan<T> Array => _array;
}