using FluentAssertions;
using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Foundation.Collections;

[TestFixture]
public class EnumerableExtensionsTests
{
    [Test]
    public void AggregateAsOption_Should_ReturnNone_When_ListIsEmpty()
    {
        var numbers = Enumerable.Empty<int>();

        var result = numbers.AggregateAsOption(x => x, (l, r) => l > r ? l : r);

        result.IsNone.Should().BeTrue();
    }

    [Test]
    public void AllOfSameType_Should_ReturnFalse_When_AllItemsAreObjectAndNotOfSameType()
    {
        var numbers = new object[] { 1, "2", 3, "4" };

        var allSame = numbers.AllOfSameType(x => x);

        allSame.Should().BeFalse();
    }

    [Test]
    public void AllOfSameType_Should_ReturnTrue_When_AllItemsAreObjectAndOfSameType()
    {
        var numbers = new object[] { 1, 2, 3, 4 };

        var allSame = numbers.AllOfSameType(x => x);

        allSame.Should().BeTrue();
    }

    [Test]
    public void AllOfSameType_Should_ReturnTrue_When_AllItemsAreDateTimeAndSelectionOfSameType()
    {
        static DateTime create(int day) => new DateTime(2020, 1, day);

        var dts = Enumerable.Range(1, 10).Select(create);

        var allSame = dts.AllOfSameType(x => x.Year);

        allSame.Should().BeTrue();
    }

    [Test]
    public void Ignore_Should_Return_9Elements_When_ListHas10ElementsAndOneIsIgnored()
    {
        var ids = Enumerable.Range(1, 10).Select(x => Id.New(x.ToString()));
        var toBeIgnored = Id.New("2");

        var filtered = ids.Ignore(toBeIgnored).ToArray();

        filtered.Length.Should().Be(9);
        filtered.Should().NotContain(toBeIgnored);
    }

    [Test]
    public void Ignore_Should_Return_9Elements_When_ListHas10ElementsAndAllDuplicatesAreIgnored()
    {
        var ids = Enumerable.Range(1, 10).Select(x => Id.New(x.ToString()));
        ids = ids.Concat(Enumerable.Repeat(2, 5).Select(x => Id.New(x.ToString())))
                 .Shuffle();

        var toBeIgnored = Id.New("2");

        var filtered = ids.Ignore(toBeIgnored).ToArray();

        filtered.Length.Should().Be(9);
        filtered.Should().NotContain(toBeIgnored);
    }

    [Test]
    public void LastAsOption_Should_Return_SomeGuid_When_HashSetIsNotEmpty()
    {
        var lastGuid = Guid.Empty;
        var guids = Enumerable.Range(1, 20).Select<int, Guid>(_ =>
        {
            var guid = Guid.NewGuid();
            lastGuid = guid;

            return guid;
        }).ToHashSet();

        var tail = guids.LastAsOption();

        tail.TryGet(out var last).Should().BeTrue();
        last.Should().Be(lastGuid);
    }

    [Test]
    public void LastAsOption_Should_Return_SomeGuid_When_ListIsNotEmpty()
    {
        var lastGuid = Guid.Empty;
        var guids = Enumerable.Range(1, 20).Select<int, Guid>(_ =>
        {
            var guid = Guid.NewGuid();
            lastGuid = guid;

            return guid;
        }).ToList();

        var tail = guids.LastAsOption();

        tail.TryGet(out var last).Should().BeTrue();
        last.Should().Be(lastGuid);
    }

    [Test]
    public void MinAsOption_Should_ReturnNone_When_ListIsEmpty()
    {
        var numbers = Enumerable.Empty<int>();

        var result = numbers.MinAsOption();

        result.IsNone.Should().BeTrue();
    }

    [Test]
    public void MinAsOption_Should_ReturnSome_5_When_5_IsMinimum()
    {
        var numbers = Enumerable.Range(5, 15);

        var result = numbers.MinAsOption();

        result.TryGet(out int value).Should().BeTrue();
        value.Should().Be(5);
    }

    [Test]
    public void Occurrencies_Should_Return3Tuples_When_Added3DifferentItems()
    {
        var numbers = new int [] { 1, 1, 2, 2, 2, 3, 3, 3, 3 };

        var occurrencies = numbers.Occurrencies().ToArray();

        occurrencies.Length.Should().Be(3);
        occurrencies[0].Should().Be((1, 2));
        occurrencies[1].Should().Be((2, 3));
        occurrencies[2].Should().Be((3, 4));
    }

