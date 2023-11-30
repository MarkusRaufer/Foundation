using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Collections.Generic;


[TestFixture]
public class ReadOnlyDictionaryExtensionsTests
{
    [Test]
    public void Replace_Should_Return_KeyValuesWithReplacedKeyValuesWhichExist_When_AddNonExistingReplacements_IsFalse()
    {
        var dict = new Dictionary<int, string>
        {
            { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" },
        };

        var replacement = new Dictionary<int, string>
        {
            { 2, "zwei" }, { 4, "vier" }, { 5, "fünf" },
        };

        var replaced = dict.Replace(replacement, addNonExistingReplacements: false).ToDictionary(x => x.Key, x => x.Value);

        replaced.Count.Should().Be(dict.Count);
        
        replaced[1].Should().Be("one");
        replaced[2].Should().Be("zwei");
        replaced[3].Should().Be("three");
        replaced[4].Should().Be("vier");
    }

    [Test]
    public void Replace_Should_Return_KeyValuesWithReplacedKeyValuesIncludingNonExistingReplacements_When_AddNonExistingReplacements_IsTrue()
    {
        var dict = new Dictionary<int, string>
        {
            { 1, "one" }, { 2, "two" }, { 3, "three" }, { 4, "four" },
        };

        var replacement = new Dictionary<int, string>
        {
            { 2, "zwei" }, { 4, "vier" }, { 5, "fünf" },
        };

        var replaced = dict.Replace(replacement, addNonExistingReplacements: true).ToDictionary(x => x.Key, x => x.Value);

        replaced.Count.Should().Be(dict.Count + 1);

        replaced[1].Should().Be("one");
        replaced[2].Should().Be("zwei");
        replaced[3].Should().Be("three");
        replaced[4].Should().Be("vier");
        replaced[5].Should().Be("fünf");
    }
}
