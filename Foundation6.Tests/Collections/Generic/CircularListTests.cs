using NUnit.Framework;
using NUnit.Framework.Internal;
using Shouldly;
using System;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class CircularListTests
{

    [Test]
    public void Add_Should_Have3Elements_When_CapacityIs3AndAdded3()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<int>(capacity);

        // Act
        sut.Add(1);
        sut.Add(2);
        sut.Add(3);

        // Assert
        sut.Count.ShouldBe(3);
        var values = sut.ToArray();
        values.Length.ShouldBe(3);
        values[0].ShouldBe(1);
        values[1].ShouldBe(2);
        values[2].ShouldBe(3);
    }

    [Test]
    public void Add_Should_Have3Elements_When_CapacityIs3AndAdded4()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<int>(capacity);

        // Act
        sut.Add(1);
        sut.Add(2);
        sut.Add(3);
        sut.Add(4);

        // Assert
        sut.Count.ShouldBe(3);
        var values = sut.ToArray();

        values.Length.ShouldBe(3);
        values[0].ShouldBe(2);
        values[1].ShouldBe(3);
        values[2].ShouldBe(4);
    }

    [Test]
    public void Add_Should_Have3Elements_When_CapacityIs3AndAdded5()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<int>(capacity);

        // Act
        sut.Add(1);
        sut.Add(2);
        sut.Add(3);
        sut.Add(4);
        sut.Add(5);

        // Assert
        sut.Count.ShouldBe(3);

        var values = sut.ToArray();
        values.Length.ShouldBe(3);
        values[0].ShouldBe(3);
        values[1].ShouldBe(4);
        values[2].ShouldBe(5);
    }

    [Test]
    public void Clear_Should_Have0Elements_When_CapacityIs3AndAdded3()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<int>(capacity);
        sut.Add(1);
        sut.Add(2);
        sut.Add(3);

        // Act
        sut.Clear();

        // Assert
        sut.Count.ShouldBe(0);
        var values = sut.ToArray();
        values.Length.ShouldBe(0);
    }

    [Test]
    public void Clear_Should_Have2Elements_When_CapacityIs3CalledClearAndAdded2()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<int>(capacity);
        sut.Add(1);
        sut.Add(2);
        sut.Add(3);
        sut.Clear();

        // Act
        sut.Add(4);
        sut.Add(5);

        // Assert
        sut.Count.ShouldBe(2);
        var values = sut.ToArray();

        values.Length.ShouldBe(2);
        values[0].ShouldBe(4);
        values[1].ShouldBe(5);
    }

    [Test]
    public void Ctor_Should_HaveLengthOf0_When_CapacityIs3()
    {
        // Arrange
        var capacity = 3;

        // Act
        var sut = CircularList.New<int>(capacity);

        // Assert
        sut.Count.ShouldBe(0);

        var values = sut.ToArray();
        values.Length.ShouldBe(0);
    }

    [Test]
    public void Ctor_Should_HaveLengthOfThree_When_AddedArrayWithLength3()
    {
        // Arrange
        var numbers = new int[] { 1, 2, 3 };

        // Act
        var sut = CircularList.New<int>(numbers);

        // Assert
        sut.Count.ShouldBe(numbers.Length);
        var values = sut.ToArray();

        values.Length.ShouldBe(numbers.Length);
        values[0].ShouldBe(numbers[0]);
        values[1].ShouldBe(numbers[1]);
        values[2].ShouldBe(numbers[2]);
    }

    [Test]
    [TestCase(0, 1)]
    [TestCase(1, 2)]
    [TestCase(2, 0)]
    [TestCase(3, -1)]
    public void GetInternalIndex_Should_Return2_When_CapacityIs3AndAdded4AndIndexIs3(int value, int index)
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<int>(capacity);
        sut.Add(1);
        sut.Add(2);
        sut.Add(3);
        sut.Add(4);

        // Act
        var result = sut.GetInternalIndex(value);

        // Assert
        result.ShouldBe(index);
    }

    [Test]
    public void Head_Should_ReturnOne_When_CapacityIs3AndAdded4()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<string>(capacity);
        sut.Add("one");
        sut.Add("two");
        sut.Add("three");
        sut.Add("four");

        // Act
        var head = sut.Head;

        // Assert
        head.ShouldBe("two");
    }

    [Test]
    public void HeadIndex_Should_Return1_When_CapacityIs3AndAdded4()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<string>(capacity);
        sut.Add("one");
        sut.Add("two");
        sut.Add("three");
        sut.Add("four");

        // Act
        var index = sut.HeadIndex;

        // Assert
        index.ShouldBe(1);
    }

    [Test]
    [TestCase("1", -1)]
    [TestCase("2", 0)]
    [TestCase("3", 1)]
    [TestCase("4", 2)]
    public void IndexOf_Should_Return0_When_CapacityIs3AndAdded4AndValueIs4(string value, int index)
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<string>(capacity);
        sut.Add("1");
        sut.Add("2");
        sut.Add("3");
        sut.Add("4");

        // Act
        var result = sut.IndexOf(value);

        // Assert
        result.ShouldBe(index);
    }

    [Test]
    public void Indexer_Get_Should_Return3_When_CapacityIs3AndAdded4()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<string>(capacity);
        sut.Add("1");
        sut.Add("2");
        sut.Add("3");
        sut.Add("4");

        // Act
        var item = sut[1];

        // Assert
        item.ShouldBe("3");
    }

    [Test]
    public void Indexer_Set_Should_Return0_When_CapacityIs3AndAdded4AndValueIs4()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<string>(capacity);
        sut.Add("1");
        sut.Add("2");
        sut.Add("3");
        sut.Add("4");

        // Act
        sut[1] = "5";

        // Assert
        var values = sut.ToArray();
        values[0].ShouldBe("2");
        values[1].ShouldBe("5");
        values[2].ShouldBe("4");
    }

    [Test]
    public void InternalIndexOf_Should_Return0_When_CapacityIs3AndAdded4AndValueIs4()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<string>(capacity);
        sut.Add("1");
        sut.Add("2");
        sut.Add("3");
        sut.Add("4");

        // Act
        var index = sut.InternalIndexOf("4");

        // Assert
        index.ShouldBe(0);
    }

    [Test]
    public void Tail_Should_ReturnFour_When_CapacityIs3AndAdded4()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<string>(capacity);
        sut.Add("one");
        sut.Add("two");
        sut.Add("three");
        sut.Add("four");

        // Act
        var tail = sut.Tail;

        // Assert
        tail.ShouldBe("four");
    }

    [Test]
    public void TailIndex_Should_ReturnFour_When_CapacityIs3AndAdded4()
    {
        // Arrange
        var capacity = 3;
        var sut = CircularList.New<string>(capacity);
        sut.Add("one");
        sut.Add("two");
        sut.Add("three");
        sut.Add("four");

        // Act
        var index = sut.TailIndex;

        // Assert
        index.ShouldBe(0);
    }
}