    [Test]
    public void Permutations_ShouldReturn_AnEnumerableOfPermutations_When_WhenListsHasValues()
    {
        var lists = new object[][]
        {
            new object[] { 'a', 'b', 'c' },
            new object[] { 1, 2 },
            new object[] { true, false }
        };

        var permutations = lists.Permutations().ToArray();

        permutations.Count().Should().Be(12);

        static IEnumerable<object> expected(params object[] values) => values;

        permutations[0].Should().BeEquivalentTo(expected('a', 1, true));
        permutations[1].Should().BeEquivalentTo(expected('a', 1, false));
        permutations[2].Should().BeEquivalentTo(expected('a', 2, true));
        permutations[3].Should().BeEquivalentTo(expected('a', 2, false));
        permutations[4].Should().BeEquivalentTo(expected('b', 1, true));
        permutations[5].Should().BeEquivalentTo(expected('b', 1, false));
        permutations[6].Should().BeEquivalentTo(expected('b', 2, true));
        permutations[7].Should().BeEquivalentTo(expected('b', 2, false));
        permutations[8].Should().BeEquivalentTo(expected('c', 1, true));
        permutations[9].Should().BeEquivalentTo(expected('c', 1, false));
        permutations[10].Should().BeEquivalentTo(expected('c', 2, true));
        permutations[11].Should().BeEquivalentTo(expected('c', 2, false));
    }

    [Test]
    public void PermutationsArrays_ShouldReturn_AnArrayOfPermutations_When_WhenListsHasValues()
    {
        var lists = new object[][]
        {
            new object[] { 'a', 'b', 'c' },
            new object[] { 1, 2 },
            new object[] { true, false }
        };

        var permutations = lists.PermutationsArrays();

        Assert.AreEqual(12, permutations.Count());

        static object[] expected(params object[] values) => values;

        permutations[0].Should().BeEquivalentTo(expected('a', 1, true));
        permutations[1].Should().BeEquivalentTo(expected('a', 1, false));
        permutations[2].Should().BeEquivalentTo(expected('a', 2, true));
        permutations[3].Should().BeEquivalentTo(expected('a', 2, false));
        permutations[4].Should().BeEquivalentTo(expected('b', 1, true));
        permutations[5].Should().BeEquivalentTo(expected('b', 1, false));
        permutations[6].Should().BeEquivalentTo(expected('b', 2, true));
        permutations[7].Should().BeEquivalentTo(expected('b', 2, false));
        permutations[8].Should().BeEquivalentTo(expected('c', 1, true));
        permutations[9].Should().BeEquivalentTo(expected('c', 1, false));
        permutations[10].Should().BeEquivalentTo(expected('c', 2, true));
        permutations[11].Should().BeEquivalentTo(expected('c', 2, false));
    }

    [Test]
    public void PermutationsLists_ShouldReturn_AListOfPermutations_When_WhenListsHasValues()
    {
        var lists = new List<List<object>>
        {
            new List<object> { 'a', 'b', 'c' },
            new List<object> { 1, 2 },
            new List<object> { true, false }
        };

        var permutations = lists.PermutationsLists();

        Assert.AreEqual(12, permutations.Count());

        static List<object> expected(params object[] values) => values.ToList();

        permutations[0].Should().BeEquivalentTo(expected('a', 1, true));
        permutations[1].Should().BeEquivalentTo(expected('a', 1, false));
        permutations[2].Should().BeEquivalentTo(expected('a', 2, true));
        permutations[3].Should().BeEquivalentTo(expected('a', 2, false));
        permutations[4].Should().BeEquivalentTo(expected('b', 1, true));
        permutations[5].Should().BeEquivalentTo(expected('b', 1, false));
        permutations[6].Should().BeEquivalentTo(expected('b', 2, true));
        permutations[7].Should().BeEquivalentTo(expected('b', 2, false));
        permutations[8].Should().BeEquivalentTo(expected('c', 1, true));
        permutations[9].Should().BeEquivalentTo(expected('c', 1, false));
        permutations[10].Should().BeEquivalentTo(expected('c', 2, true));
        permutations[11].Should().BeEquivalentTo(expected('c', 2, false));
    }
}
