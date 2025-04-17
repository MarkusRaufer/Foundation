using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class EnumerableConditionalsTests
{
    [DebuggerDisplay("Name:{Name}")]
    private record A(Guid Id, string Name);

    private record B(string Name) : A(Guid.NewGuid(), Name);


    private record C(string Name, string? NickName) : A(Guid.NewGuid(), Name);

    [Test]
    public void AllIfAny_Should_Return_False_When_SequenceIsEmpty()
    {
        var emptySequence = Enumerable.Empty<int>();

        var all = emptySequence.All(x => x % 2 == 0);
        all.Should().BeTrue();

        all = emptySequence.AllIfAny(x => x % 2 == 0);
        all.Should().BeFalse();
    }

    [Test]
    public void AllIfAny_Should_Return_True_When_AllElementsMatchThePredicate()
    {
        int[] emptySequence = [2, 4, 6, 8, 10];

        var all = emptySequence.All(x => x % 2 == 0);
        all.Should().BeTrue();

        all = emptySequence.AllIfAny(x => x % 2 == 0);
        all.Should().BeTrue();
    }

    [Test]
    public void ContainInOrder_Should_ReturnFalse_When_Collection_ContainsElementsInOrder()
    {
        int[] numbers = [2, 3, 4, 5, 10, 1, 6, 7, 8, 9];
        int[] expected = [5, 10, 6];

        var result = numbers.ContainsSequence(expected);
        result.Should().BeFalse();
    }

    [Test]
    public void ContainInOrder_Should_ReturnTrue_When_Collection_ContainsElementsInOrder()
    {
        int[] numbers = [2, 3, 4, 5, 10, 1, 6, 7, 8, 9];
        int[] expected = [5, 10, 1];

        var result = numbers.ContainsSequence(expected);
        result.Should().BeTrue();
    }

    [Test]
    public void If_Should_ExecuteAction_When_Predicate_IsTrue()
    {
        {
            var items = Enumerable.Range(1, 6);
            var ifItems = new List<int>();

            var elseItems = items.If(item => item < 4, ifItems.Add)
                                 .Else()
                                 .ToArray();

            CollectionAssert.AreEqual(Enumerable.Range(1, 3), ifItems);
            CollectionAssert.AreEqual(Enumerable.Range(4, 3), elseItems);
        }
        {
            var items = Enumerable.Range(1, 6);
            var ifItems = new List<int>();

            var elseIfItems = new List<int>();
            var elseItems = items.If(item => item < 3, ifItems.Add)
                                 .ElseIf(item => item < 5, elseIfItems.Add)
                                 .Else()
                                 .ToArray();

            CollectionAssert.AreEqual(Enumerable.Range(1, 2), ifItems);
            CollectionAssert.AreEqual(Enumerable.Range(3, 2), elseIfItems);
            CollectionAssert.AreEqual(Enumerable.Range(5, 2), elseItems);
        }

        {
            var items = Enumerable.Range(1, 6);
            var ifItems = new List<int>();
            var elseIfItems = new List<int>();

            items.If(item => item < 3, ifItems.Add)
                 .ElseIf(item => item < 5, elseIfItems.Add)
                 .EndIf();

            CollectionAssert.AreEqual(Enumerable.Range(1, 2), ifItems);
            CollectionAssert.AreEqual(Enumerable.Range(3, 2), elseIfItems);
        }
    }

    [Test]
    public void If_Should_ReturnMappedValues_When_Predicate_IsTrue()
    {
        {
            var numbers = Enumerable.Range(1, 6).ToArray();

            var actual = numbers.If(n => n % 2 == 0, n => n * 10)
                                .Else(n => n)
                                .ToArray();

            var expected = new[] { 1, 20, 3, 40, 5, 60 };

            Assert.AreEqual(numbers.Length, actual.Length);
            Assert.AreEqual( 1, actual[0]);
            Assert.AreEqual(20, actual[1]);
            Assert.AreEqual( 3, actual[2]);
            Assert.AreEqual(40, actual[3]);
            Assert.AreEqual( 5, actual[4]);
            Assert.AreEqual(60, actual[5]);
        }

        {
            var numbers = Enumerable.Range(1, 6);

            var strings = numbers.If(n => 3 > n, n => n.ToString())
                                 .Else(n => $"{n * 10}").ToArray();

        }
    }

    [Test]
    public void OpenWhen_Should_Return0Elements_When_NumberOfElementsIsSmallerThan11()
    {
        var numbers = Enumerable.Range(1, 10);

        var actual = numbers.OpenWhen((n, i) => i == 11).ToArray();

        actual.Length.Should().Be(0);
    }

    [Test]
    public void OpenWhen_Should_Return10Elements_When_NumberOfElementsIsAtLeast10()
    {
        var numbers = Enumerable.Range(1, 10);

        var actual = numbers.OpenWhen((n, i) => i == 9).ToArray();

        actual.Length.Should().Be(10);
    }

    [Test]
    public void OpenWhen_Should_Return10Elements_When_NumberOfElementsIsAtLeast5()
    {
        var numbers = Enumerable.Range(1, 10);

        var actual = numbers.OpenWhen((n, i) => i == 5).ToArray();

        actual.Length.Should().Be(10);
    }

    [Test]
    public void IsInAscendingOrder_Should_ReturnFalse_When_ValuesAreNotAscending()
    {
        var numbers = new[] { 4, 3, 7, 9 };

        numbers.IsInAscendingOrder().Should().BeFalse();
    }

    [Test]
    public void IsInAscendingOrder_Should_ReturnFalse_When_AllowEqualFalseAndItemsIncludeEqualValues()
    {
        var items = new int[] { 1, 2, 2, 3, 4 };

        items.IsInAscendingOrder().Should().BeFalse();
    }

    [Test]
    public void IsInAscendingOrder_Should_ReturnTrue_When_AllowEqualFalseAndAllItemsAreInAscendingOrder()
    {
        var items = Enumerable.Range(0, 5);

        items.IsInAscendingOrder().Should().BeTrue();
    }

    [Test]
    public void IsInAscendingOrder_Should_ReturnTrue_When_AllowEqualTrueAndItemsIncludeEqualValues()
    {
        var items = new int[] { 1, 2, 2, 3, 4 };

        items.IsInAscendingOrder(allowEqual: true).Should().BeTrue();
    }

    [Test]
    public void IsInAscendingOrder_ShouldReturnFalse_When_ValuesAreNotAscending_And_CompareIsUsed()
    {
        var numbers = new[] { 4, 3, 7, 9 };

        numbers.IsInAscendingOrder((a, b) =>
        {
            if (a < b) return CompareResult.Smaller;
            if (a > b) return CompareResult.Greater;
            return CompareResult.Equal;
        }).Should().BeFalse();
    }

    [Test]
    public void IsInAscendingOrder_ShouldReturnTrue_When_ValuesAreAscending_And_CompareIsUsed()
    {
        {
            var numbers = Enumerable.Range(1, 5);

            numbers.IsInAscendingOrder((a, b) =>
            {
                if (a < b) return CompareResult.Smaller;
                if (a > b) return CompareResult.Greater;
                return CompareResult.Equal;
            }).Should().BeTrue();
        }
        {
            var numbers = new[] { 3, 4, 4, 7, 9 };

            numbers.IsInAscendingOrder((a, b) =>
            {
                if (a < b) return CompareResult.Smaller;
                if (a > b) return CompareResult.Greater;
                return CompareResult.Equal;
            }).Should().BeTrue();
        }
    }

    [Test]
    public void ModifyIf_Should_ReturnModifiedItems_When_ModifyIsTrue()
    {
        var s1 = "1";
        var s2 = "2";
        var s3 = "3";
        var s4 = "4";

        var items = Enumerable.Empty<string>()
                        .Append(s1)
                        .Append(s2)
                        .ModifyIf(() => s3 is not null, elems => elems.Append(s3!))
                        .Append(s4)
                        .ToArray();

        items.Should().NotBeNull();
        items.Length.Should().Be(4);

        items[0].Should().Be(s1);
        items[1].Should().Be(s2);
        items[2].Should().Be(s3);
        items[3].Should().Be(s4);
    }

    [Test]
    public void ModifyIf_Should_ReturnUnmodifiedItems_When_ModifyIsFalse()
    {
        var s1 = "1";
        var s2 = "2";
        string? s3 = null;
        var s4 = "4";

        var items = Enumerable.Empty<string>()
                        .Append(s1)
                        .Append(s2)
                        .ModifyIf(() => s3 is not null, elems => elems.Append(s3!))
                        .Append(s4)
                        .ToArray();

        items.Should().NotBeNull();
        items.Length.Should().Be(3);

        items[0].Should().Be(s1);
        items[1].Should().Be(s2);
        items[2].Should().Be(s4);
    }

    [Test]
    public void ThrowIfNumberNotExact_Should_ReturnNumberOfElements_When_ExactNumberOfElements_Exist()
    {
        var numberOfElems = 10;
        var numbers = Enumerable.Range(1, numberOfElems);

        var exactNumberOfElems = numbers.ThrowIfNumberNotExact(numberOfElems).ToArray();
        Assert.AreEqual(numberOfElems, exactNumberOfElems.Length);
    }

    [Test]
    public void ThrowIfNumberNotExact_Should_ThrowException_When_NumberOfElements_IsDifferent()
    {
        var numberOfElems = 10;
        var numbers = Enumerable.Range(1, numberOfElems).ToArray();

        Assert.Throws<ArgumentException>(() => numbers.ThrowIfNumberNotExact(numberOfElems - 1).ToArray());
        Assert.Throws<ArgumentException>(() => numbers.ThrowIfNumberNotExact(numberOfElems + 1).ToArray());
    }
}

// ReSharper restore InconsistentNaming
