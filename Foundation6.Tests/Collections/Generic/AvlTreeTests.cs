using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class AvlTreeTests
{
    [Test]
    public void Clear_Should_RemoveAllElements_When_Iterating()
    {
        var sut = new AvlTree<int>();
        sut.Insert(10);
        sut.Insert(3);
        sut.Insert(5);
        sut.Insert(7);
        sut.Insert(1);


        sut.Clear();

        var elements = sut.ToArray();
        elements.Length.Should().Be(0);
    }

    [Test]
    public void Contains_Should_ReturnFalse_When_NotContainingElement()
    {
        var sut = new AvlTree<int>();
        sut.Insert(10);
        sut.Insert(3);
        sut.Insert(5);
        sut.Insert(7);
        sut.Insert(1);

        foreach (var element in new[] { 2, 4, 6, 9, 11 })
            sut.Contains(element).Should().BeFalse();
    }

    [Test]
    public void Contains_Should_ReturnTrue_When_ContainsElement()
    {
        var sut = new AvlTree<int>();
        sut.Insert(10);
        sut.Insert(3);
        sut.Insert(5);
        sut.Insert(7);
        sut.Insert(1);

        foreach (var element in sut)
            sut.Contains(element).Should().BeTrue();
    }

    [Test]
    public void Ctor_Should_NotBeNull_When_Created()
    {
        var sut = new AvlTree<int>();

        sut.Should().NotBeNull();
    }

    [Test]
    public void Insert_Should_Have1Element_When_Added1Element()
    {
        var sut = new AvlTree<int>();
        sut.Insert(5);

        var elements = sut.ToArray();
        elements.Length.Should().Be(1);
        elements[0].Should().Be(5);
    }

    [Test]
    public void Insert_Should_Have5Elements_When_Added5Elements()
    {
        var sut = new AvlTree<int>();
        sut.Insert(10);
        sut.Insert(3);
        sut.Insert(5);
        sut.Insert(7);
        sut.Insert(1);

        var elements = sut.ToArray();
        elements.Length.Should().Be(5);
        elements.Should().ContainInOrder([1, 3, 5, 7, 10]);
    }

    [Test]
    public void Remove_Should_Have4Elements_When_Removed1Element()
    {
        var sut = new AvlTree<int>();
        sut.Insert(10);
        sut.Insert(3);
        sut.Insert(5);
        sut.Insert(7);
        sut.Insert(1);

        sut.Remove(7);

        var elements = sut.ToArray();
        elements.Length.Should().Be(4);
        elements.Should().ContainInOrder([1, 3, 5, 10]);
    }

    [Test]
    public void Remove_Should_RemoveAllElements_When_Iterating()
    {
        var sut = new AvlTree<int>();
        sut.Insert(10);
        sut.Insert(3);
        sut.Insert(5);
        sut.Insert(7);
        sut.Insert(1);


        sut.Remove(5);
        sut.Remove(7);
        sut.Remove(10);
        sut.Remove(3);
        sut.Remove(1);

        var elements = sut.ToArray();
        elements.Length.Should().Be(0);
    }

    [Test]
    public void Remove_Should_RemoveElements_3_5_7_When_Iterating()
    {
        var sut = new AvlTree<int>();
        sut.Insert(10);
        sut.Insert(3);
        sut.Insert(5);
        sut.Insert(7);
        sut.Insert(1);

        foreach (var element in sut)
        {
            if (element == 1)
            {
                sut.Remove(5);
                sut.Remove(7);
                sut.Remove(3);
            }
        }

        var elements = sut.ToArray();
        elements.Length.Should().Be(2);
        elements.Should().ContainInOrder([1, 10]);
    }

    [Test]
    public void Remove_Should_RemoveElements_1_5_10_When_Iterating()
    {
        var sut = new AvlTree<int>();
        sut.Insert(10);
        sut.Insert(3);
        sut.Insert(5);
        sut.Insert(7);
        sut.Insert(1);

        foreach (var element in sut)
        {
            if(element == 5)
            {
                sut.Remove(5);
                sut.Remove(1);
                sut.Remove(9);
                sut.Remove(10);
            }
        }

        var elements = sut.ToArray();
        elements.Length.Should().Be(2);
        elements.Should().ContainInOrder([3, 7]);
    }
}
