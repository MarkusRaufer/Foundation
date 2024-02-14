using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class ResilientLinkedListTests
{
    [Test]
    public void Ctor_Should_NotBeNull_When_Created()
    {
        var sut = new ResilientLinkedList<int>();

        sut.Should().NotBeNull();
        sut.Count.Should().Be(0);
    }

    [Test]
    public void Add_Should_NotBeNull_When_Created()
    {
        var sut = new ResilientLinkedList<int>();
        var numberOfElements = 10;
        Enumerable.Range(1, numberOfElements).ForEach(sut.Add);

        sut.Count.Should().Be(numberOfElements);
    }

    [Test]
    public void First_Should_NotBeNull_When_AddedSingleItem()
    {
        var sut = new ResilientLinkedList<int>() { 1 };

        sut.Count.Should().Be(1);
        sut.First.Should().Be(1);
        sut.Last.Should().Be(1);
    }

    [Test]
    public void First_Should_Be2_When_Added3Items()
    {
        var sut = new ResilientLinkedList<int>() { 2, 3, 4 };

        sut.Count.Should().Be(3);
        sut.First.Should().Be(2);
        sut.Last.Should().Be(4);
    }

    [Test]
    public void Remove_Should_ReturnFalse_When_ItemNotInList()
    {
        var sut = new ResilientLinkedList<int>() { 2, 3, 4 };

        var removed = sut.Remove(1);

        removed.Should().BeFalse();
        sut.Count.Should().Be(3);
        sut.First.Should().Be(2);
        sut.Last.Should().Be(4);
    }

    [Test]
    public void Remove_Should_Have2Elements_When_RemovingItem2OfThree()
    {
        var sut = new ResilientLinkedList<int>() { 2, 3, 4 };

        sut.Remove(2);

        sut.Count.Should().Be(2);
        sut.First.Should().Be(3);
        sut.Last.Should().Be(4);
    }

    [Test]
    public void Remove_Should_Have2Elements_When_RemovingItem3OfThree()
    {
        var sut = new ResilientLinkedList<int>() { 2, 3, 4 };

        sut.Remove(3);

        sut.Count.Should().Be(2);
        sut.First.Should().Be(2);
        sut.Last.Should().Be(4);
    }

    [Test]
    public void Remove_Should_Have2Elements_When_RemovingItem4OfThree()
    {
        var sut = new ResilientLinkedList<int>() { 2, 3, 4 };

        sut.Remove(4);

        sut.Count.Should().Be(2);
        sut.First.Should().Be(2);
        sut.Last.Should().Be(3);
    }

    [Test]
    public void Remove_Should_RemoveAllItems_When_IteratingAndRemovingFirstItemsInALoop()
    {
        var sut = new ResilientLinkedList<int>();
        var numberOfElements = 10;
        Enumerable.Range(1, numberOfElements).ForEach(sut.Add);

        foreach (var number in sut)
        {
            sut.Remove(number);
        }

        sut.Count.Should().Be(0);
        sut.First.Should().Be(0);
        sut.Last.Should().Be(0);
    }

    [Test]
    public void Remove_Should_RemoveItems_When_IteratingAndRemovingItemsInALoop()
    {
        var sut = new ResilientLinkedList<int>();
        var numberOfElements = 10;
        Enumerable.Range(1, numberOfElements).ForEach(sut.Add);

        foreach (var number in sut)
        {
            if(number == 5)
            {
                sut.Remove(6).Should().BeTrue();
                sut.Remove(10).Should().BeTrue();
                sut.Remove(1).Should().BeTrue();
                sut.Remove(2).Should().BeTrue();
            }
        }

        sut.Count.Should().Be(6);
        sut.First.Should().Be(3);
        sut.Last.Should().Be(9);
    }
}
