using Foundation.Collections.Generic;
using Newtonsoft.Json.Bson;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Collections;

[TestFixture]
public class EnumerableExtensionsTests
{
    [Test]
    public void Occurrencies_Should_Return3Tuples_When_Added3DifferentItems()
    {
        var numbers = new int [] { 1, 1, 2, 2, 2, 3, 3, 3, 3 };

        var occurrencies = numbers.Occurrencies().ToArray();
        Assert.AreEqual(3, occurrencies.Length);
        Assert.AreEqual((1, 2), occurrencies[0]);
        Assert.AreEqual((2, 3), occurrencies[1]);
        Assert.AreEqual((3, 4), occurrencies[2]);
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

        Assert.AreEqual(12, permutations.Count());

        static IEnumerable<object> expected(params object[] values) => values;

        CollectionAssert.AreEqual(expected('a', 1, true),  permutations[0]);
        CollectionAssert.AreEqual(expected('a', 1, false), permutations[1]);
        CollectionAssert.AreEqual(expected('a', 2, true),  permutations[2]);
        CollectionAssert.AreEqual(expected('a', 2, false), permutations[3]);
        CollectionAssert.AreEqual(expected('b', 1, true),  permutations[4]);
        CollectionAssert.AreEqual(expected('b', 1, false), permutations[5]);
        CollectionAssert.AreEqual(expected('b', 2, true),  permutations[6]);
        CollectionAssert.AreEqual(expected('b', 2, false), permutations[7]);
        CollectionAssert.AreEqual(expected('c', 1, true),  permutations[8]);
        CollectionAssert.AreEqual(expected('c', 1, false), permutations[9]);
        CollectionAssert.AreEqual(expected('c', 2, true),  permutations[10]);
        CollectionAssert.AreEqual(expected('c', 2, false), permutations[11]);
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

        CollectionAssert.AreEqual(expected('a', 1, true),  permutations[0]);
        CollectionAssert.AreEqual(expected('a', 1, false), permutations[1]);
        CollectionAssert.AreEqual(expected('a', 2, true),  permutations[2]);
        CollectionAssert.AreEqual(expected('a', 2, false), permutations[3]);
        CollectionAssert.AreEqual(expected('b', 1, true),  permutations[4]);
        CollectionAssert.AreEqual(expected('b', 1, false), permutations[5]);
        CollectionAssert.AreEqual(expected('b', 2, true),  permutations[6]);
        CollectionAssert.AreEqual(expected('b', 2, false), permutations[7]);
        CollectionAssert.AreEqual(expected('c', 1, true),  permutations[8]);
        CollectionAssert.AreEqual(expected('c', 1, false), permutations[9]);
        CollectionAssert.AreEqual(expected('c', 2, true),  permutations[10]);
        CollectionAssert.AreEqual(expected('c', 2, false), permutations[11]);
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

        CollectionAssert.AreEqual(expected('a', 1, true),  permutations[0]);
        CollectionAssert.AreEqual(expected('a', 1, false), permutations[1]);
        CollectionAssert.AreEqual(expected('a', 2, true),  permutations[2]);
        CollectionAssert.AreEqual(expected('a', 2, false), permutations[3]);
        CollectionAssert.AreEqual(expected('b', 1, true),  permutations[4]);
        CollectionAssert.AreEqual(expected('b', 1, false), permutations[5]);
        CollectionAssert.AreEqual(expected('b', 2, true),  permutations[6]);
        CollectionAssert.AreEqual(expected('b', 2, false), permutations[7]);
        CollectionAssert.AreEqual(expected('c', 1, true),  permutations[8]);
        CollectionAssert.AreEqual(expected('c', 1, false), permutations[9]);
        CollectionAssert.AreEqual(expected('c', 2, true),  permutations[10]);
        CollectionAssert.AreEqual(expected('c', 2, false), permutations[11]);
    }
}
