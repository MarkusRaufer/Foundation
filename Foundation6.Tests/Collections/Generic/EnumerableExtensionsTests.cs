﻿using Foundation.TestUtil.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Foundation.Collections.Generic;

[TestFixture]
public class EnumerableExtensionsTests
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

    private record Person(string Name, int Age);

    // ReSharper disable InconsistentNaming

    [Test]
    public void AfterEach()
    {
        List<string> items = ["1", "2", "3"];
        var sb = new StringBuilder();

        foreach (var item in items.AfterEach(() => sb.Append(',')))
        {
            sb.Append(item);
        }

        var actual = sb.ToString();
        Assert.AreEqual("1,2,3", actual);
    }

    [Test]
    public void Aggregate_Should_ReturnSome_When_HasElements()
    {
        var numbers = Enumerable.Range(1, 3);

        var minmax = numbers.AggregateAsOption(number => (min: number, max: number), (acc, number) =>
        {
            if (number < acc.min) acc.min = number;
            if (number > acc.max) acc.max = number;
            return (acc.min, acc.max);
        });

        Assert.IsTrue(minmax.IsSome);

        var (min, max) = minmax.OrThrow();
        Assert.AreEqual(1, min);
        Assert.AreEqual(3, max);

    }

    [Test]
    public void AverageMedian_ShouldReturnMedian_WhenUsingConverter()
    {
        //odd number of elements
        {
            IEnumerable<string> items = new List<string> { "a", "ab", "abc" };

            var median = items.AverageMedian(x => x.Length);
            Assert.AreEqual(2M, median);
        }
        //even number of elements
        {
            IEnumerable<string> items = new List<string> { "a", "ab", "abc", "abcd" };

            var median = items.AverageMedian(x => x.Length);
            Assert.AreEqual(2.5M, median);
        }
    }

    [Test]
    public void AverageMedian_ShouldReturnMedian_WhenUsingNumbers()
    {
        //odd number of elements
        {
            var numbers = Enumerable.Range(1, 7);

            var median = numbers.AverageMedian();
            Assert.AreEqual(4, median);
        }
        //even number of elements
        {
            var numbers = Enumerable.Range(1, 8);

            var median = numbers.AverageMedian();
            Assert.AreEqual(4.5, median);
        }
    }

    [Test]
    public void AverageMedian_ShouldThrowException_WhenUsingValuesNotConvertibleToDecimal()
    {
        IEnumerable<string> items = new List<string> { "one", "two", "three" };

        Assert.Throws<FormatException>(() => items.AverageMedian());
    }

    [Test]
    public void AverageMedianValues_ShouldReturnTheMedianPositioned()
    {
        {
            var numbers = Enumerable.Range(1, 7);

            var (opt1, opt2) = numbers.AverageMedianValues();
            Assert.IsFalse(opt2.IsSome);
            Assert.AreEqual(4, opt1.OrThrow());
        }
        {
            var numbers = Enumerable.Range(1, 8);
            var (opt1, opt2) = numbers.AverageMedianValues();

            Assert.IsTrue(opt2.IsSome);
            Assert.AreEqual(4, opt1.OrThrow());
            Assert.AreEqual(5, opt2.OrThrow());
        }
        {
            var items = Enumerable.Range(1, 7).Select(x => x.ToString());

            var (opt1, opt2) = items.AverageMedianValues();
            Assert.IsFalse(opt2.IsSome);
            Assert.AreEqual("4", opt1.OrThrow());
        }
        {
            var items = Enumerable.Range(1, 8).Select(x => x.ToString());

            var (opt1, opt2) = items.AverageMedianValues();
            Assert.IsTrue(opt2.IsSome);
            Assert.AreEqual("4", opt1.OrThrow());
            Assert.AreEqual("5", opt2.OrThrow());
        }
    }

    [Test]
    public void CartesianProduct()
    {
        List<string> items1 = ["1", "2", "3"];
        List<string> items2 = ["a", "b", "c"];

        var erg = items1.CartesianProduct(items2, (l, r) => (l, r)).ToArray();
        erg[0].ShouldBe(("1", "a"));
        erg[1].ShouldBe(("1", "b"));
        erg[2].ShouldBe(("1", "c"));
        erg[3].ShouldBe(("2", "a"));
        erg[4].ShouldBe(("2", "b"));
        erg[5].ShouldBe(("2", "c"));
        erg[6].ShouldBe(("3", "a"));
        erg[7].ShouldBe(("3", "b"));
        erg[8].ShouldBe(("3", "c"));
    }
    
    [Test]
    public void Contains_AllNumbersWithinRange()
    {
        var items1 = Enumerable.Range(1, 10);
        var items2 = Enumerable.Range(4,  9);

        items1.Contains(items2).ShouldBeTrue();

        var items3 = new List<int> { 11, 12 };
        items1.Contains(items3).ShouldBeFalse();
    }

    [Test]
    public void Contains_IncludingNumbersOutOfRange()
    {
        var items1 = Enumerable.Range(0, 9);
        IEnumerable<int> items2 = new List<int> { 1, 5, 12 };

        items1.Contains(items2).ShouldBeTrue();
    }

    [Test]
    public void Correlate_Should_ReturnTwoElements_When_AllListsContainTwoSameElements()
    {
        var lhs = new DateOnly[]
        {
            new(2023, 1, 1),
            new(2023, 2, 2),
            new(2023, 3, 3),
            new(2023, 4, 4),
        };

        var rhs = new DateOnly[]
        {
            new(2023, 5, 3),
            new(2023, 6, 4),
            new(2023, 7, 5),
            new(2023, 8, 6),
        };

        var intersected = lhs.Correlate(rhs, x => x.Day).ToArray();

        intersected.Length.ShouldBe(4);
        intersected[0].ShouldBe(new DateOnly(2023, 3, 3));
        intersected[1].ShouldBe(new DateOnly(2023, 5, 3));
        intersected[2].ShouldBe(new DateOnly(2023, 4, 4));
        intersected[3].ShouldBe(new DateOnly(2023, 6, 4));
    }

    [Test]
    public void Cycle_ShouldReturn7CycledElements_When_Take7()
    {
        List<string> items = ["A", "B", "C"];

        var elements = items.Cycle().Take(7).ToArray();

        string[] expected = ["A", "B", "C", "A", "B", "C", "A"];

        elements.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public void Enumerate_Should_Return5TuplesWithIncrementedCounter_When_Using_Default()
    {
        List<string> items = ["A", "B", "C", "D", "E"];

        var enumerated = items.Enumerate().ToArray();
        enumerated[0].ShouldBe((0, "A"));
        enumerated[1].ShouldBe((1, "B"));
        enumerated[2].ShouldBe((2, "C"));
        enumerated[3].ShouldBe((3, "D"));
        enumerated[4].ShouldBe((4, "E"));
    }

    [Test]
    public void Enumerate_Should_Return5TuplesWithIncrementedCounter_When_Using_Min()
    {
        List<string> items = ["A", "B", "C", "D", "E"];

        var enumerated = items.Enumerate(2).ToArray();
        enumerated[0].ShouldBe((2, "A"));
        enumerated[1].ShouldBe((3, "B"));
        enumerated[2].ShouldBe((4, "C"));
        enumerated[3].ShouldBe((5, "D"));
        enumerated[4].ShouldBe((6, "E"));

    }
    [Test]
    public void Enumerate_Should_Return5TuplesWithIncrementedCounter_When_Used_MinMaxAndIncrement()
    {
        List<string> items = ["A", "B", "C", "D", "E"];

        var enumerated = items.EnumerateRange(1, 2).ToArray();
        enumerated[0].ShouldBe((1, "A"));
        enumerated[1].ShouldBe((2, "B"));
        enumerated[2].ShouldBe((1, "C"));
        enumerated[3].ShouldBe((2, "D"));
        enumerated[4].ShouldBe((1, "E"));
    }

    [Test]
    public void Enumerate_Should_Return5TuplesWithIncrementedCounter_When_Used_MinMaxAndNextCounterValueDecrementingCounterValue()
    {
        List<string> items = ["A", "B", "C", "D", "E"];

        var enumerated = items.Enumerate(1, -1, x => --x).ToArray();
        enumerated[0].ShouldBe(( 1, "A"));
        enumerated[1].ShouldBe(( 0, "B"));
        enumerated[2].ShouldBe((-1, "C"));
        enumerated[3].ShouldBe(( 1, "D"));
        enumerated[4].ShouldBe(( 0, "E"));
    }

    [Test]
    public void Enumerate_Should_Return5TuplesWithIncrementedCounter_When_Used_MinMaxAndNextCounterValueIncrementingCounterValue()
    {
        List<string> items = ["A", "B", "C", "D", "E"];

        var enumerated = items.Enumerate(2, 6, x => x + 2).ToArray();
        enumerated[0].ShouldBe((2, "A"));
        enumerated[1].ShouldBe((4, "B"));
        enumerated[2].ShouldBe((6, "C"));
        enumerated[3].ShouldBe((2, "D"));
        enumerated[4].ShouldBe((4, "E"));
    }

    [Test]
    public void SymmetricDifference_Should_ReturnAllItems_When_ListsAreCompletelyDifferent()
    {
        var items1 = Enumerable.Range(0, 10);
        var items2 = Enumerable.Range(10, 10);

        // return all items because lists are completely different
        var diff = items1.SymmetricDifference(items2).ToArray();

        diff.Length.ShouldBe(20);

        Array.Sort(diff);
        diff.ShouldBeEquivalentTo(Enumerable.Range(0, 20).ToArray());
    }

    [Test]
    public void SymmetricDifference_Should_ReturnNoItem_When_ListsHaveTheSameItems()
    {
        var items1 = Enumerable.Range(0, 10);
        var items2 = Enumerable.Range(0, 10);

        var diff = items1.SymmetricDifference(items2).ToArray();

        diff.Length.ShouldBe(0);
    }

    [Test]
    public void SymmetricDifference_Should_ReturnDifferentItemsFromBothLists_When_BothListsHaveDifferentItems()
    {
        List<int> items1 = [1, 2, 3, 4, 5];
        List<int> items2 = [2, 4, 6];

        // return items of both lists that don't match
        var diff = items1.SymmetricDifference(items2).ToArray();

        diff.Length.ShouldBe(4);

        Array.Sort(diff);

        diff.ShouldBeEquivalentTo(new[] { 1, 3, 5, 6 });
    }

    [Test]
    public void SymmetricDifference_Should_Return3Items_When_ConsiderDuplicatesFalse()
    {
        int[] items1 = [1, 1, 1, 1];
        int[] items2 = [1, 1, 2, 2, 3];

        var diff = items1.SymmetricDifference(items2, preserveDuplicates: false).ToArray();

        diff.Length.ShouldBe(2);
        diff.ShouldBeEquivalentTo(new[] { 2, 3 });
    }

    [Test]
    public void SymmetricDifference_Should_Return5Items_When_ConsiderDuplicatesTrue()
    {
        int[] items1 = [1, 1, 1, 1];
        int[] items2 = [1, 1, 2, 2, 3];

        var diff = items1.SymmetricDifference(items2, preserveDuplicates: true).ToArray();

        diff.Length.ShouldBe(5);
        diff.ShouldBeEquivalentTo(new[] { 1, 1, 2, 2, 3 });
    }


    [Test]
    public void SymmetricDifference_Should_Return3DateTimes1Doublet_When_Using_Selector_And_RetainDuplicates()
    {
        DateTime date(int day) => new (2020, 5, day);

        DateTime[] dates1 = [date(1), date(2), date(1), date(3), date(4)];
        DateTime[] dates2 = [date(1), date(2), date(3), date(5)];

        var result = dates1.SymmetricDifference(dates2, x => x.Day, true).ToArray();

        result.Length.ShouldBe(3);
        result[0].ShouldBe(date(1));
        result[1].ShouldBe(date(4));
        result[2].ShouldBe(date(5));
    }

    [Test]
    public void SymmetricDifferenceWithSideIndication_Should_ReturnDifferentItemsFromBothLists_When_UsingPreserveDuplicatesFalse()
    {
        List<int> items1 = [1, 2, 3, 4, 5];
        List<int> items2 = [2, 4, 6];

        // return items of both lists that don't match
        var (lhs, rhs) = items1.SymmetricDifferenceWithSideIndication(items2);

        var lhsArray = lhs.ToArray();
        var rhsArray = rhs.ToArray();

        lhsArray.Length.ShouldBe(3);
        lhsArray.ShouldBe([1, 3, 5]);

        rhsArray.Length.ShouldBe(1);
        rhsArray.ShouldBe([6]);
    }

    [Test]
    public void SymmetricDifferenceWithSideIndication_Should_ReturnDifferentItemsFromBothLists_When_UsingSelector()
    {

        DateTime date(int day) => new (2020, 5, day);

        DateTime[] dates1 = [date(1), date(2), date(1), date(3), date(4), date(6)];
        DateTime[] dates2 = [date(1), date(2), date(3), date(5), date(7)];

        var (lhs, rhs) = dates1.SymmetricDifferenceWithSideIndication(dates2, x => x.Day);

        var lhsArray = lhs.ToArray();
        var rhsArray = rhs.ToArray();

        lhsArray.Length.ShouldBe(2);
        lhsArray.ShouldBe([date(4), date(6)]);

        rhsArray.Length.ShouldBe(2);
        rhsArray.ShouldBe([date(5), date(7)]);
    }

    [Test]
    public void SymmetricDifferenceWithSideIndication_Should_Return3DateTimes1Doublet_When_Using_Selector_And_PreserveDuplicates()
    {
        static DateTime date(int day) => new (2020, 5, day);

        DateTime[] dates1 = [date(1), date(2), date(1), date(3), date(4)];
        DateTime[] dates2 = [date(2), date(3), date(5), date(6)];

        var (lhs, rhs) = dates1.SymmetricDifferenceWithSideIndication(dates2, x => x.Day, true);

        var lhsArray = lhs.ToArray();
        var rhsArray = rhs.ToArray();

        lhsArray.Length.ShouldBe(3);
        lhsArray.ShouldBe([date(1), date(1), date(4)]);

        rhsArray.Length.ShouldBe(2);
        rhsArray.ShouldBe([date(5), date(6)]);
    }


    [Test]
    public void Duplicates_DistinctIsFalse_WithMultipleDuplicateValues()
    {
        int[] items = [1, 2, 3, 4, 5, 2, 4, 2];

        var result = items.Duplicates().ToArray();

        result.Length.ShouldBe(3);
        result[0].ShouldBe(2);
        result[1].ShouldBe(4);
        result[2].ShouldBe(2);
    }

    [Test]
    public void Duplicates_ShouldReturnDouplets_When_HasDuplicates()
    {
        int[] items = [1, 2, 3, 4, 5, 2, 4, 2];

        var result = items.Duplicates().ToArray();

        result.Length.ShouldBe(3);
        result[0].ShouldBe(2);
        result[1].ShouldBe(4);
        result[2].ShouldBe(2);
    }

    [Test]
    public void Duplicates_ShouldReturnDouplets_When_HasDuplicates2()
    {
        var items = new List<(int, string)> 
        { 
            (1, "a"),
            (2, "b"),
            (3, "c"),
            (4, "d"),
            (5, "e"), 
            (2, "f"),
            (4, "g"),
            (2, "h")
        };

        var result = items.Duplicates(x => x.Item1).ToArray();

        result.Length.ShouldBe(3);

        result[0].Item1.ShouldBe(2);
        result[0].Item2.ShouldBe("f");

        result[1].Item1.ShouldBe(4);
        result[1].Item2.ShouldBe("g");

        result[2].Item1.ShouldBe(2);
        result[2].Item2.ShouldBe("h");
    }

    [Test]
    public void Duplicates_DistinctIsFalse_WithoutDuplicateValues()
    {
        int[] items = [1, 2, 3, 4, 5];

        var result = items.Duplicates().ToArray();

        result.Length.ShouldBe(0);
    }

    [Test]
    public void Enumerate_ShouldReturn9Elements_When_CalledWithCreateValue()
    {
        var items1 = Enumerable.Range(0, 9).ToArray();

        var enumerated = items1.Enumerate(n => n * 2).ToArray();

        items1.Length.ShouldBe(enumerated.Length);

        foreach (var (counter, item) in enumerated)
            counter.ShouldBe(item * 2);
    }

    [Test]
    public void Enumerate_ShouldReturn3Tuples_When_CalledWithoutParamenter()
    {
        var items = new[] { "one", "two", "three" };

        var enumerated = items.Enumerate().ToArray();

        enumerated[0].ShouldBe((0, "one"));
        enumerated[1].ShouldBe((1, "two"));
        enumerated[2].ShouldBe((2, "three"));
    }

    [Test]
    public void Enumerate_ShouldReturn3Tuples_When_CalledWithParamenter()
    {
        var items = new[] { "one", "two", "three" };
        var i = 10;

        var enumerated = items.Enumerate(item => i++).ToArray();

        enumerated[0].ShouldBe((10, "one"));
        enumerated[1].ShouldBe((11, "two"));
        enumerated[2].ShouldBe((12, "three"));
    }

    [Test]
    public void Enumerate_ShouldReturn10Tuples_When_UsingMinMax_On10Items()
    {
        var items = Enumerable.Range(1, 10).Select(x => x.ToString());

        var enumerated = items.EnumerateRange(min: -1, max: 1).ToArray();

        enumerated[0].ShouldBe((-1, "1"));
        enumerated[1].ShouldBe(( 0, "2"));
        enumerated[2].ShouldBe(( 1, "3"));
        enumerated[3].ShouldBe((-1, "4"));
        enumerated[4].ShouldBe(( 0, "5"));
        enumerated[5].ShouldBe(( 1, "6"));
        enumerated[6].ShouldBe((-1, "7"));
        enumerated[7].ShouldBe(( 0, "8"));
        enumerated[8].ShouldBe(( 1, "9"));
        enumerated[9].ShouldBe((-1, "10"));
    }

    [Test]
    public void Enumerate_ShouldReturn10Tuples_When_UsingRange_On10Items()
    {
        var items = Enumerable.Range(1, 10).Select(x => x.ToString());

        var x = items.EnumerateRange(10..12).ToArray();

        var enumerated = items.EnumerateRange(10..12).ToArray();

        enumerated[0].ShouldBe((10, "1"));
        enumerated[1].ShouldBe((11, "2"));
        enumerated[2].ShouldBe((12, "3"));
        enumerated[3].ShouldBe((10, "4"));
        enumerated[4].ShouldBe((11, "5"));
        enumerated[5].ShouldBe((12, "6"));
        enumerated[6].ShouldBe((10, "7"));
        enumerated[7].ShouldBe((11, "8"));
        enumerated[8].ShouldBe((12, "9"));
        enumerated[9].ShouldBe((10, "10"));
    }

    [Test]
    public void Enumerate_ShouldReturn3Tuples_When_UsingWithSeed_On3Items()
    {
        var items = new[] { "1", "2", "3" };

        var enumerated = items.Enumerate(5).ToArray();

        enumerated[0].ShouldBe((5, "1"));
        enumerated[1].ShouldBe((6, "2"));
        enumerated[2].ShouldBe((7, "3"));
    }

    [Test]
    public void EqualsCollection_Should_ReturnFalse_When_Items_SameNumberOfElementsAndDifferentOccrencies()
    {
        var items1 = new[] { 1, 2, 3, 2, 1 };
        var items2 = new[] { 2, 3, 2, 1, 2 };

        items1.EqualsCollection(items2).ShouldBeFalse();
        items2.EqualsCollection(items1).ShouldBeFalse();
    }

    [Test]
    public void EqualsCollection_Should_ReturnTrue_When_SameNumberOfElementsAndSameOrder()
    {
        var items1 = Enumerable.Range(0, 5);
        var items2 = Enumerable.Range(0, 5);

        items1.EqualsCollection(items2).ShouldBeTrue();
        items2.EqualsCollection(items1).ShouldBeTrue();
    }

    [Test]
    public void EqualsCollection_Should_ReturnTrue_When_Items_SameNumberOfElementsAndDifferentOrder()
    {
        var items1 = new[] { 1, 2, 3, 2 };
        var items2 = new[] { 2, 3, 2, 1 };

        items1.EqualsCollection(items2).ShouldBeTrue();
        items2.EqualsCollection(items1).ShouldBeTrue();
    }

    [Test]
    public void EqualsCollection_Should_ReturnTrue_When_Items_SameNumberOfElementsAndSameOrder()
    {
        var items1 = Enumerable.Range(0, 5);
        var items2 = Enumerable.Range(0, 5);

        items1.EqualsCollection(items2).ShouldBeTrue();
        items2.EqualsCollection(items1).ShouldBeTrue();
    }

    [Test]
    public void EqualsCollection_Should_ReturnFalse_When_DifferentNumberOfElements()
    {
        var items1 = Enumerable.Range(0, 5);
        var items2 = Enumerable.Range(0, 6);

        items1.EqualsCollection(items2).ShouldBeFalse();
        items2.EqualsCollection(items1).ShouldBeFalse();
    }

    [Test]
    public void ExceptBy()
    
    {
        var items1 = new[] 
        { 
            new A("1"),
            new A("2"),
            new A("3"),
        };

        var items2 = new[]
        {
            new C("1") { NickName = "3" },
            new C("2") { NickName = "1" },
            new C("3") { NickName = "1" },
        };

        var different = items1.ExceptBy(items2, i1 => i1.Name, i2 => i2.NickName, i1 => i1).ToArray();

        different.Length.ShouldBe(1);
        different[0].Name.ShouldBe("2");
    }

    [Test]
    public void ExceptWithDuplicates_Should_Return4Doublets_When_LhsHasDuplicates()
    {
        var items1 = new[] { 1, 1, 1, 2, 3, 2, 1 };
        var items2 = new[] { 1, 2, 3, 4 };

        var result = items1.ExceptWithDuplicates(items2).OrderBy(x => x).ToArray();

        var expected = new[] { 1, 1, 1, 2 };
        expected.SequenceEqual(result).ShouldBeTrue();
    }

    [Test]
    public void ExceptWithDuplicates_Should_ReturnDoublets_When_LhsHasDuplicates()
    {
        var items1 = new[] { 1, 1, 1, 2, 3, 2, 1 };
        var items2 = new[] { 1, 2, 3, 1, 4 };

        var result = items1.ExceptWithDuplicates(items2).OrderBy(x => x).ToArray();

        var expected = new[] { 1, 1, 2 };
        expected.SequenceEqual(result).ShouldBeTrue();
    }

    [Test]
    public void ExceptWithDuplicates_Should_NoDoublets_When_LhsHasNoDuplicates()
    {
        var items1 = new[] { 1, 2, 3, 4 };
        var items2 = new[] { 2, 4 };

        var result = items1.ExceptWithDuplicates(items2).OrderBy(x => x).ToArray();

        var expected = new[] { 1, 3 };
        expected.SequenceEqual(result).ShouldBeTrue();
    }

    [Test]
    public void ExceptWithDuplicates_Should_Return2DateTimes1Doublet_When_Using_Selector_LhsHasDuplicates()
    {
        DateTime date(int day) => new DateTime(2020, 5, day);

        var dates1 = new DateTime[] { date(1), date(2), date(1), date(3), date(4) };
        var dates2 = new DateTime[] { date(1), date(2), date(3) };

        var result = dates1.ExceptWithDuplicates(dates2, x => x.Day).ToArray();

        result.Length.ShouldBe(2);
        result[0].ShouldBe(date(1));
        result[1].ShouldBe(date(4));
    }

    [Test]
    public void ExceptWithDuplicates_Should_Return2Strings1Doublet_When_Using_Selector_LhsHasDuplicates()
    {
        DateTime date(int day) => new (2020, 5, day);

        var dates1 = new string?[] 
        { 
            date(1).ToString(),
            date(2).ToString(),
            default,
            date(1).ToString(),
            date(3).ToString(),
            date(4).ToString()
        };

        var dates2 = new string?[] 
        {
            date(1).ToString(),
            date(2).ToString(),
            date(3).ToString()
        };

        var result = dates1.ExceptWithDuplicates(dates2, x => null == x ? 0 : DateTime.Parse(x).Day).ToArray();

        result.Length.ShouldBe(3);
        result[0].ShouldBeNull();
        result[1].ShouldBe(date(1).ToString());
        result[2].ShouldBe(date(4).ToString());
    }

    [Test]
    public void ExceptWithDuplicatesSorted_Should_ReturnDoublets_When_LhsHasDuplicates()
    {
        var items1 = new[] { 1, 1, 1, 2, 3, 2, 1 };
        var items2 = new[] { 1, 2, 3, 1, 4 };

        var result = items1.ExceptWithDuplicatesSorted(items2).ToArray();

        var expected = new[] { 1, 1, 2 };
        expected.SequenceEqual(result).ShouldBeTrue();
    }

    [Test]
    public void ExceptWithDuplicatesSorted_Should_NoDoublets_When_LhsHasNoDuplicates()
    {
        var items1 = new[] { 1, 2, 3, 4 };
        var items2 = new[] { 2, 4 };

        var result = items1.ExceptWithDuplicatesSorted(items2).OrderBy(x => x).ToArray();

        var expected = new[] { 1, 3 };
        expected.SequenceEqual(result).ShouldBeTrue();
    }

    [Test]
    public void ExceptWithDuplicatesSorted_Should_Return4Doublets_When_LhsHasDuplicates()
    {
        var items1 = new [] { 1, 1, 1, 2, 3, 2, 1 };
        var items2 = new [] { 1, 2, 3, 4 };
        
        var result = items1.ExceptWithDuplicatesSorted(items2).OrderBy(x => x).ToArray();

        var expected = new[] { 1, 1, 1, 2 };
        expected.SequenceEqual(result).ShouldBeTrue();
    }

    [Test]
    public void FilterMap()
    {
        var numbers = Enumerable.Range(1, 10);

        var strings = numbers.FilterMap(x => x % 2 == 0, x => x.ToString());
        var expected = new[] { "2", "4", "6", "8", "10" };
        strings.ShouldBe(expected);
    }


    [Test]
    public void FirstOf_Should_ReturnFirstOccurenceOfString_When_Using_ResultTypeString()
    {
        var items = new object[] { 1, '2', "3", 4, '5', "6" };
        var first = items.FirstOf<string>();

        first.ShouldBe("3");
    }

    [Test]
    public void FirstOfType_Should_ReturnFirstOccurenceOfString_When_Using_ElementsTypeObjectAndResultTypeString()
    {
        var items = new object[] { 1, '2', "3", 4, '5', "6" };
        var first = items.FirstOfType<object, string>();

        first.ShouldBe("3");
    }

    [Test]
    public void ForEach_Returning_number_of_processed_acctions()
    {
        var items = Enumerable.Range(1, 5);
        var sum = 0;

        items.ForEach(x => sum += x);

        sum.ShouldBe(15);
    }

    [Test]
    public void ForEach_WithEmptyList()
    {
        var items = Enumerable.Empty<int>();
        var sum = 0;

        items.ForEach(action: x => sum += x, onEmpty: () => sum = 1);

        sum.ShouldBe(1);
    }

    [Test]
    public void HasAtLeast_Should_ReturnFalse_When_LessElementsInListAsNumberOfElements()
    {
        var numbers = new[] { 1, 2 };

        var result = numbers.HasAtLeast(3);

        result.ShouldBeFalse();
    }

    [Test]
    public void HasAtLeast_Should_ReturnTrue_When_NumberOfElementsInListIsEqual()
    {
        var numbers = new[] { 1, 2, 3 };

        var result = numbers.HasAtLeast(3);

        result.ShouldBeTrue();
    }

    [Test]
    public void HasAtLeast_Should_ReturnTrue_When_NumberOfElementsInListIsGreater()
    {
        var numbers = new[] { 1, 2, 3 };
 
        var result = numbers.HasAtLeast(2);

        result.ShouldBeTrue();
    }

    [Test]
    public void Ignore_Should_Ignore_2_Items_When_2_IndicesUsed()
    {
        var numbers = Enumerable.Range(1, 5);

        var filtered = numbers.Ignore(new[] { 2, 3 }).ToArray();

        filtered.Length.ShouldBe(3);
        filtered[0].ShouldBe(1);
        filtered[1].ShouldBe(2);
        filtered[2].ShouldBe(5);
    }

    [Test]
    public void Ignore_Should_Ignore_Items_When_Match_On_Indices()
    {
        var numbers = Enumerable.Range(0, 10);

        var filtered = numbers.Ignore(new[] { 1, 3, 5, 7, 9 }).ToArray();

        filtered.Length.ShouldBe(5);
        filtered[0].ShouldBe(0);
        filtered[1].ShouldBe(2);
        filtered[2].ShouldBe(4);
        filtered[3].ShouldBe(6);
        filtered[4].ShouldBe(8);
    }

    [Test]
    public void Ignore_Should_Ignore_Items_When_Matching_Predicate_Is_True()
    {
        var numbers = Enumerable.Range(0, 10);

        var filtered = numbers.Ignore(n => n % 2 == 0).ToArray();

        filtered.Length.ShouldBe(5);
        filtered[0].ShouldBe(1);
        filtered[1].ShouldBe(3);
        filtered[2].ShouldBe(5);
        filtered[3].ShouldBe(7);
        filtered[4].ShouldBe(9);
    }

    [Test]
    public void IndexOf()
    {
        var items = Enumerable.Range(1, 5);

        items.IndexOf(1).ShouldBe(0);
        items.IndexOf(2).ShouldBe(1);
        items.IndexOf(3).ShouldBe(2);
        items.IndexOf(4).ShouldBe(3);
        items.IndexOf(5).ShouldBe(4);
        items.IndexOf(6).ShouldBe(-1);
    }

    [Test]
    public void IndexOf_Should_ReturnTheIndexOfAnItem_When_PredicateIsTrue()
    {
        var items = new string[] { "a", "b", "c", "b", "d" };

        var index = items.IndexOf("b");
        index.ShouldBe(1);

        index = items.IndexOf(index + 1, x => x == "b");
        index.ShouldBe(3);
    }


    [Test]
    public void Insert_Should_InsertItem_When_EmptyEnumerable_Predicate()
    {
        var items = new List<int>();
        var item = 4;

        var newItems = items.Insert(item, n => n > 3).ToArray();

        newItems.ShouldContain(item);
    }

    [Test]
    public void Insert_Should_InsertItem_When_EmptyEnumerable_UsingComparer()
    {
        var items = new List<int>();
        var item = 4;

        var newItems = items.Insert(item, Comparer<int>.Default).ToArray();

        newItems.ShouldContain(item);
    }

    [Test]
    public void InsertAt_Should_InsertItem_When_EmptyEnumerableAndIndexIs0()
    {
        var items = Enumerable.Empty<int>();
        var item = 4;

        var newItems = items.InsertAt(item, 0).ToArray();

        newItems.Length.ShouldBe(1);
        newItems[0].ShouldBe(item);
    }

    [Test]
    public void InsertAt_Should_InsertItem_When_ListHasAnElementAndIndexIs0()
    {
        var items = Enumerable.Range(1, 1);
        var item = 4;

        var newItems = items.InsertAt(item, 0).ToArray();

        newItems.Length.ShouldBe(2);
        newItems[0].ShouldBe(item);
        newItems[1].ShouldBe(1);
    }

    [Test]
    public void InsertAt_Should_NotInsertItem_When_EmptyEnumerableAndIndexIs1()
    {
        var items = Enumerable.Empty<int>();
        var item = 4;

        var newItems = items.InsertAt(item, 1).ToArray();

        newItems.Length.ShouldBe(0);
    }

    [Test]
    public void InsertAt_Should_InsertItem_When_3ElementsAndIndexIs1()
    {
        IEnumerable<int> items = new[] { 1, 2, 3 };
        var item = 4;

        var newItems = items.InsertAt(item, 1).ToArray();

        newItems.Length.ShouldBe(4);
        newItems[0].ShouldBe(1);
        newItems[1].ShouldBe(item);
        newItems[2].ShouldBe(2);
        newItems[3].ShouldBe(3);
    }

    [Test]
    public void InsertAt_Should_InsertItem_When_3ElementsAndIndexIs2()
    {
        IEnumerable<int> items = new[] { 1, 2, 3 };
        var item = 4;

        var newItems = items.InsertAt(item, 2).ToArray();

        newItems.Length.ShouldBe(4);
        newItems[0].ShouldBe(1);
        newItems[1].ShouldBe(2);
        newItems[2].ShouldBe(item);
        newItems[3].ShouldBe(3);
    }

    [Test]
    public void InsertAt_Should_NotInsert_When_3ElementsAndIndexIs5()
    {
        IEnumerable<int> items = new[] { 1, 2, 3 };
        var item = 4;

        var newItems = items.InsertAt(item, 5).ToArray();

        newItems.Length.ShouldBe(3);
        newItems[0].ShouldBe(1);
        newItems[1].ShouldBe(2);
        newItems[2].ShouldBe(3);
    }

    [Test]
    public void Intersect_Should_ReturnTwoElements_When_AllListsContainTwoSameElements()
    {
        var items = new List<IEnumerable<int>>
        {
            new int[] { 1, 2, 3, 4 },
            new int[] { 2, 3, 4, 5 },
            new int[] { 3, 4, 5, 6 }
        };

        var intersected = items.Intersect().ToArray();

        intersected.Length.ShouldBe(2);
        intersected[0].ShouldBe(3);
        intersected[1].ShouldBe(4);
    }

    [Test]
    public void Insert_Should_InsertAnItem_When_Using_Comparer()
    {
        var items = new [] { 1, 3, 5 };

        var newItems = items.Insert(4, Comparer<int>.Default).ToArray();

        var expected = new[] { 1, 3, 4, 5 };
        CollectionAssert.AreEqual(expected, newItems);
    }

    [Test]
    public void Insert_Should_InsertAnItem_When_Using_Predicate()
    {
        var items = new[] { 1, 3, 5 };
        {
            var newItems = items.Insert(4, n => n > 3).ToArray();

            var expected = new[] { 1, 3, 4, 5 };

            newItems.ShouldBe(expected);
        }
        {
            var newItems = items.Insert(4, n => n > 1 && n <= 3).ToArray();

            var expected = new[] { 1, 4, 3, 5 };

            newItems.ShouldBe(expected);
        }
    }

    [Test]
    public void KCombinations_Should_ReturnPermutations_WithoutRepetitions_When_RepetitionsIsNotSet_Using_No_Duplicates()
    {
        var numbers = Enumerable.Range(1, 3);

        var kCombinations = numbers.KCombinations(2).ToArray();

        kCombinations.Length.ShouldBe(3);

        kCombinations.Any(g => g.EqualsCollection(new[] { 1, 2 })).ShouldBeTrue();
        kCombinations.Any(g => g.EqualsCollection(new[] { 1, 3 })).ShouldBeTrue();
        kCombinations.Any(g => g.EqualsCollection(new[] { 2, 3 })).ShouldBeTrue();
    }

    [Test]
    public void KCombinationsWithRepetition_Should_ReturnKCombinations_When_RepetitionsIsNotSet_Using_No_Duplicates()
    {
        var numbers = Enumerable.Range(1, 3);

        var kCombinations = numbers.KCombinationsWithRepetition(2).ToArray();

        kCombinations.Length.ShouldBe(6);

        kCombinations.Any(g => g.EqualsCollection(new[] { 1, 1 })).ShouldBeTrue();
        kCombinations.Any(g => g.EqualsCollection(new[] { 1, 2 })).ShouldBeTrue();
        kCombinations.Any(g => g.EqualsCollection(new[] { 1, 3 })).ShouldBeTrue();
        kCombinations.Any(g => g.EqualsCollection(new[] { 2, 2 })).ShouldBeTrue();
        kCombinations.Any(g => g.EqualsCollection(new[] { 2, 3 })).ShouldBeTrue();
        kCombinations.Any(g => g.EqualsCollection(new[] { 3, 3 })).ShouldBeTrue();
    }

    [Test]
    public void Match_Should_ReturnAllMatchingItems_When_KeyMatches()
    {
        var dates1 = new List<DateTime>
        {
            new (2017, 4, 1),
            new (2017, 5, 2),
            new (2017, 9, 3),
            new (2018, 7, 1)
        };

        var dates2 = new List<DateTime>
        {
            new (2019, 2, 5),
            new (2019, 6, 1),
            new (2020, 4, 1)
        };

        var (lhs, rhs) = dates1.Match(dates2, dt => dt.Day);

        var lhsArray = lhs.ToArray();
        var rhsArray = rhs.ToArray();

        lhsArray.Length.ShouldBe(2);
        rhsArray.Length.ShouldBe(2);

        lhsArray.ShouldContain(new DateTime(2017, 4, 1));
        lhsArray.ShouldContain(new DateTime(2018, 7, 1));
        rhsArray.ShouldContain(new DateTime(2019, 6, 1));
        rhsArray.ShouldContain(new DateTime(2020, 4, 1));
    }

    [Test]
    public void Match_Should_ReturnAllMatchingItems_When_CompositeKeyMatches()
    {
        var dates1 = new List<DateTime>
        {
            new (2017, 4, 1),
            new (2017, 5, 2),
            new (2017, 9, 3),
            new (2018, 7, 1)
        };

        var dates2 = new List<DateTime>
        {
            new (2019, 2, 5),
            new (2019, 6, 1),
            new (2020, 4, 1)
        };

        var (lhs, rhs) = dates1.Match(dates2, dt => new { dt.Day, dt.Month });

        var lhsFound = lhs.Single();
        var rhsFound = rhs.Single();

        lhsFound.ShouldBe(new DateTime(2017, 4, 1));
        rhsFound.ShouldBe(new DateTime(2020, 4, 1));
    }

    [Test]
    public void Match_Should_ReturnValues_When_ItemsMatch()
    {
        var lhs = new[] { 3, 2, 2, 1 };
        var rhs = new[] { 1, 3, 4, 3 };

        var (l, r) = lhs.Match(rhs, x => x);

        var lhsMatch = l.OrderBy(x => x).ToArray();

        lhsMatch.Length.ShouldBe(2);

        {
            lhsMatch[0].ShouldBe(1);
            lhsMatch[1].ShouldBe(3);
        }

        var rhsMatch = r.OrderBy(x => x).ToArray();

        rhsMatch.Length.ShouldBe(3);
        {
            rhsMatch[0].ShouldBe(1);
            rhsMatch[1].ShouldBe(3);
            rhsMatch[2].ShouldBe(3);
        }
    }

    [Test]
    public void Match_Should_ReturnValues_When_ItemsMatchKey()
    {
        var dates1 = new List<DateTime>
        {
           new DateTime(2017, 4, 13),
           new DateTime(2017, 5,  2),
           new DateTime(2017, 9,  3),
           new DateTime(2018, 7,  1),
        };

        var dates2 = new List<DateTime>
        {
            new DateTime(2015, 4, 29),
            new DateTime(2019, 2,  5),
            new DateTime(2019, 6,  1),
            new DateTime(2020, 4,  1)
        };

        var (lhs, rhs) = dates1.Match(dates2, dt => new { dt.Month });

        var lhsFound = lhs.Single();

        lhsFound.ShouldBe(new DateTime(2017, 4, 13));

        var rhsFound = rhs.ToArray();
        rhsFound.Length.ShouldBe(2);

        rhsFound[0].ShouldBe(new DateTime(2015, 4, 29));
        rhsFound[1].ShouldBe(new DateTime(2020, 4, 1));
    }

    [Test]
    public void MatchWithOccurrencies_Should_ReturnValuesWithTheirOccurrencies_When_ItemsMatch()
    {
        var lhs = new[] { 3, 2, 2, 1 };
        var rhs = new[] { 1, 3, 4, 3 };

        var matching = lhs.MatchWithOccurrencies(rhs).ToArray();

        matching.Length.ShouldBe(2);
        {
            var tuple = matching.First(t => t.lhs.item == 1);
            tuple.lhs.counter.ShouldBe(1);
            tuple.rhs.counter.ShouldBe(1);
        }
        {
            var tuple = matching.First(t => t.lhs.item == 3);

            tuple.lhs.counter.ShouldBe(1);
            tuple.rhs.counter.ShouldBe(2);
        }
    }

    [Test]
    public void MatchWithOccurrencies_Should_ReturnValuesWithTheirOccurrencies_When_KeysMatch()
    {
        var dates1 = new List<DateTime>
        {
           new DateTime(2017, 4, 13),
           new DateTime(2017, 5,  2),
           new DateTime(2017, 9,  3),
           new DateTime(2018, 7,  1)
        };

        var dates2 = new List<DateTime>
        {
            new DateTime(2015, 4, 29),
            new DateTime(2019, 2,  5),
            new DateTime(2019, 6,  1),
            new DateTime(2020, 4,  1)
        };

        var matching = dates1.MatchWithOccurrencies(dates2, dt => dt.Month).ToArray();

        matching.Length.ShouldBe(1);

        var expectedLhs = (counter: 1, item: new DateTime(2017, 4, 13));
        var expectedRhs = (counter: 2, item: new DateTime(2017, 4, 13));

        var tuple = matching[0];

        tuple.lhs.ShouldBe(expectedLhs);
        tuple.rhs.ShouldBe(expectedRhs);
    }

    [Test]
    public void MatchWithOccurrencies_Should_NotReturnValues_When_ItemsDonotMatch()
    {
        var lhs = new[] { 1, 2, 3 };
        var rhs = new[] { 4, 5, 6 };

        var matching = lhs.MatchWithOccurrencies(rhs).ToArray();

        matching.Length.ShouldBe(0);
    }

    [Test]
    public void MatchWithOccurrencies_Should_ReturnValuesWithTheirOccurrencies_When_ItemsMatchIncludingNullValues()
    {
        var lhs = new int?[] { 3, 2, null, 2, 1 };
        var rhs = new int?[] { 1, null, 3, 4, null, 3 };

        var matching = lhs.MatchWithOccurrencies(rhs).ToArray();

        matching.Length.ShouldBe(3);
        {
            var tuple = matching.First(t => t.lhs.item == null);

            tuple.lhs.counter.ShouldBe(1);
            tuple.rhs.counter.ShouldBe(2);
        }
        {
            var tuple = matching.First(t => t.lhs.item == 1);

            tuple.lhs.counter.ShouldBe(1);
            tuple.rhs.counter.ShouldBe(1);
        }
        {
            var tuple = matching.First(t => t.lhs.item == 3);

            tuple.lhs.counter.ShouldBe(1);
            tuple.rhs.counter.ShouldBe(2);
        }
    }

    [Test]
    public void MaxBy()
    {
        string[] items = ["A", "ABC", "AB", "ABCD"];

        var max = items.MaxBy((a, b) => a.Length > b.Length ? 1 : -1);

        max.ShouldBe("ABCD");
    }

    [Test]
    public void MinBy()
    {
        string[] items = ["A", "ABC", "AB", "ABCD"];

        var min = items.MinBy((a, b) => a.Length > b.Length ? 1 : -1);

        min.ShouldBe("A");
    }

    [Test]
    public void MinMax_Should_ReturnMinMax_When_UsingSelectorWithDifferentValues()
    {
        var dates = new DateOnly[]
        {
            new (2015, 2, 10),
            new (2016, 7, 11),
            new (2017, 3,  5),
            new (2018, 1,  1),
            new (2019, 5, 26),
            new (2020, 4, 13),
        };

        var (min, max) = dates.MinMax(dt => dt.Month).OrThrow();

        var expectedMin = new DateOnly(2018, 1, 1);
        var expectedMax = new DateOnly(2016, 7, 11);

        (min, max).ShouldBe((expectedMin, expectedMax));
    }

    [Test]
    public void MinMax_Should_ReturnMinMax_When_RepeatingValues()
    {
        var numbers = new[] { 1, 2, 2, 2, 5, 3, 3, 3, 3, 4 };

        var (min, max) = numbers.MinMax().OrThrow();

        var expected = (1, 5);

        (min, max).ShouldBe(expected);
    }

    [Test]
    public void MostFrequent_Should_ReturnRightValue_When_MultipleMaxValue()
    {
        var numbers = new[] { 1, 2, 2, 2, 2, 3, 3, 3, 3, 4 };

        var (mostFrequent, count) = numbers.MostFrequent(x => x);

        var items = mostFrequent.ToArray();

        items.Length.ShouldBe(2);

        items[0].ShouldBe(2);
        items[1].ShouldBe(3);

        count.ShouldBe(4);
    }

    [Test]
    public void MostFrequent_Should_ReturnRightValue_When_SingleMaxValue()
    {
        var numbers = new[] { 1, 2, 2, 3, 3, 3, 3, 4 };

        var (mostFrequent, count) = numbers.MostFrequent(x => x);

        var items = mostFrequent.ToArray();

        items.Length.ShouldBe(1);

        items[0].ShouldBe(3);

        count.ShouldBe(4);
    }

    [Test]
    public void NotOfType_Should_ReturnRightValue_When_SingleMaxValue()
    {
        var values = new object[] { 1, "2", new DateOnly(2022, 3, 5), 4, "5", 6.6 };

        var noString = values.NotOfType(typeof(string)).ToArray();

        Assert.AreEqual(4, noString.Length);

        Assert.AreEqual(1, noString[0]);
        Assert.AreEqual(new DateOnly(2022, 3, 5), noString[1]);
        Assert.AreEqual(4, noString[2]);
        Assert.AreEqual(6.6, noString[3]);
    }

    [Test]
    public void Nth_Should_ReturnItemAtIndex_When_UsingValidIndex()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };

        Assert.AreEqual(1, items.Nth(0).OrThrow());
        Assert.AreEqual(2, items.Nth(1).OrThrow());
        Assert.AreEqual(3, items.Nth(2).OrThrow());
        Assert.AreEqual(4, items.Nth(3).OrThrow());
        Assert.AreEqual(5, items.Nth(4).OrThrow());
    }

    [Test]
    public void Nth_Should_ReturnNone_When_UsingInvalidIndex()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        var none = Option.None<int>();

        Assert.AreEqual(none, items.Nth(-1));
        Assert.AreEqual(none, items.Nth(10));
    }

    [Test]
    public void Nth_Should_ReturnNone_When_ListOfIntegersIsEmpty()
    {
        var items = Enumerable.Empty<int>();

        //items.FirstOrDefault() would return 0.

        var first = items.Nth(0);

        Assert.IsTrue(first.IsNone);
    }

    [Test]
    public void Nths_ReturnItemsFromMinToEnd_When_OnlyMinIsSet()
    {
        var list = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        var items = list.AsEnumerable();
        const int min = 2;

        var foundItems = items.Nths(min..).ToArray();

        Assert.AreEqual(8, foundItems.Length);

        for (int i = min, j = 0; i < list.Count; i++, j++)
            Assert.AreEqual(list[i], foundItems[j]);
    }

    [Test]
    public void Nths_Should_ReturnItemsFromStartToMax_When_OnlyMaxIsSet()
    {
        var list = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        var items = list.AsEnumerable();
        const int max = 5;

        var foundItems = items.Nths(..max).ToArray();

        Assert.AreEqual(6, foundItems.Length);

        for (int i = 0, j = 0; i < max; i++, j++)
            Assert.AreEqual(list[i], foundItems[j]);
    }

    [Test]
    public void Nths_Should_ReturnItemsFromMinToMax_When_MinAndMaxIstSet()
    {
        var list = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        var items = list.AsEnumerable();
        const int min = 2;
        const int max = 6;

        var foundItems = items.Nths(min..max).ToArray();

        Assert.AreEqual(5, foundItems.Length);

        for (int i = min, j = 0; i <= max; i++, j++)
            Assert.AreEqual(list[i], foundItems[j]);
    }

    [Test]
    public void Nths_Should_ReturnItemsOnlyFromMinToEnd_When_MaxExceedsMaximumIndex()
    {
        var list = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        var items = list.AsEnumerable();
        const int min = 5;
        const int max = 15;

        var foundItems = items.Nths(min..max).ToArray();

        Assert.AreEqual(5, foundItems.Length);

        var end = min + foundItems.Length - 1;

        for (int i = min, j = 0; i <= end; i++, j++)
            Assert.AreEqual(list[i], foundItems[j]);
    }

    [Test]
    public void Nths_Should_ReturnItemsAtIndices_When_ItemsExistAtIndices()
    {
        var items = Enumerable.Range(0, 10);

        var selected = items.Nths(new[] { 1, 2, 5, 7 }).ToArray();

        Assert.AreEqual(4, selected.Length);

        Assert.AreEqual(1, selected[0]);
        Assert.AreEqual(2, selected[1]);
        Assert.AreEqual(5, selected[2]);
        Assert.AreEqual(7, selected[3]);
    }

    [Test]
    public void Nths_Should_ReturnItemsOnlyAtValidIndices_When_IncludingInvalidIndices()
    {
        var items = Enumerable.Range(0, 10);

        //with invalid indexes

        var selected = items.Nths(new[] { -1, 2, 5, 17 }).ToArray();

        Assert.AreEqual(2, selected.Length);

        Assert.AreEqual(2, selected[0]);
        Assert.AreEqual(5, selected[1]);
    }

    [Test]
    public void Nths_Should_ThrowException_When_MinIsNegative()
    {
        var list = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        var items = list.AsEnumerable();

        Assert.Throws<ArgumentOutOfRangeException>(() => items.Nths(-5..9).ToArray());
    }
    [Test]
    public void Nths_Should_ReturnItemsOfEvenPositions_When_PredicateSelectsEvenPositions()
    {
        var items = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        var selected = items.Nths(index => (index % 2) == 0).ToArray();

        Assert.AreEqual(items.Count / 2, selected.Length);
        Assert.AreEqual("0", selected[0]);
        Assert.AreEqual("2", selected[1]);
        Assert.AreEqual("4", selected[2]);
        Assert.AreEqual("6", selected[3]);
        Assert.AreEqual("8", selected[4]);
    }

    [Test]
    public void Occurrencies_ShouldReturnTheRightQuantity_When_AValuesOccurresMoreThanOneTimes()
    {
        var numbers = new[] { 1, 2, 1, 3, 4, 1, 3, 5 };
        var occurrencies = numbers.Occurrencies().ToArray();

        var distinct = numbers.Distinct().ToArray();
        Assert.AreEqual(distinct.Length, occurrencies.Length);
        {
            var (value, quantity) = occurrencies[0];
            Assert.AreEqual(1, value);
            Assert.AreEqual(3, quantity);
        }
        {
            var (value, quantity) = occurrencies[1];
            Assert.AreEqual(2, value);
            Assert.AreEqual(1, quantity);
        }
        {
            var (value, quantity) = occurrencies[2];
            Assert.AreEqual(3, value);
            Assert.AreEqual(2, quantity);
        }
        {
            var (value, quantity) = occurrencies[3];
            Assert.AreEqual(4, value);
            Assert.AreEqual(1, quantity);
        }
        {
            var (value, quantity) = occurrencies[4];
            Assert.AreEqual(5, value);
            Assert.AreEqual(1, quantity);
        }
    }

    [Test]
    public void OfTypes_ShouldJumpIntoAction_When_UsedAction()
    {
        var items = new object[] { "1", 2, 3.3, "4", 5, 6.6 };

        var selected = items.OfTypes(typeof(string), typeof(double)).ToArray();

        Assert.AreEqual(4, selected.Length);

        var expected = new object[] { "1", 3.3, "4", 6.6 };
        CollectionAssert.AreEqual(expected, selected);
    }

    [Test]
    public void OnCombinationOnFirstAfterFirstAfterEachOnLast_WithArgument()
    {
        var numbers = Enumerable.Range(1, 10);
        var loopCounter = 0;

        var onFirstCounter = 0;
        var onFirstValue = -1;
        void onFirst(int x)
        {
            onFirstCounter++;
            onFirstValue = x;
        }

        var afterFirstCounter = 0;
        var afterFirstValue = -1;
        void afterFirst(int x)
        {
            afterFirstCounter++;
            afterFirstValue = x;
        }

        var afterEachCounter = 0;
        void afterEach(int x)
        {
            afterEachCounter++;
        }

        var onLastCounter = 0;
        var onLastValue = -1;
        void onLast(int x)
        {
            onLastCounter++;
            onLastValue = x;
        }

        foreach (var n in numbers.OnFirst(onFirst)
                                 .AfterFirst(afterFirst)
                                 .AfterEach(afterEach)
                                 .OnLast(onLast))
        {
            loopCounter++;
        }

        Assert.AreEqual(10, loopCounter);

        Assert.AreEqual(1, onFirstCounter);
        Assert.AreEqual(1, onFirstValue);

        Assert.AreEqual(1, afterFirstCounter);
        Assert.AreEqual(1, afterFirstValue);

        Assert.AreEqual(9, afterEachCounter);

        Assert.AreEqual(1, onLastCounter);
        Assert.AreEqual(10, onLastValue);
    }

    [Test]
    public void OnEmpty_ShouldCallAction_When_ListIsEmpty()
    {
        var items = Enumerable.Empty<int>();

        var called = false;
        foreach (var _ in items.OnEmpty(() => called = true))
        {
        }

        Assert.IsTrue(called);
    }

    [Test]
    public void OnEmpty_ShouldNotCallAction_When_ListIsNotEmpty()
    {
        var items = Enumerable.Range(0, 10);

        var called = false;
        foreach(var _ in items.OnEmpty(() => called = true))
        {
        }

        Assert.IsFalse(called);
    }

    [Test]
    public void OnEmpty_ShouldReturnValue_When_ListIsEmpty()
    {
        var items = Enumerable.Empty<int>();

        var expected = -1;
        var actual = items.OnEmpty(() => expected).First();

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void OnFirst_ShouldCallAction_When_ListIsNotEmpty()
    {
        var numbers = Enumerable.Range(0, 10);
        var actionCounter = 0;
        var loopCounter = 0;
        void action() => actionCounter++;

        foreach (var n in numbers.OnFirst(action))
            loopCounter++;

        Assert.AreEqual(1, actionCounter);
        Assert.AreEqual(10, loopCounter);
    }

    [Test]
    public void OnFirst_ShouldCallAction_When_ActionUsedWithArgument_And_ListIsNotEmpty()
    {
        var numbers = Enumerable.Range(0, 10);
        var actionCounter = 0;
        var actionValue = -1;
        var loopCounter = 0;

        void action(int x)
        {
            actionCounter++;
            actionValue = x;
        }

        foreach (var n in numbers.OnFirst(action))
            loopCounter++;

        Assert.AreEqual(1, actionCounter);
        Assert.AreEqual(0, actionValue);
        Assert.AreEqual(10, loopCounter);
    }

    [Test]
    public void OnLast()
    {
        var numbers = Enumerable.Range(0, 10);
        var actionCounter = 0;
        var loopCounter = 0;
        void action() => actionCounter++;

        foreach (var n in numbers.OnLast(action))
            loopCounter++;

        Assert.AreEqual(1, actionCounter);
        Assert.AreEqual(10, loopCounter);
    }

    [Test]
    public void OnLast_WithArgument()
    {
        var numbers = Enumerable.Range(0, 10);
        var loopCounter = 0;
        var actionCounter = 0;
        var actionValue = -1;

        void action(int x)
        {
            actionCounter++;
            actionValue = x;
        }

        foreach (var n in numbers.OnLast(action))
            loopCounter++;

        Assert.AreEqual(10, loopCounter);

        Assert.AreEqual(1, actionCounter);
        Assert.AreEqual(9, actionValue);
    }

    [Test]
    public void Pairs_Should_ReturnAListOfTuples_When_NotEmpty()
    {
        var numbers = Enumerable.Range(0, 5);

        var tuples = numbers.Pairs().ToArray();

        Assert.AreEqual(4, tuples.Length);

        Assert.AreEqual((0, 1), tuples[0]);
        Assert.AreEqual((1, 2), tuples[1]);
        Assert.AreEqual((2, 3), tuples[2]);
        Assert.AreEqual((3, 4), tuples[3]);
    }

    [Test]
    public void Pairs_ShouldReturnStrings_When_TransformToStrings()
    {
        var numbers = Enumerable.Range(0, 5);

        var strings = numbers.Pairs((lhs, rhs) => $"{lhs}, {rhs}").ToArray();

        Assert.AreEqual(4, strings.Length);

        Assert.AreEqual("0, 1", strings[0]);
        Assert.AreEqual("1, 2", strings[1]);
        Assert.AreEqual("2, 3", strings[2]);
        Assert.AreEqual("3, 4", strings[3]);
    }

    [Test]
    public void Partition_Should_ReturnTupleWithEvenAndOddNumbers_When_PredicateSelectsEventNumbers()
    {
        var numbers = Enumerable.Range(1, 10);

        var(matching, notMatching) = numbers.Partition(x => x % 2 == 0);

        var even = matching.ToArray();
        var odd = notMatching.ToArray();

        Assert.AreEqual(5, even.Length);
        Assert.AreEqual(5, odd.Length);

        Assert.Contains(2,  even);
        Assert.Contains(4,  even);
        Assert.Contains(6,  even);
        Assert.Contains(8,  even);
        Assert.Contains(10, even);

        Assert.Contains(1, odd);
        Assert.Contains(3, odd);
        Assert.Contains(5, odd);
        Assert.Contains(7, odd);
        Assert.Contains(9, odd);
    }

    [Test]
    public void Permutations2_Should_Return3Permutations_When_LengthIs1()
    {
        var numbers = Enumerable.Empty<int>();

        var permutations = numbers.Permutations(1);

        permutations.Any().ShouldBeFalse();

        var it = permutations.GetEnumerator();
        it.MoveNext().ShouldBeFalse();
    }

    [Test]
    public void Permutations_Should_Return3Permutations_When_LengthIs1()
    {
        var numbers = Enumerable.Range(1, 3);

        var permutations = numbers.Permutations(1).ToArray();

        Assert.AreEqual(3, permutations.Length);

        permutations[0].ShouldBe([1]);
        permutations[1].ShouldBe([2]);
        permutations[2].ShouldBe([3]);
    }

    [Test]
    public void Permutations_Should_ReturnPermutationsWithRepetitions_When_ContainsRepetitionsIsTrue()
    {
        var numbers = Enumerable.Range(1, 3);

        var permutations = numbers.Permutations(2).ToArray();

        permutations.Length.ShouldBe(9);

        permutations[0].ContainsSequence(new int[] { 1, 1 });
        permutations[1].ContainsSequence(new[] { 1, 2 });
        permutations[2].ContainsSequence(new[] { 1, 3 });
        permutations[3].ContainsSequence(new[] { 2, 1 });
        permutations[4].ContainsSequence(new[] { 2, 2 });
        permutations[5].ContainsSequence(new[] { 2, 3 });
        permutations[6].ContainsSequence(new[] { 3, 1 });
        permutations[7].ContainsSequence(new[] { 3, 2 });
        permutations[8].ContainsSequence(new[] { 3, 3 });
    }

    [Test]
    public void Permutations_Should_ReturnPermutationsWithoutRepetitions_When_ContainsRepetitionsIsFalse()
    {
        var numbers = new[] { 1, 2, 3 };

        var permutations = numbers.Permutations(2, false).ToArray();

        permutations[0].ContainsSequence(new[] { 1, 2 });
        permutations[1].ContainsSequence(new[] { 1, 3 });
        permutations[2].ContainsSequence(new[] { 2, 1 });
        permutations[3].ContainsSequence(new[] { 2, 3 });
        permutations[4].ContainsSequence(new[] { 3, 1 });
        permutations[5].ContainsSequence(new[] { 3, 2 });
    }

    [Test]
    public void Permutations_Should_Return2Permutations_When_Using2Lists()
    {
        var numbers = Enumerable.Range(1, 3);
        var strings = Enumerable.Range(1, 3).Select(x => x.ToString());

        var permutations = numbers.Permutations(strings).ToArray();

        permutations.Length.ShouldBe(9);

        permutations[0].ShouldBe((1, "1"));
        permutations[1].ShouldBe((1, "2"));
        permutations[2].ShouldBe((1, "3"));
        permutations[3].ShouldBe((2, "1"));
        permutations[4].ShouldBe((2, "2"));
        permutations[5].ShouldBe((2, "3"));
        permutations[6].ShouldBe((3, "1"));
        permutations[7].ShouldBe((3, "2"));
        permutations[8].ShouldBe((3, "3"));
    }

    //[Test]
    //public void Permutations_Should_Return3TuplesOfPermutations_When_RepetitionsFalse()
    //{
    //    var n1 = Enumerable.Range(1, 2);
    //    var n2 = Enumerable.Range(10, 2);
    //    var numbers = new[] { n1, n2 };

    //    var permutations = numbers.Permutations().ToArray();

    //    permutations.Length.ShouldBe(4);
    //    {
    //        var values = permutations[0];
    //        values.Should().ContainInOrder(new[] { 1, 10 });
    //    }
    //    {
    //        var values = permutations[1];
    //        values.Should().ContainInOrder(new[] { 1, 11 });
    //    }
    //    {
    //        var values = permutations[2];
    //        values.Should().ContainInOrder(new[] { 2, 10 });
    //    }
    //    {
    //        var values = permutations[3];
    //        values.Should().ContainInOrder(new[] { 2, 11 });
    //    }
    //}

    [Test]
    public void RandomSubset()
    {
        var numbers = Enumerable.Range(1, 5).ToArray();
        {
            var subset = numbers.RandomSubset(3).ToArray();

            Assert.AreEqual(3, subset.Length);
            foreach (var randomSelected in subset)
            {
                CollectionAssert.Contains(numbers, randomSelected);
            }
        }
        {
            var subset = numbers.RandomSubset(6).ToArray();

            Assert.AreEqual(5, subset.Length);
            Assert.IsTrue(subset.EqualsCollection(numbers));
        }
    }

    [Test]
    public void RemoveTail()
    {
        var numbers = Enumerable.Range(0, 5);
        var expected = Enumerable.Range(0, 4);

        var actual = numbers.RemoveTail().ToArray();

        actual.ShouldBe(expected);
    }

    [Test]
    public void Replace_Should_ReturnAModifiedList_When_ReplaceSingleItemAtIndex()
    {
        var numbers = Enumerable.Range(1, 5);

        var modified = numbers.Replace(2, 100).ToArray();

        modified.ShouldBeEquivalentTo(new[] { 1, 2, 100, 4, 5 });
    }

    [Test]
    public void Replace_ShouldReturnList_When_ReplaceFizzBuzz()
    {
        var numbers = Enumerable.Range(1, 20).Select(n => n.ToString());

        var fizzBuzz = "FizzBuzz";
        var fizz = "Fizz";
        var buzz = "Buzz";

        var all = numbers.Replace((index, n) =>
        {
            if (0 == index) return n;

            var pos = index + 1;

            if (0 == pos % 15) return fizzBuzz;
            if (0 == pos % 3) return fizz;
            if (0 == pos % 5) return buzz;

            return n;
        }).ToArray();

        foreach(var (counter, item) in all.Enumerate())
        {
            if(0 == counter)
            {
                item.ShouldBe("1");
                continue;
            }
            var pos = counter + 1;

            if (0 == pos % 15)
            {
                fizzBuzz.ShouldBe(item);
                continue;
            }
            if (0 == pos % 3)
            {
                fizz.ShouldBe(item);
                continue;
            }

            if (0 == pos % 5)
            {
                buzz.ShouldBe(item);
                continue;
            }

            item.ShouldBe(pos.ToString());
        }
    }

    [Test]
    public void Replace_Should_ReturnReplacedList_When_IndexValueTuples_AreUsed()
    {
        var numbers = Enumerable.Range(1, 5);

        // using tuples (value, index)
        var replaced = numbers.Replace(new[] { (1, 20), (3, 40) }).ToArray();
        replaced.ShouldBeEquivalentTo(new[] { 1, 20, 3, 40, 5 });
    }

    [Test]
    public void Replace_Should_ReturnReplacedList_When_IndexValueTuples_AreUsed_AndIntsTransformedToString()
    {
        var numbers = Enumerable.Range(1, 5);

        var modified = numbers.Replace(new[] { (1, 20), (3, 40) }, n => n.ToString()).ToArray();

        modified.Length.ShouldBe(5);
        modified[0].ShouldBe("1");
        modified[1].ShouldBe("20");
        modified[2].ShouldBe("3");
        modified[3].ShouldBe("40");
        modified[4].ShouldBe("5");
    }

    [Test]
    public void Replace_Should_ReturnReplacedList_When_ListOfIndexValues_IsUsed()
    {
        var numbers = Enumerable.Range(1, 5);

        var modified = numbers.Replace((_, n) => 0 == n % 2 ? n * 10 : n).ToArray();

        modified.Length.ShouldBe(5);
        modified[0].ShouldBe(1);
        modified[1].ShouldBe(20);
        modified[2].ShouldBe(3);
        modified[3].ShouldBe(40);
        modified[4].ShouldBe(5);
    }

    [Test]
    public void Replace_Should_ReturnReplacedList_When_PredicateIsUsed()
    {
        var numberOfElements = 5;
        var numbers = Enumerable.Range(1, numberOfElements);

        var modified = numbers.Replace((index, item) => index % 2 == 0, (index, item) => 0).ToArray();

        modified.Length.ShouldBe(numberOfElements);

        modified[0].ShouldBe(0);
        modified[1].ShouldBe(2);
        modified[2].ShouldBe(0);
        modified[3].ShouldBe(4);
        modified[4].ShouldBe(0);
    }

    [Test]
    public void Replace_Should_ReturnReplacedList_When_ListIsShorterThanMaxReplaceIndex()
    {
        var numbers = Enumerable.Range(1, 5);

        var replaced = numbers.Replace(new[] { (1, 20), (3, 40), (5, 60) }).ToArray();

        replaced.Length.ShouldBe(5);
        replaced[0].ShouldBe(1);
        replaced[1].ShouldBe(20);
        replaced[2].ShouldBe(3);
        replaced[3].ShouldBe(40);
        replaced[4].ShouldBe(5);
    }

    [Test]
    public void ScanLeft_Should_ReturnAListOfIntegers_When_ItemsIsAListOfIntegers()
    {
        var numbers = Enumerable.Range(1, 5);
        var scanned = numbers.ScanLeft(0, (x, y) => x + y)
                             .ToArray();

        // scanned == [1, 3, 6, 10, 15]

        // method iterations
        // 0 + 1             =  1
        // 1 + 2             =  3
        // 1 + 2 + 3         =  6
        // 1 + 2 + 3 + 4     = 10
        // 1 + 2 + 3 + 4 + 5 = 15

        scanned.ShouldBeEquivalentTo(new int[] { 1, 3, 6, 10, 15 });

    }

    [Test]
    public void ScanLeft_Should_ReturnAListOfStrings_When_ItemsIsAListOfStrings()
    {
        var values = new[] { "A", "B", "C" };
        var scanned = values.ScanLeft("z", (x, y) => x + y)
                            .ToArray();

        // scanned == ["zA", "zAB", "zABC"]

        // method iterations
        // z   + A   = zA
        // zA  + B   = zAB
        // zAB + C   = zABC

        scanned.ShouldBeEquivalentTo(new string[] { "zA", "zAB", "zABC" });

    }

    [Test]
    public void ScanRight_Should_ReturnAListOfIntegers_When_ItemsAreIntegers()
    {
        var numbers = Enumerable.Range(1, 5);
        var scanned = numbers.ScanRight(0, (x, y) => x + y)
                             .ToArray();

        //method iterations
        //5 + 4 + 3 + 2 + 1 = 15
        //5 + 4 + 3 + 2     = 14
        //5 + 4 + 3         = 12
        //5 + 4             =  9
        //5 + 0             =  5
        //0                 =  0

        scanned.ShouldBeEquivalentTo(new int[] { 15, 14, 12, 9, 5, 0 });
    }

    [Test]
    public void ScanRight_Should_ReturnAListOfStrings_When_ItemsAreStrings()
    {
        var values = new[] { "A", "B", "C" };
        var scanned = values.ScanRight("z", (x, y) => x + y)
                            .ToArray();

        // scanned == [ "ABCz", "BCz", "Cz", "z"  ]

        // method iterations
        // A + B + C + z = ABCz
        // B + C + z     = BCz
        // C + z         = Cz
        // z             = z

        scanned.ShouldBeEquivalentTo(new string[] { "ABCz", "BCz", "Cz", "z" });
    }

    [Test]
    public void SequenceEqual_Should_ReturnFalse_When_ListsAreDifferent_RhsHasShorterLength()
    {
        var names = Enumerable.Range(10, 5).Select(x => x.ToString());
        var ages = Enumerable.Range(18, 5);

        var persons1 = names.Zip(ages).Select(tuple => new Person(tuple.Item1, tuple.Item2));
        var persons2 = names.Zip(ages).Select(tuple => new Person(tuple.Item1, tuple.Item2)).Take(2);

        Assert.IsFalse(persons1.SequenceEqual(persons2, (l, r) => l.Name == r.Name && l.Age == r.Age));
    }

    [Test]
    public void SequenceEqual_Should_ReturnFalse_When_ListsAreDifferent_RhsHasLargerLength()
    {
        var names = Enumerable.Range(10, 5).Select(x => x.ToString());
        var ages = Enumerable.Range(18, 5);

        var persons1 = names.Zip(ages).Select(tuple => new Person(tuple.Item1, tuple.Item2)).Take(2);
        var persons2 = names.Zip(ages).Select(tuple => new Person(tuple.Item1, tuple.Item2));

        persons1.SequenceEqual(persons2, (l, r) => l.Name == r.Name && l.Age == r.Age).ShouldBeFalse();
    }
    
    [Test]
    public void SequenceEqual_Should_ReturnTrue_When_ListsAreSame()
    {
        var names = Enumerable.Range(10, 5).Select(x => x.ToString());
        var ages = Enumerable.Range(18, 5);

        var persons1 = names.Zip(ages).Select(tuple => new Person(tuple.Item1, tuple.Item2));
        var persons2 = names.Zip(ages).Select(tuple => new Person(tuple.Item1, tuple.Item2));

        persons1.SequenceEqual(persons2, (l, r) => l.Name == r.Name && l.Age == r.Age).ShouldBeTrue();
    }

    [Test]
    public void Shingles_Should_Return1ShingleWith2Elements_When_ListHas2Elements_And_Kis5()
    {
        // Arrange
        var numbers = Enumerable.Range(1, 2);

        // Act
        var shingles = numbers.Shingles(5).ToArray();

        // ClassicAssert
        shingles.Length.ShouldBe(1);
        
        var shingle = shingles[0];
        Enumerable.Range(1, 2).SequenceEqual(shingle);
    }

    [Test]
    public void Shingles_Should_Return6Shingles_When_ListHas10Elements_And_Kis5()
    {
        // Arrange
        var numbers = Enumerable.Range(1, 10);

        // Act
        var shingles = numbers.Shingles(5).ToArray();

        // ClassicAssert
        shingles.Length.ShouldBe(6);
        {
            var shingle = shingles[0];
            Enumerable.Range(1, 5).SequenceEqual(shingle);
        }
        {
            var shingle = shingles[1];
            Enumerable.Range(2, 5).SequenceEqual(shingle);
        }
        {
            var shingle = shingles[2];
            Enumerable.Range(3, 5).SequenceEqual(shingle);
        }
        {
            var shingle = shingles[3];
            Enumerable.Range(4, 5).SequenceEqual(shingle);
        }
        {
            var shingle = shingles[4];
            Enumerable.Range(5, 5).SequenceEqual(shingle);
        }
        {
            var shingle = shingles[5];
            Enumerable.Range(6, 5).SequenceEqual(shingle);
        }
    }

    [Test]
    public void Shuffle()
    {
        var items = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        var shuffled = items.Shuffle().ToArray();

        shuffled.SequenceEqual(items).ShouldBeFalse();
        EnumerableExtensions.EqualsCollection(items, shuffled).ShouldBeTrue();
    }

    [Test]
    public void SkipUntilSatisfied_Should_ReturnOneValueForEachMach_When_ListHasDuplicateValues()
    {
        var numbers = new[] { 1, 2, 3, 2, 4, 4, 5, 6, 7 };
        var predicates = new Func<int, bool>[] { n => n == 2, n => n == 4 };

        // if a predicate is fulfilled it is no longer used
        var foundNumbers = numbers.SkipUntilSatisfied(predicates).ToArray();

        foundNumbers.Length.ShouldBe(4);

        foundNumbers[0].ShouldBe(4);
        foundNumbers[1].ShouldBe(5);
        foundNumbers[2].ShouldBe(6);
        foundNumbers[3].ShouldBe(7);
    }

    [Test]
    public void Slice_ShouldReturn2Lists_When_2_PredicatesAreUsed()
    {
        //{0, 1, 2, 3, 4, 5}
        var numbers = Enumerable.Range(0, 6);

        var spliced = numbers.Slice(n => n % 2 == 0, n => n % 2 != 0).ToArray();

        spliced.Length.ShouldBe(2);

        var even = spliced[0].ToArray();
        even.Length.ShouldBe(3);
        even[0].ShouldBe(0);
        even[1].ShouldBe(2);
        even[2].ShouldBe(4); 

        var odd = spliced[1].ToArray();
        odd.Length.ShouldBe(3);
        odd[0].ShouldBe(1);
        odd[1].ShouldBe(3);
        odd[2].ShouldBe(5);
    }

    [Test]
    public void Swap()
    {
        var count = 10;
        var numbers = Enumerable.Range(1, count);

        var swapped = numbers.Swap(2, 6).ToArray();

        var expected = new[] { 1, 2, 7, 4, 5, 6, 3, 8, 9, 10 };
        swapped.Length.ShouldBe(count);
        swapped.ShouldBeEquivalentTo(expected);
    }

    [Test]
    public void Slice_Should_ReturnListOf5Enumerables_When_ListWith10ElemsAndLength2()
    {
        var numberOfItems = 10;
        var chopSize = 2;
        var start = 1;

        var items = Enumerable.Range(start, numberOfItems);

        var slices = items.Slice(chopSize).ToArray();

        slices.Length.ShouldBe(5);
        var value = start;

        foreach (var slice in slices)
        {
            foreach (var v in slice)
            {
                value.ShouldBe(v);
                value++;
            }
        }
    }

    [Test]
    public void Slice_Should_ReturnListOf6Enumerables_When_ListWith11ElemsAndLength2()
    {
        var numberOfItems = 11;
        var chopSize = 2;
        var start = 1;

        var items = Enumerable.Range(start, numberOfItems);

        var slices = items.Slice(chopSize).ToArray();

        slices.Length.ShouldBe(6);
        var value = start;

        foreach (var slice in slices)
        {
            foreach (var v in slice)
            {
                value.ShouldBe(v);
                value++;
            }
        }
    }

    [Test]
    public void Take_Should_ReturnATupleWith3TakenAnd7Remaining_When_Taken3Elements()
    {
        var numbers = Enumerable.Range(0, 10).ToArray();

        var (taken, remaining) = numbers.Take(3, x => x.ToArray(), x => x.ToArray());

        taken.Length.ShouldBe(3);

        for(var i = 1; i < taken.Length; i++)
            taken[i].ShouldBe(i);

        remaining.Length.ShouldBe(7);

        for (var i = 0; i < remaining.Length; i++)
            remaining[i].ShouldBe(i + taken.Length);
    }

    [Test]
    public void TakeAtLeast_Should_Return0Elements_When_ListHas3ELementsAndNumberOfElementsIs4()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeAtLeast(4).ToArray();

        actual.Length.ShouldBe(0);
    }

    [Test]
    public void TakeAtLeast_Should_Return3Elements_When_ListHas3ELementsAndNumberOfElementsIs2()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeAtLeast(2).ToArray();
        actual.Length.ShouldBe(3);
    }

    [Test]
    public void TakeAtLeast_Should_Return3Elements_When_ListHas3ELementsAndNumberOfElementsIs3()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeAtLeast(3).ToArray();

        actual.Length.ShouldBe(3);
    }

    [Test]
    public void TakeExact_Should_Return0Elements_When_ListHas3ELementsAndNumberOfElementsIs2()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeExact(2).ToArray();

        actual.Length.ShouldBe(0);
    }

    [Test]
    public void TakeExact_Should_Return0Elements_When_ListHas3ELementsAndNumberOfElementsIs4()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeExact(4).ToArray();

        actual.Length.ShouldBe(0);
    }

    [Test]
    public void TakeExact_Should_Return3Elements_When_ListHas3ELementsAndNumberOfElementsIs3()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeExact(3).ToArray();

        actual.Length.ShouldBe(3);
    }

    [Test]
    public void TakeUntil_Should_Return4_When_InclusiveIsFalse()
    {
        var numbers = Enumerable.Range(1, 10);
        var foundNumbers = numbers.TakeUntil(x => x == 5).ToArray();

        foundNumbers.Length.ShouldBe(4);

        var expected = Enumerable.Range(1, 4);
        expected.SequenceEqual(foundNumbers).ShouldBeTrue();
    }

    [Test]
    public void TakeUntil_Should_Return5_When_InclusiveIsTrue()
    {
        var numbers = Enumerable.Range(1, 10);
        var foundNumbers = numbers.TakeUntil(x => x == 5, inclusive: true).ToArray();

        foundNumbers.Length.ShouldBe(5);

        var expected = Enumerable.Range(1, 5);
        expected.SequenceEqual(foundNumbers).ShouldBeTrue();
    }

    [Test]
    public void TakeUntilSatisfied_Should_ReturnOneValueForEachMach_When_ListHasDuplicateValues()
    {
        var numbers = new[] { 1, 2, 3, 2, 4, 4, 5, 6, 7 };
        var predicates = new Func<int, bool>[] { n => n == 2, n => n == 4, n => n == 6 };

        // if a predicate is fulfilled it is no longer used
        var foundNumbers = numbers.TakeUntilSatisfied(predicates).ToArray();

        foundNumbers.Length.ShouldBe(3);
        foundNumbers[0].ShouldBe(2);
        foundNumbers[1].ShouldBe(4);
        foundNumbers[2].ShouldBe(6);
    }

    [Test]
    public void TakeUntilSatisfied_Should_StopIteration_When_AllPredicatesMatched()
    {
        var numbers = new TestEnumerable<int>(Enumerable.Range(1, 10));

        var calledMoveNext = 0;
        void onMoveNext(bool hasNext) => calledMoveNext++;

        numbers.OnMoveNext.Subscribe(onMoveNext);

        var predicates = new Func<int, bool>[] { n => n == 2, n => n == 5 };

        var foundNumbers = numbers.TakeUntilSatisfied(predicates).ToArray();

        calledMoveNext.ShouldBe(6);
        foundNumbers.Length.ShouldBe(2);
        foundNumbers[0].ShouldBe(2);
        foundNumbers[1].ShouldBe(5);
    }

    [Test]
    public void MapOk_Should_StopIteration_When_AllPredicatesMatched()
    {
        var numbers = Enumerable.Range(1, 10).Select(x =>
        {
            return x % 2 == 0
            ? Result.Ok<int, Error>(x)
            : Result.Error<int, Error>(new Error("invalid operaion", "value must not be odd"));
        });

        var evenNumbers = numbers.MapOk().ToArray();

    }

    [Test]
    public void Zip()
    {
        var items1 = new List<A> { new A("1"), new A("2"), new A("3") };
        var items2 = new List<A> { new A("a"), new A("b"), new A("c"), new A("1"), new A("3") };

        var mapping = items1.Zip(items2, (f, s) => f.Name == s.Name, (f, s) => (f, s)).ToArray();

        mapping.Length.ShouldBe(2);

        foreach (var (f, s) in mapping)
        {
            f.Id.ShouldNotBe(s.Id);
            f.Name.ShouldBe(s.Name);
        }
    }

    [Test]
    public void Zip_ItemsInDifferentOrder()
    {
        var items1 = new List<A> { new A("1"), new A("2"), new A("3") };
        var items2 = new List<A> { new A("a"), new A("b"), new A("c"), new A("3"), new A("1") };

        var mapping = items1.Zip(items2, (f, s) => f.Name == s.Name, (f, s) => (f, s)).ToArray();

        mapping.Length.ShouldBe(2);  

        foreach (var (f, s) in mapping)
        {
            f.Id.ShouldNotBe(s.Id);
            f.Name.ShouldBe(s.Name);
        }
    }

    [Test]
    public void Zip_ListIncludesSameValues()
    {
        var items1 = new List<A> { new A("1"), new A("2"), new A("3") };
        var items2 = new List<A> { new A("a"), new A("b"), new A("c"), new A("1"), new A("3"), new A("1") };

        var mapping = items1.Zip(items2, (f, s) => f.Name == s.Name, (f, s) => (f, s)).ToArray();

        mapping.Length.ShouldBe(3);

        foreach (var (f, s) in mapping)
        {
            f.Id.ShouldNotBe(s.Id);
            f.Name.ShouldBe(s.Name);
        }
    }

    [Test]
    public void Zip_WithoutMappingValue()
    {
        var items1 = new List<A> { new A("1"), new A("2"), new A("3") };
        var items2 = new List<A> { new A("a"), new A("b"), new A("c") };

        var mapping = items1.Zip(items2, (f, s) => f.Name == s.Name, (f, s) => (f, s)).ToArray();

        mapping.Length.ShouldBe(0);
    }
}

// ReSharper restore InconsistentNaming
