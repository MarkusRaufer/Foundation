using Foundation.ComponentModel;
using Foundation.TestUtil.Collections.Generic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

    // ReSharper disable InconsistentNaming

    [Test]
    public void AfterEach()
    {
        var items = new List<string> { "1", "2", "3" };
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

        var minmax = numbers.Aggregate(number => (min: number, max: number), (acc, number) =>
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
        var items1 = new List<string> {"1", "2", "3"};
        var items2 = new List<string> {"a", "b", "c"};

        var erg = items1.CartesianProduct(items2, (l, r) => (l, r)).ToArray();
        Assert.AreEqual(("1", "a"), erg[0]);
        Assert.AreEqual(("1", "b"), erg[1]);
        Assert.AreEqual(("1", "c"), erg[2]);
        Assert.AreEqual(("2", "a"), erg[3]);
        Assert.AreEqual(("2", "b"), erg[4]);
        Assert.AreEqual(("2", "c"), erg[5]);
        Assert.AreEqual(("3", "a"), erg[6]);
        Assert.AreEqual(("3", "b"), erg[7]);
        Assert.AreEqual(("3", "c"), erg[8]);
    }
    
    [Test]
    public void Contains_AllNumbersWithinRange()
    {
        var items1 = Enumerable.Range(1, 10);
        var items2 = Enumerable.Range(4,  9);

        Assert.IsTrue(items1.Contains(items2));

        var items3 = new List<int> { 11, 12 };
        Assert.IsFalse(items1.Contains(items3));
    }

    [Test]
    public void Contains_IncludingNumbersOutOfRange()
    {
        var items1 = Enumerable.Range(0, 9);
        IEnumerable<int> items2 = new List<int> { 1, 5, 12 };

        Assert.IsTrue(items1.Contains(items2));
    }

    [Test]
    public void Cycle_ShouldReturn7CycledElements_When_Take7()
    {
        var items = new List<string> { "A", "B", "C" };

        var elements = items.Cycle().Take(7).ToArray();

        var expected = new[] { "A", "B", "C", "A", "B", "C", "A" };
        CollectionAssert.AreEqual(expected, elements);
    }

    [Test]
    public void CyclicEnumerate_MinMax()
    {
        var items = new List<string> { "A", "B", "C", "D", "E" };

        var enumerated = items.CycleEnumerate(1, 2).Take(5).ToArray();
        Assert.AreEqual(("A", 1), enumerated[0]);
        Assert.AreEqual(("B", 2), enumerated[1]);
        Assert.AreEqual(("C", 1), enumerated[2]);
        Assert.AreEqual(("D", 2), enumerated[3]);
        Assert.AreEqual(("E", 1), enumerated[4]);
    }

    [Test]
    public void SymmetricDifference_Should_ReturnAllItems_When_ListsAreCompletelyDifferent()
    {
        var items1 = Enumerable.Range(0, 10);
        var items2 = Enumerable.Range(10, 10);

        // return all items because lists are completely different
        var diff = items1.SymmetricDifference(items2).ToArray();

        Assert.AreEqual(20, diff.Length);

        Array.Sort(diff);
        CollectionAssert.AreEqual(Enumerable.Range(0, 20).ToArray(), diff);
    }

    [Test]
    public void SymmetricDifference_Should_ReturnNoItem_When_ListsHaveTheSameItems()
    {
        var items1 = Enumerable.Range(0, 10);
        var items2 = Enumerable.Range(0, 10);

        var diff = items1.SymmetricDifference(items2).ToArray();

        Assert.AreEqual(0, diff.Length);
    }

    [Test]
    public void SymmetricDifference_Should_ReturnDifferentItemsFromBothLists_When_BothListsHaveDifferentItems()
    {
        var items1 = new List<int> { 1, 2, 3, 4, 5 };
        var items2 = new List<int> { 2, 4, 6 };

        // return items of both lists that don't match
        var diff = items1.SymmetricDifference(items2).ToArray();

        Assert.AreEqual(4, diff.Length);

        Array.Sort(diff);
        CollectionAssert.AreEqual(new[] { 1, 3, 5, 6 }, diff);
    }

    [Test]
    public void SymmetricDifference_Should_Return3Items_When_ConsiderDuplicatesFalse()
    {
        var items1 = new int[] { 1, 1, 1, 1 };
        var items2 = new int[] { 1, 1, 2, 2, 3 };

        var diff = items1.SymmetricDifference(items2, retainDuplicates: false).ToArray();

        Assert.AreEqual(2, diff.Length);
        CollectionAssert.AreEqual(new[] { 2, 3 }, diff);
    }

    [Test]
    public void SymmetricDifference_Should_Return5Items_When_ConsiderDuplicatesTrue()
    {
        var items1 = new int[] { 1, 1, 1, 1 };
        var items2 = new int[] { 1, 1, 2, 2, 3 };

        var diff = items1.SymmetricDifference(items2, retainDuplicates: true).ToArray();

        Assert.AreEqual(5, diff.Length);
        CollectionAssert.AreEqual(new[] { 1, 1, 2, 2, 3 }, diff);
    }

    [Test]
    public void Duplicates_DistinctIsFalse_WithMultipleDuplicateValues()
    {
        var items = new List<int> { 1, 2, 3, 4, 5, 2, 4, 2 };

        var result = items.Duplicates().ToArray();

        Assert.AreEqual(3, result.Length);
        Assert.AreEqual(2, result[0]);
        Assert.AreEqual(4, result[1]);
        Assert.AreEqual(2, result[2]);
    }

    [Test]
    public void Duplicates_ShouldReturnDouplets_When_HasDuplicates()
    {
        var items = new List<int> { 1, 2, 3, 4, 5, 2, 4, 2 };

        var result = items.Duplicates().ToArray();

        Assert.AreEqual(3, result.Length);
        Assert.AreEqual(2, result[0]);
        Assert.AreEqual(4, result[1]);
        Assert.AreEqual(2, result[2]);
    }

    [Test]
    public void Duplicates_DistinctIsFalse_WithoutDuplicateValues()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };

        var result = items.Duplicates().ToArray();

        Assert.AreEqual(0, result.Length);
    }

    [Test]
    public void Enumerate_ShouldReturn9Elements_When_CalledWithCreateValue()
    {
        var items1 = Enumerable.Range(0, 9).ToArray();

        var enumerated = items1.Enumerate(n => n * 2).ToArray();

        Assert.AreEqual(items1.Length, enumerated.Length);

        foreach (var (item, counter) in enumerated)
            Assert.AreEqual(item * 2, counter);
    }

    [Test]
    public void Enumerate_ShouldReturn3Tuples_When_CalledWithoutParamenter()
    {
        var items = new[] { "one", "two", "three" };

        var enumerated = items.Enumerate().ToArray();

        Assert.AreEqual(("one", 0), enumerated[0]);
        Assert.AreEqual(("two", 1), enumerated[1]);
        Assert.AreEqual(("three", 2), enumerated[2]);
    }

    [Test]
    public void Enumerate_ShouldReturn3Tuples_When_CalledWithParamenter()
    {
        var items = new[] { "one", "two", "three" };
        var i = 10;

        var enumerated = items.Enumerate(item => i++).ToArray();

        Assert.AreEqual(("one",   10), enumerated[0]);
        Assert.AreEqual(("two",   11), enumerated[1]);
        Assert.AreEqual(("three", 12), enumerated[2]);
    }

    [Test]
    public void Enumerate_ShouldReturn10Tuples_When_UsingMinMax_On10Items()
    {
        var items = Enumerable.Range(1, 10).Select(x => x.ToString());

        var enumerated = items.Enumerate(min: -1, max: 1).ToArray();

        Assert.AreEqual(("1",  -1), enumerated[0]);
        Assert.AreEqual(("2",   0), enumerated[1]);
        Assert.AreEqual(("3",   1), enumerated[2]);
        Assert.AreEqual(("4",  -1), enumerated[3]);
        Assert.AreEqual(("5",   0), enumerated[4]);
        Assert.AreEqual(("6",   1), enumerated[5]);
        Assert.AreEqual(("7",  -1), enumerated[6]);
        Assert.AreEqual(("8",   0), enumerated[7]);
        Assert.AreEqual(("9",   1), enumerated[8]);
        Assert.AreEqual(("10", -1), enumerated[9]);
    }

    [Test]
    public void Enumerate_ShouldReturn10Tuples_When_UsingRange_On10Items()
    {
        var items = Enumerable.Range(1, 10).Select(x => x.ToString());

        var x = items.Enumerate(10..12).ToArray();

        var enumerated = items.Enumerate(10..12).ToArray();

        Assert.AreEqual(("1",  10), enumerated[0]);
        Assert.AreEqual(("2",  11), enumerated[1]);
        Assert.AreEqual(("3",  12), enumerated[2]);
        Assert.AreEqual(("4",  10), enumerated[3]);
        Assert.AreEqual(("5",  11), enumerated[4]);
        Assert.AreEqual(("6",  12), enumerated[5]);
        Assert.AreEqual(("7",  10), enumerated[6]);
        Assert.AreEqual(("8",  11), enumerated[7]);
        Assert.AreEqual(("9",  12), enumerated[8]);
        Assert.AreEqual(("10", 10), enumerated[9]);
    }

    [Test]
    public void Enumerate_ShouldReturn3Tuples_When_UsingWithSeed_On3Items()
    {
        var items = new[] { "1", "2", "3" };

        var enumerated = items.Enumerate(5).ToArray();

        Assert.AreEqual(("1", 5), enumerated[0]);
        Assert.AreEqual(("2", 6), enumerated[1]);
        Assert.AreEqual(("3", 7), enumerated[2]);
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

        Assert.AreEqual(1, different.Length);
        Assert.AreEqual("2", different[0].Name);
    }

    [Test]
    public void ExceptWithDuplicates_Should_Return4Doublets_When_LhsHasDuplicates()
    {
        var items1 = new[] { 1, 1, 1, 2, 3, 2, 1 };
        var items2 = new[] { 1, 2, 3, 4 };

        var result = items1.ExceptWithDuplicates(items2).OrderBy(x => x).ToArray();

        var expected = new[] { 1, 1, 1, 2 };
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [Test]
    public void ExceptWithDuplicates_Should_ReturnDoublets_When_LhsHasDuplicates()
    {
        var items1 = new[] { 1, 1, 1, 2, 3, 2, 1 };
        var items2 = new[] { 1, 2, 3, 1, 4 };

        var result = items1.ExceptWithDuplicates(items2).OrderBy(x => x).ToArray();

        var expected = new[] { 1, 1, 2 };
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [Test]
    public void ExceptWithDuplicates_Should_NoDoublets_When_LhsHasNoDuplicates()
    {
        var items1 = new[] { 1, 2, 3, 4 };
        var items2 = new[] { 2, 4 };

        var result = items1.ExceptWithDuplicates(items2).OrderBy(x => x).ToArray();

        var expected = new[] { 1, 3 };
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [Test]
    public void ExceptWithDuplicatesSorted_Should_ReturnDoublets_When_LhsHasDuplicates()
    {
        var items1 = new[] { 1, 1, 1, 2, 3, 2, 1 };
        var items2 = new[] { 1, 2, 3, 1, 4 };

        var result = items1.ExceptWithDuplicatesSorted(items2).OrderBy(x => x).ToArray();

        var expected = new[] { 1, 1, 2 };
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [Test]
    public void ExceptWithDuplicatesSorted_Should_NoDoublets_When_LhsHasNoDuplicates()
    {
        var items1 = new[] { 1, 2, 3, 4 };
        var items2 = new[] { 2, 4 };

        var result = items1.ExceptWithDuplicatesSorted(items2).OrderBy(x => x).ToArray();

        var expected = new[] { 1, 3 };
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [Test]
    public void ExceptWithDuplicatesSorted_Should_Return4Doublets_When_LhsHasDuplicates()
    {
        var items1 = new [] { 1, 1, 1, 2, 3, 2, 1 };
        var items2 = new [] { 1, 2, 3, 4 };
        
        var result = items1.ExceptWithDuplicatesSorted(items2).OrderBy(x => x).ToArray();

        var expected = new[] { 1, 1, 1, 2 };
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [Test]
    public void ForEach_Returning_number_of_processed_acctions()
    {
        var items = Enumerable.Range(1, 5);
        var sum = 0;

        items.ForEach(x => sum += x);

        Assert.AreEqual(15, sum);
    }

    [Test]
    public void ForEach_WithEmptyList()
    {
        var items = Enumerable.Empty<int>();
        var sum = 0;

        items.ForEach(action: x => sum += x, emptyAction: () => sum = 1);

        Assert.AreEqual(1, sum);
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
    public void Ignore_Should_Ignore_Items_When_Match_On_Indices()
    {
        var numbers = Enumerable.Range(0, 10);

        var filtered = numbers.Ignore(new[] { 1, 3, 5, 7, 9 }).ToArray();

        Assert.AreEqual(5, filtered.Length);
        Assert.AreEqual(0, filtered[0]);
        Assert.AreEqual(2, filtered[1]);
        Assert.AreEqual(4, filtered[2]);
        Assert.AreEqual(6, filtered[3]);
        Assert.AreEqual(8, filtered[4]);
    }

    [Test]
    public void Ignore_Should_Ignore_Items_When_Matching_Predicate_Is_True()
    {
        var numbers = Enumerable.Range(0, 10);

        var filtered = numbers.Ignore(n => n % 2 == 0).ToArray();

        Assert.AreEqual(5, filtered.Length);
        Assert.AreEqual(1, filtered[0]);
        Assert.AreEqual(3, filtered[1]);
        Assert.AreEqual(5, filtered[2]);
        Assert.AreEqual(7, filtered[3]);
        Assert.AreEqual(9, filtered[4]);
    }

    [Test]
    public void IndexOf()
    {
        var items = Enumerable.Range(1, 5);

        Assert.AreEqual(0, items.IndexOf(1));
        Assert.AreEqual(1, items.IndexOf(2));
        Assert.AreEqual(2, items.IndexOf(3));
        Assert.AreEqual(3, items.IndexOf(4));
        Assert.AreEqual(4, items.IndexOf(5));
        Assert.AreEqual(-1, items.IndexOf(6));
    }

    [Test]
    public void Insert_Should_InsertItem_When_EmptyEnumerable_Predicate()
    {
        var items = new List<int>();
        var item = 4;

        var newItems = items.Insert(item, n => n > 3).ToArray();

        Assert.Contains(item, newItems);
    }

    [Test]
    public void Insert_Should_InsertItem_When_EmptyEnumerable_UsingComparer()
    {
        var items = new List<int>();
        var item = 4;

        var newItems = items.Insert(item, Comparer<int>.Default).ToArray();

        Assert.Contains(item, newItems);
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
            CollectionAssert.AreEqual(expected, newItems);
        }
        {
            var newItems = items.Insert(4, n => n > 1 && n <= 3).ToArray();

            var expected = new[] { 1, 4, 3, 5 };
            CollectionAssert.AreEqual(expected, newItems);
        }
    }

    [Test]
    public void IsEqualToSet_Should_ReturnFalse_When_Items_SameNumberOfElementsAndDifferentOccrencies()
    {
        var items1 = new[] { 1, 2, 3, 2, 1 };
        var items2 = new[] { 2, 3, 2, 1, 2 };

        Assert.IsFalse(items1.IsEqualToSet(items2));
        Assert.IsFalse(items2.IsEqualToSet(items1));
    }

    [Test]
    public void IsEqualToSet_Should_ReturnTrue_When_SameNumberOfElementsAndSameOrder()
    {
        var items1 = Enumerable.Range(0, 5);
        var items2 = Enumerable.Range(0, 5);

        Assert.IsTrue(items1.IsEqualToSet(items2));
        Assert.IsTrue(items2.IsEqualToSet(items1));
    }

    [Test]
    public void IsEqualToSet_Should_ReturnTrue_When_Items_SameNumberOfElementsAndDifferentOrder()
    {
        var items1 = new[] { 1, 2, 3, 2 };
        var items2 = new[] { 2, 3, 2, 1 };

        Assert.IsTrue(items1.IsEqualToSet(items2));
        Assert.IsTrue(items2.IsEqualToSet(items1));
    }

    [Test]
    public void IsEqualToSet_Should_ReturnTrue_When_Items_SameNumberOfElementsAndSameOrder()
    {
        var items1 = Enumerable.Range(0, 5);
        var items2 = Enumerable.Range(0, 5);

        Assert.IsTrue(items1.IsEqualToSet(items2));
        Assert.IsTrue(items2.IsEqualToSet(items1));
    }

    [Test]
    public void IsEqualToSet_Should_ReturnFalse_When_DifferentNumberOfElements()
    {
        var items1 = Enumerable.Range(0, 5);
        var items2 = Enumerable.Range(0, 6);

        Assert.IsFalse(items1.IsEqualToSet(items2));
        Assert.IsFalse(items2.IsEqualToSet(items1));
    }

    [Test]
    public void IsInAscendingOrder_ShouldReturnFalse_When_ValuesAreNotAscending()
    {
        var numbers = new[] { 4, 3, 7, 9 };

        Assert.IsFalse(numbers.IsInAscendingOrder());
    }

    [Test]
    public void IsInAscendingOrder_ShouldReturnFalse_When_ValuesAreNotAscending_And_CompareIsUsed()
    {
        var numbers = new[] { 4, 3, 7, 9 };

        Assert.IsFalse(numbers.IsInAscendingOrder((a, b) =>
        {
            if (a < b) return CompareResult.Smaller;
            if (a > b) return CompareResult.Greater;
            return CompareResult.Equal;
        }));
    }

    [Test]
    public void IsInAscendingOrder_ShouldReturnTrue_When_ValuesAreAscending()
    {
        {
            var numbers = Enumerable.Range(1, 5);

            Assert.IsTrue(numbers.IsInAscendingOrder());
        }
        {
            var numbers = new[] { 3, 4, 4, 7, 9 };

            Assert.IsTrue(numbers.IsInAscendingOrder());
        }
    }

    [Test]
    public void IsInAscendingOrder_ShouldReturnTrue_When_ValuesAreAscending_And_CompareIsUsed()
    {
        {
            var numbers = Enumerable.Range(1, 5);

            Assert.IsTrue(numbers.IsInAscendingOrder((a, b) =>
            {
                if (a < b) return CompareResult.Smaller;
                if (a > b) return CompareResult.Greater;
                return CompareResult.Equal;
            }));
        }
        {
            var numbers = new[] { 3, 4, 4, 7, 9 };

            Assert.IsTrue(numbers.IsInAscendingOrder((a, b) =>
            {
                if (a < b) return CompareResult.Smaller;
                if (a > b) return CompareResult.Greater;
                return CompareResult.Equal;
            }));
        }
    }

    [Test]
    public void KCombinations_Should_ReturnPermutations_WithoutRepetitions_When_RepetitionsIsNotSet_Using_No_Duplicates()
    {
        var numbers = Enumerable.Range(1, 3);

        var kCombinations = numbers.KCombinations(2).ToArray();

        Assert.AreEqual(3, kCombinations.Length);

        Assert.IsTrue(kCombinations.Any(g => g.IsEqualToSet(new[] { 1, 2 })));
        Assert.IsTrue(kCombinations.Any(g => g.IsEqualToSet(new[] { 1, 3 })));
        Assert.IsTrue(kCombinations.Any(g => g.IsEqualToSet(new[] { 2, 3 })));
    }

    [Test]
    public void KCombinationsWithRepetition_Should_ReturnKCombinations_When_RepetitionsIsNotSet_Using_No_Duplicates()
    {
        var numbers = Enumerable.Range(1, 3);

        var kCombinations = numbers.KCombinationsWithRepetition(2).ToArray();

        Assert.AreEqual(6, kCombinations.Length);

        Assert.IsTrue(kCombinations.Any(g => g.IsEqualToSet(new[] { 1, 1 })));
        Assert.IsTrue(kCombinations.Any(g => g.IsEqualToSet(new[] { 1, 2 })));
        Assert.IsTrue(kCombinations.Any(g => g.IsEqualToSet(new[] { 1, 3 })));
        Assert.IsTrue(kCombinations.Any(g => g.IsEqualToSet(new[] { 2, 2 })));
        Assert.IsTrue(kCombinations.Any(g => g.IsEqualToSet(new[] { 2, 3 })));
        Assert.IsTrue(kCombinations.Any(g => g.IsEqualToSet(new[] { 3, 3 })));
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

        Assert.AreEqual(2, lhsArray.Length);
        Assert.AreEqual(2, rhsArray.Length);

        Assert.Contains(new DateTime(2017, 4, 1), lhsArray);
        Assert.Contains(new DateTime(2018, 7, 1), lhsArray);
        Assert.Contains(new DateTime(2019, 6, 1), rhsArray);
        Assert.Contains(new DateTime(2020, 4, 1), rhsArray);
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

        Assert.AreEqual(new DateTime(2017, 4, 1), lhsFound);
        Assert.AreEqual(new DateTime(2020, 4, 1), rhsFound);
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
        
        Assert.AreEqual((expectedMin, expectedMax), (min, max));
    }

    [Test]
    public void MinMax_Should_ReturnMinMax_When_RepeatingValues()
    {
        var numbers = new[] { 1, 2, 2, 2, 5, 3, 3, 3, 3, 4 };

        var (min, max) = numbers.MinMax().OrThrow();

        var expected = (1, 5);
        Assert.AreEqual(expected, (min, max));
    }

    [Test]
    public void MostFrequent_Should_ReturnRightValue_When_MultipleMaxValue()
    {
        var numbers = new[] { 1, 2, 2, 2, 2, 3, 3, 3, 3, 4 };

        var (mostFrequent, count) = numbers.MostFrequent(x => x);

        var items = mostFrequent.ToArray();
        Assert.AreEqual(2, items.Length);
        Assert.AreEqual(2, items[0]);
        Assert.AreEqual(3, items[1]);
        Assert.AreEqual(4, count);
    }

    [Test]
    public void MostFrequent_Should_ReturnRightValue_When_SingleMaxValue()
    {
        var numbers = new[] { 1, 2, 2, 3, 3, 3, 3, 4 };

        var (mostFrequent, count) = numbers.MostFrequent(x => x);

        var items = mostFrequent.ToArray();
        Assert.AreEqual(1, items.Length);
        Assert.AreEqual(3, items[0]);       // 3 occurs most often
        Assert.AreEqual(4, count);          // occurrs 4 times
    }

    [Test]
    public void MultipleOccurrences_Should_Return2Groupings_When_2NumbersOccureMoreThanOnce()
    {
        var numbers = new[] { 1, 2, 2, 3, 3, 3, 3, 4 };

        var multiOccurrences = numbers.MultipleOccurrences(x => x).ToArray();

        Assert.AreEqual(2, multiOccurrences.Length);

        Assert.AreEqual(2, multiOccurrences[0].Key);
        Assert.AreEqual(2, multiOccurrences[0].Count()); // 2 occurrences

        Assert.AreEqual(3, multiOccurrences[1].Key);
        Assert.AreEqual(4, multiOccurrences[1].Count()); // 4 occurrences
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

        var selected = items.Nths(1, 2, 5, 7).ToArray();

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

        var selected = items.Nths(-1, 2, 5, 17).ToArray();

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
    public void Permutations_Should_Return3Permutations_When_LengthIs1()
    {
        var numbers = Enumerable.Range(1, 3);

        var permutations = numbers.Permutations(1).ToArray();

        Assert.AreEqual(3, permutations.Length);

        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 1 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 2 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 3 })));
    }

    [Test]
    public void Permutations_Should_ReturnPermutationsWithRepetitions_When_ContainsRepetitionsIsTrue()
    {
        var numbers = Enumerable.Range(1, 3);

        var permutations = numbers.Permutations(2).ToArray();

        Assert.AreEqual(9, permutations.Length);

        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 1, 1 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 1, 2 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 1, 3 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 2, 1 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 2, 2 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 2, 3 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 3, 1 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 3, 2 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 3, 3 })));
    }

    [Test]
    public void Permutations_Should_ReturnPermutationsWithoutRepetitions_When_ContainsRepetitionsIsFalse()
    {
        var numbers = new[] { 1, 2, 3 };

        var permutations = numbers.Permutations(2, false).ToArray();

        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 1, 2 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 1, 3 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 2, 1 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 2, 3 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 3, 1 })));
        Assert.IsTrue(permutations.Any(g => g.IsEqualToSet(new[] { 3, 2 })));
    }

    [Test]
    public void Match_Should_ReturnValues_When_ItemsMatch()
    {
        var lhs = new[] { 3, 2, 2, 1 };
        var rhs = new[] { 1, 3, 4, 3 };

        var (l, r) = lhs.Match(rhs, x => x);

        var lhsMatch = l.OrderBy(x => x).ToArray();
        Assert.AreEqual(2, lhsMatch.Length);

        {
            Assert.AreEqual(1, lhsMatch[0]);
            Assert.AreEqual(3, lhsMatch[1]);
        }

        var rhsMatch = r.OrderBy(x => x).ToArray();

        Assert.AreEqual(3, rhsMatch.Length);
        {
            Assert.AreEqual(1, rhsMatch[0]);
            Assert.AreEqual(3, rhsMatch[1]);
            Assert.AreEqual(3, rhsMatch[2]);
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
        Assert.AreEqual(new DateTime(2017, 4, 13), lhsFound);

        var rhsFound = rhs.ToArray();
        Assert.AreEqual(2, rhsFound.Length);

        Assert.AreEqual(new DateTime(2015, 4, 29), rhsFound[0]);
        Assert.AreEqual(new DateTime(2020, 4,  1), rhsFound[1]);
    }

    [Test]
    public void MatchWithOccurrencies_Should_ReturnValuesWithTheirOccurrencies_When_ItemsMatch()
    {
        var lhs = new [] { 3, 2, 2, 1 };
        var rhs = new [] { 1, 3, 4, 3 };

        var matching = lhs.MatchWithOccurrencies(rhs).ToArray();

        Assert.AreEqual(2, matching.Length);
        {
            var tuple = matching.First(t => t.lhs.item == 1);
            Assert.AreEqual(1, tuple.lhs.count);
            Assert.AreEqual(1, tuple.rhs.count);
        }
        {
            var tuple = matching.First(t => t.lhs.item == 3);
            Assert.AreEqual(1, tuple.lhs.count);
            Assert.AreEqual(2, tuple.rhs.count);
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

        Assert.AreEqual(1, matching.Length);
        var expectedLhs = (item: new DateTime(2017, 4, 13), count: 1);
        var expectedRhs = (item: new DateTime(2017, 4, 13), count: 2);

        var tuple = matching[0];
        Assert.AreEqual(expectedLhs, tuple.lhs);
        Assert.AreEqual(expectedRhs, tuple.rhs);
    }

    [Test]
    public void MatchWithOccurrencies_Should_NotReturnValues_When_ItemsDonotMatch()
    {
        var lhs = new[] { 1, 2, 3 };
        var rhs = new[] { 4, 5, 6 };

        var matching = lhs.MatchWithOccurrencies(rhs).ToArray();

        Assert.AreEqual(0, matching.Length);
    }

    [Test]
    public void MatchWithOccurrencies_Should_ReturnValuesWithTheirOccurrencies_When_ItemsMatchIncludingNullValues()
    {
        var lhs = new int?[] { 3, 2, null, 2, 1 };
        var rhs = new int?[] { 1, null, 3, 4, null, 3 };

        var matching = lhs.MatchWithOccurrencies(rhs).ToArray();

        Assert.AreEqual(3, matching.Length);
        {
            var tuple = matching.First(t => t.lhs.item == null);
            Assert.AreEqual(1, tuple.lhs.count);
            Assert.AreEqual(2, tuple.rhs.count);
        }
        {
            var tuple = matching.First(t => t.lhs.item == 1);
            Assert.AreEqual(1, tuple.lhs.count);
            Assert.AreEqual(1, tuple.rhs.count);
        }
        {
            var tuple = matching.First(t => t.lhs.item == 3);
            Assert.AreEqual(1, tuple.lhs.count);
            Assert.AreEqual(2, tuple.rhs.count);
        }
    }

    [Test]
    public void MaxBy()
    {
        var items = new string[] { "A", "ABC", "AB", "ABCD" };

        var max = items.MaxBy((a, b) => a.Length > b.Length ? 1 : -1);

        Assert.AreEqual("ABCD", max);
    }

    [Test]
    public void MinBy()
    {
        var items = new string[] { "A", "ABC", "AB", "ABCD" };

        var min = items.MinBy((a, b) => a.Length > b.Length ? 1 : -1);

        Assert.AreEqual("A", min);
    }

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
            Assert.IsTrue(subset.IsEqualToSet(numbers));
        }
    }

    [Test]
    public void RemoveTail()
    {
        var numbers = Enumerable.Range(0, 5);
        var expected = Enumerable.Range(0, 4);

        var actual = numbers.RemoveTail();

        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void Replace_ShouldReturnTheModifiedList_When_ReplaceSingleItem()
    {
        var numbers = Enumerable.Range(1, 5);

        var modified = numbers.Replace(100, 2).ToArray();

        CollectionAssert.AreEqual(new[] { 1, 2, 100, 4, 5 }, modified);
    }

    [Test]
    public void Replace_ShouldReturnList_When_ReplaceFizzBuzz()
    {
        var numbers = Enumerable.Range(1, 20).Select(n => n.ToString());

        var fizzBuzz = "FizzBuzz";
        var fizz = "Fizz";
        var buzz = "Buzz";

        var all = numbers.Replace((n, index) =>
        {
            if (0 == index) return n;

            var pos = index + 1;

            if (0 == pos % 15) return fizzBuzz;
            if (0 == pos % 3) return fizz;
            if (0 == pos % 5) return buzz;

            return n;
        }).ToArray();

        foreach(var (item, counter) in all.Enumerate())
        {
            if(0 == counter)
            {
                Assert.AreEqual(item, "1");
                continue;
            }
            var pos = counter + 1;

            if (0 == pos % 15)
            {
                Assert.AreEqual(fizzBuzz, item);
                continue;
            }
            if (0 == pos % 3)
            {
                Assert.AreEqual(fizz, item);
                continue;
            }

            if (0 == pos % 5)
            {
                Assert.AreEqual(buzz, item);
                continue;
            }

            Assert.AreEqual(item, pos.ToString());
        }
    }

    [Test]
    public void Replace_Should_ReturnReplacedList_When_ListIsLongerThanMaxReplaceIndex()
    {
        var numbers = Enumerable.Range(1, 5);

        // using tuples (value, index)
        var replaced = numbers.Replace(new[] { (20, 1), (40, 3) }).ToArray();

        CollectionAssert.AreEqual(new[] {1, 20, 3, 40, 5}, replaced);
    }

    [Test]
    public void Replace_Should_ReturnReplacedList_When_ListIsLongerThanMaxReplaceIndexAndProjectIsSet()
    {
        var numbers = Enumerable.Range(1, 5);

        var replaced = numbers.Replace(new[] { (20, 1), (40, 3) }, n => n.ToString()).ToArray();

        Assert.AreEqual(5, replaced.Length);
        Assert.AreEqual("1", replaced[0]);
        Assert.AreEqual("20", replaced[1]);
        Assert.AreEqual("3", replaced[2]);
        Assert.AreEqual("40", replaced[3]);
        Assert.AreEqual("5", replaced[4]);
    }

    [Test]
    public void Replace_Should_ReturnReplacedList_WhenListIsLongerThanMaxReplaceIndexAndUsingPredicate()
    {
        var numbers = Enumerable.Range(1, 5);

        var replaced = numbers.Replace((n, _) => 0 == n % 2 ? n * 10 : n).ToArray();

        Assert.AreEqual(5, replaced.Length);
        Assert.AreEqual(1, replaced[0]);
        Assert.AreEqual(20, replaced[1]);
        Assert.AreEqual(3, replaced[2]);
        Assert.AreEqual(40, replaced[3]);
        Assert.AreEqual(5, replaced[4]);
    }

    [Test]
    public void Replace_Should_ReturnReplacedList_When_ListIsShorterThanMaxReplaceIndex()
    {
        var numbers = Enumerable.Range(1, 5);

        var replaced = numbers.Replace(new[] { (20, 1), (40, 3), (60, 5) }).ToArray();

        Assert.AreEqual(5, replaced.Length);
        Assert.AreEqual(1, replaced[0]);
        Assert.AreEqual(20, replaced[1]);
        Assert.AreEqual(3, replaced[2]);
        Assert.AreEqual(40, replaced[3]);
        Assert.AreEqual(5, replaced[4]);
    }

    [Test]
    public void Shuffle()
    {
        var items = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        var shuffled = items.Shuffle().ToArray();

        Assert.IsFalse(shuffled.SequenceEqual(items));
        Assert.IsTrue(EnumerableExtensions.IsEqualToSet(items, shuffled));
    }

    [Test]
    public void SkipUntilSatisfied_Should_ReturnOneValueForEachMach_When_ListHasDuplicateValues()
    {
        var numbers = new[] { 1, 2, 3, 2, 4, 4, 5, 6, 7 };
        var predicates = new Func<int, bool>[] { n => n == 2, n => n == 4 };

        // if a predicate is fulfilled it is no longer used
        var foundNumbers = numbers.SkipUntilSatisfied(predicates).ToArray();

        Assert.AreEqual(4, foundNumbers.Length);

        Assert.AreEqual(4, foundNumbers[0]);
        Assert.AreEqual(5, foundNumbers[1]);
        Assert.AreEqual(6, foundNumbers[2]);
        Assert.AreEqual(7, foundNumbers[3]);
    }


    [Test]
    public void Slice_ShouldReturn2Lists_When_2_PredicatesAreUsed()
    {
        //{0, 1, 2, 3, 4, 5}
        var numbers = Enumerable.Range(0, 6);

        var spliced = numbers.Slice(n => n % 2 == 0, n => n % 2 != 0).ToArray();

        Assert.AreEqual(2, spliced.Length);

        var even = spliced[0].ToArray();
        Assert.AreEqual(3, even.Length);
        Assert.AreEqual(0, even[0]);
        Assert.AreEqual(2, even[1]);
        Assert.AreEqual(4, even[2]);

        var odd = spliced[1].ToArray();
        Assert.AreEqual(3, odd.Length);
        Assert.AreEqual(1, odd[0]);
        Assert.AreEqual(3, odd[1]);
        Assert.AreEqual(5, odd[2]);
    }


    [Test]
    public void Slice_Should_ReturnListOf5Enumerables_When_ListWith10ElemsAndLength2()
    {
        var numberOfItems = 10;
        var chopSize = 2;
        var start = 1;

        var items = Enumerable.Range(start, numberOfItems);

        var slices = items.Slice(chopSize).ToArray();

        Assert.AreEqual(5, slices.Length);
        var value = start;

        foreach (var slice in slices)
        {
            foreach (var v in slice)
            {
                Assert.AreEqual(value, v);
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

        Assert.AreEqual(6, slices.Length);
        var value = start;

        foreach (var slice in slices)
        {
            foreach (var v in slice)
            {
                Assert.AreEqual(value, v);
                value++;
            }
        }
    }

    [Test]
    public void Take_Should_ReturnATupleWith3TakenAnd7Remaining_When_Taken3Elements()
    {
        var numbers = Enumerable.Range(0, 10).ToArray();

        var (taken, remaining) = numbers.Take(3, x => x.ToArray(), x => x.ToArray());

        Assert.AreEqual(3, taken.Length);

        for(var i = 1; i < taken.Length; i++)
            Assert.AreEqual(i, taken[i]);

        Assert.AreEqual(7, remaining.Length);
        for (var i=0; i < remaining.Length; i++)
            Assert.AreEqual(i + taken.Length, remaining[i]);
    }

    [Test]
    public void TakeAtLeast_Should_Return0Elements_When_ListHas3ELementsAndNumberOfElementsIs4()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeAtLeast(4).ToArray();
        Assert.AreEqual(0, actual.Length);
    }

    [Test]
    public void TakeAtLeast_Should_Return3Elements_When_ListHas3ELementsAndNumberOfElementsIs2()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeAtLeast(2).ToArray();
        Assert.AreEqual(3, actual.Length);
    }

    [Test]
    public void TakeAtLeast_Should_Return3Elements_When_ListHas3ELementsAndNumberOfElementsIs3()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeAtLeast(3).ToArray();
        Assert.AreEqual(3, actual.Length);
    }

    [Test]
    public void TakeExact_Should_Return0Elements_When_ListHas3ELementsAndNumberOfElementsIs2()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeExact(2).ToArray();
        Assert.AreEqual(0, actual.Length);
    }

    [Test]
    public void TakeExact_Should_Return0Elements_When_ListHas3ELementsAndNumberOfElementsIs4()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeExact(4).ToArray();
        Assert.AreEqual(0, actual.Length);
    }

    [Test]
    public void TakeExact_Should_Return3Elements_When_ListHas3ELementsAndNumberOfElementsIs3()
    {
        var items = new List<string> { "1", "2", "3" }.ToArray();

        var actual = items.TakeExact(3).ToArray();
        Assert.AreEqual(3, actual.Length);
    }

    [Test]
    public void TakeUntil_Should_Return4_When_InclusiveIsFalse()
    {
        var numbers = Enumerable.Range(1, 10);
        var foundNumbers = numbers.TakeUntil(x => x == 5).ToArray();

        Assert.AreEqual(4, foundNumbers.Length);

        var expected = Enumerable.Range(1, 4);
        Assert.IsTrue(expected.SequenceEqual(foundNumbers));
    }

    [Test]
    public void TakeUntil_Should_Return5_When_InclusiveIsTrue()
    {
        var numbers = Enumerable.Range(1, 10);
        var foundNumbers = numbers.TakeUntil(x => x == 5, inclusive: true).ToArray();

        Assert.AreEqual(5, foundNumbers.Length);

        var expected = Enumerable.Range(1, 5);
        Assert.IsTrue(expected.SequenceEqual(foundNumbers));
    }

    [Test]
    public void TakeUntilSatisfied_Should_ReturnOneValueForEachMach_When_ListHasDuplicateValues()
    {
        var numbers = new[] { 1, 2, 3, 2, 4, 4, 5, 6, 7 };
        var predicates = new Func<int, bool>[] { n => n == 2, n => n == 4, n => n == 6 };

        // if a predicate is fulfilled it is no longer used
        var foundNumbers = numbers.TakeUntilSatisfied(predicates).ToArray();

        Assert.AreEqual(3, foundNumbers.Length);
        Assert.AreEqual(2, foundNumbers[0]);
        Assert.AreEqual(4, foundNumbers[1]);
        Assert.AreEqual(6, foundNumbers[2]);
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

        Assert.AreEqual(6, calledMoveNext);
        Assert.AreEqual(2, foundNumbers.Length);
        Assert.AreEqual(2, foundNumbers[0]);
        Assert.AreEqual(5, foundNumbers[1]);
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

    [Test]
    public void ToBreakable()
    {
        {
            var items1 = Enumerable.Range(1, 3);
            var items2 = Enumerable.Range(1, 3);

            var i1 = 0;
            var i2 = 0;
            var stop = ObservableValue.New(false);
            foreach(var item1 in items1.ToBreakable(ref stop))
            {
                i1++;
                foreach (var item2 in items2.ToBreakable(ref stop))
                {
                    i2++;
                    
                    if (i2 == 2) stop.Value = true;
                }
            }

            Assert.AreEqual(1, i1);
            Assert.AreEqual(2, i2);
        }

        {
            var items1 = Enumerable.Range(1, 3);
            var items2 = Enumerable.Range(1, 3);
            var items3 = Enumerable.Range(1, 3);

            var i1 = 0;
            var i2 = 0;
            var i3 = 0;

            foreach (var item1 in items1)
            {
                var stop = ObservableValue.New(false);

                i1++;
                foreach (var item2 in items2.ToBreakable(ref stop))
                {
                    i2++;
                    foreach (var item3 in items3.ToBreakable(ref stop))
                    {
                        i3++;
                        if (item3 == 2) stop.Value = true;
                    }
                }
            }

            Assert.AreEqual(3, i1);
            Assert.AreEqual(3, i2);
            Assert.AreEqual(6, i3);
        }
    }

    [Test]
    public void ToBreakable_Cascaded()
    {
        var items1 = Enumerable.Range(0, 3);
        var items2 = Enumerable.Range(0, 3);
        var items3 = Enumerable.Range(0, 3);

        var i1 = 0;
        var i2 = 0;
        var i3 = 0;
        var stop = ObservableValue.New(false);
        var stopAll = ObservableValue.New(false);
        foreach (var item1 in items1.ToBreakable(ref stopAll))
        {
            i1++;
            foreach (var item2 in items2.ToBreakable(ref stop)
                                        .ToBreakable(ref stopAll))
            {
                i2++;
                foreach (var item3 in items3.ToBreakable(ref stop)
                                            .ToBreakable(ref stopAll))
                {
                    i3++;

                    if (item1 == 0 && item3 == 1)
                        stop.Value = true;

                    if (item2 == 1)
                        stopAll.Value = true;
                }
            }
        }

        Assert.AreEqual(2, i1);
        Assert.AreEqual(3, i2);
        Assert.AreEqual(6, i3);
    }

    [Test]
    public void ToDualStreams_Should_ReturnDualStreams_When_PredicateIsFizzBuzz_And_IsExhaustiveIsTrue()
    {
        var numbers = Enumerable.Range(1, 50);

        var re = new Regex("([0-9]+)");

        var all = numbers.ToDualStreams(n => 0 == n % 2, n => n, true)
                         .SelectLeft(_ => true, n => $"odd({n})")
                         .SelectRight(_ => true, n => $"even({n})")
                         .MergeAndSort(x => x, x => int.Parse(re.Match(x).Value))
                         .ToArray();

        foreach (var (item, counter) in all.Enumerate(seed: 1))
        {
            var expected = (0 == counter % 2) ? $"even({counter})" : $"odd({counter})";
            Assert.AreEqual(expected, item);
        }
    }

    [Test]
    public void ToDualOrdinalStreams_Should_ReturnDualOrdinalStreams_When_PredicateIsFizzBuzz_And_IsExhaustiveIsTrue()
    {
        var numbers = Enumerable.Range(1, 50);

        var fizzBuzz = "FizzBuzz";
        var fizz = "Fizz";
        var buzz = "Buzz";

        var all = numbers.ToDualOrdinalStreams(n => 0 == n % 15, _ => fizzBuzz, true)
                         .AddToRight(n => 0 == n % 3, _ => fizz, true)
                         .AddToRight(n => 0 == n % 5, _ => buzz, true)
                         .MergeStreams(n => n.ToString())
                         .ToArray();


        foreach (var (item, counter) in all.Enumerate(seed: 1))
        {
            if (0 == counter % 15)
            {
                Assert.AreEqual(fizzBuzz, item);
                continue;
            }
            if (0 == counter % 3)
            {
                Assert.AreEqual(fizz, item);
                continue;
            }

            if (0 == counter % 5)
            {
                Assert.AreEqual(buzz, item);
                continue;
            }

            Assert.AreEqual(item, counter.ToString());
        }
    }

    [Test]
    public void Zip()
    {
        var items1 = new List<A> { new A("1"), new A("2"), new A("3") };
        var items2 = new List<A> { new A("a"), new A("b"), new A("c"), new A("1"), new A("3") };

        var mapping = items1.Zip(items2, (f, s) => f.Name == s.Name, (f, s) => (f, s)).ToArray();

        Assert.AreEqual(2, mapping.Length);
        foreach (var (f, s) in mapping)
        {
            Assert.AreNotEqual(f.Id, s.Id);
            Assert.AreEqual(f.Name, s.Name);
        }
    }

    [Test]
    public void Zip_ItemsInDifferentOrder()
    {
        var items1 = new List<A> { new A("1"), new A("2"), new A("3") };
        var items2 = new List<A> { new A("a"), new A("b"), new A("c"), new A("3"), new A("1") };

        var mapping = items1.Zip(items2, (f, s) => f.Name == s.Name, (f, s) => (f, s)).ToArray();

        Assert.AreEqual(2, mapping.Length);
        foreach (var (f, s) in mapping)
        {
            Assert.AreNotEqual(f.Id, s.Id);
            Assert.AreEqual(f.Name, s.Name);
        }
    }

    [Test]
    public void Zip_ListIncludesSameValues()
    {
        var items1 = new List<A> { new A("1"), new A("2"), new A("3") };
        var items2 = new List<A> { new A("a"), new A("b"), new A("c"), new A("1"), new A("3"), new A("1") };

        var mapping = items1.Zip(items2, (f, s) => f.Name == s.Name, (f, s) => (f, s)).ToArray();

        Assert.AreEqual(3, mapping.Length);
        foreach (var (f, s) in mapping)
        {
            Assert.AreNotEqual(f.Id, s.Id);
            Assert.AreEqual(f.Name, s.Name);
        }
    }

    [Test]
    public void Zip_WithoutMappingValue()
    {
        var items1 = new List<A> { new A("1"), new A("2"), new A("3") };
        var items2 = new List<A> { new A("a"), new A("b"), new A("c") };

        var mapping = items1.Zip(items2, (f, s) => f.Name == s.Name, (f, s) => (f, s)).ToArray();

        Assert.AreEqual(0, mapping.Length);
    }
}

// ReSharper restore InconsistentNaming
