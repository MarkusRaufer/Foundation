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
    public class A
    {
        private Guid _id;

        public A(string name)
        {
            Name = name;
        }

        public Guid Id
        {
            get
            {
                if (Guid.Empty == _id)
                    _id = Guid.NewGuid();

                return _id;
            }
            set { _id = value; }
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class B : A
    {
        public B(string name) : base(name)
        {
        }
    }


    public class C : A
    {
        public C(string name) : base(name)
        {
        }

        public string? NickName { get; set; }
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
            var numbers = Enumerable.Range(1, 6);

            var actual = numbers.If(n => n % 2 == 0, n => n * 10)
                                .Else(n => n).ToArray();

            var expected = new[] { 1, 20, 3, 40, 5, 60 };

            Assert.AreEqual(expected.Length, actual.Length);
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
            Assert.AreEqual(expected[2], actual[2]);
            Assert.AreEqual(expected[3], actual[3]);
            Assert.AreEqual(expected[4], actual[4]);
            Assert.AreEqual(expected[5], actual[5]);
        }

        {
            var numbers = Enumerable.Range(1, 6);

            var strings = numbers.If(n => 3 > n, n => n.ToString())
                                 .Else(n => $"{n * 10}").ToArray();

        }
    }

    [Test]
    public void IfMoreOrEqualThan_Should_Return0Elements_When_NumberOfElementsAreSmallerThan11()
    {
        var numbers = Enumerable.Range(1, 10);

        var actual = numbers.IfMoreOrEqualThan(11).ToArray();

        actual.Length.Should().Be(0);
    }

    [Test]
    public void IfMoreOrEqualThan_Should_Return10Elements_When_NumberOfElementsIs10()
    {
        var numbers = Enumerable.Range(1, 10);

        var actual = numbers.IfMoreOrEqualThan(10).ToArray();

        actual.Length.Should().Be(10);
    }

    [Test]
    public void IfMoreOrEqualThan_Should_Return10Elements_When_NumberOfElementsIs5()
    {
        var numbers = Enumerable.Range(1, 10);

        var actual = numbers.IfMoreOrEqualThan(5).ToArray();

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
