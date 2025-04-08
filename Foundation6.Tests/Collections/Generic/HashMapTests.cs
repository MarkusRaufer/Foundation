using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Foundation.Collections.Generic;

[TestFixture]
public class HashMapTests
{
    [Test]
    public void Ctor_Should_HaveNoElements_When_Created()
    {
        // Act
        var sut = new HashMap<int, string>();

        // ClassicAssert
        sut.Count.Should().Be(0);
    }

    [Test]
    public void Add_Should_AddAnElements_When_KeyAndValue_DoNotExist()
    {
        // Arrange
        var sut = new HashMap<int, string>();
        
        // Act
        sut.Add(1, 1.ToString());

        // ClassicAssert
        sut.Count.Should().Be(1);

        var value = sut[1];
        value.Should().Be(1.ToString());
    }

    [Test]
    public void Add_Should_NotAddAnElement_When_ExactKeyAndValueExist()
    {
        // Arrange
        var sut = new HashMap<int, string>();
        sut.Add(1, 1.ToString());

        // Act
        sut.Add(1, 1.ToString());

        // ClassicAssert
        sut.Count.Should().Be(1);
    }

    [Test]
    public void Add_Should_NotAddAnElement_When_ValueExists()
    {
        // Arrange
        var sut = new HashMap<int, string>();
        sut.Add(1, 1.ToString());
        sut.Add(2, 2.ToString());

        // Act
        sut.Add(3, 1.ToString());

        // ClassicAssert
        sut.Count.Should().Be(2);
        {
            var value = sut[1];
            value.Should().Be(1.ToString());
        }
        {
            var value = sut[2];
            value.Should().Be(2.ToString());
        }
    }

    [Test]
    public void Add_Should_NotAddAnElement_When_KeyExist()
    {
        // Arrange
        var sut = new HashMap<int, string>();
        sut.Add(1, 1.ToString());

        // Act
        sut.Add(1, 10.ToString());

        // ClassicAssert
        sut.Count.Should().Be(1);

        var value = sut[1];
        value.Should().Be(1.ToString());
    }


    [Test]
    public void Clear_Should_RemoveAllElements_When_Called()
    {
        // Arrange
        var sut = new HashMap<int, string>
        {
            // Act
            { 1, 1.ToString() },
            { 2, 2.ToString() }
        };

        // Act
        sut.Clear();

        // ClassicAssert
        sut.Count.Should().Be(0);
    }


    [Test]
    public void Count_Should_Be2_When_2ElementsWithDifferentKeysAndValuesAdded()
    {
        // Act
        var sut = new HashMap<int, string>
        {
            // Act
            { 1, 1.ToString() },
            { 2, 2.ToString() }
        };

        // ClassicAssert
        sut.Count.Should().Be(2);
    }

    [Test]
    public void Indexer_Should_AddAnElement_When_KeyAndValue_DoNotExist()
    {
        // Arrange
        var sut = new HashMap<int, string>();

        // Act
        sut[1] = 1.ToString();

        // ClassicAssert
        sut.Count.Should().Be(1);

        var value = sut[1];
        value.Should().Be(1.ToString());
    }

    [Test]
    public void Indexer_Should_NotAddAnElement_When_ExactKeyAndValueExist()
    {
        // Arrange
        var sut = new HashMap<int, string>
        {
            [1] = 1.ToString(),
        };

        // Act
        sut[1] = 1.ToString();

        // ClassicAssert
        sut.Count.Should().Be(1);
    }

    [Test]
    public void Indexer_Should_NotAddAnElement_When_ValueExists()
    {
        // Arrange
        var sut = new HashMap<int, string>
        {
            [1] = 1.ToString(),
            [2] = 2.ToString(),
        };

        // Act
        sut[3] = 1.ToString();

        // ClassicAssert
        sut.Count.Should().Be(2);
        {
            var value = sut[1];
            value.Should().Be(1.ToString());
        }
        {
            var value = sut[2];
            value.Should().Be(2.ToString());
        }
    }

    [Test]
    public void Indexer_Should_ReplaceAnElement_When_KeyExistButValueNot()
    {
        // Arrange
        var sut = new HashMap<int, string>()
        {
            [1] = 1.ToString(),
            [2] = 2.ToString(),
        };

        // Act
        sut[1] = 10.ToString();

        // ClassicAssert
        sut.Count.Should().Be(2);

        var value = sut[1];
        value.Should().Be(10.ToString());
    }

    [Test]
    public void Remove_Should_ReturnFalse_When_KeyExistsButValueNot()
    {
        // Arrange
        var sut = new HashMap<int, string>
        {
            { 1, 1.ToString() },
            { 2, 2.ToString() }
        };

        // Act
        var removed = sut.Remove(new KeyValuePair<int, string>(1, 10.ToString()));

        // ClassicAssert
        sut.Count.Should().Be(2);
        removed.Should().BeFalse();
    }

    [Test]
    public void Remove_Should_ReturnTrue_When_KeyAndValueExist()
    {
        // Arrange
        var sut = new HashMap<int, string>
        {
            { 1, 1.ToString() },
            { 2, 2.ToString() }
        };

        // Act
        var removed = sut.Remove(new KeyValuePair<int, string>(1, 1.ToString()));

        // ClassicAssert
        sut.Count.Should().Be(1);
        removed.Should().BeTrue();

        var value = sut[2];
        value.Should().Be(2.ToString());
    }

    [Test]
    public void Remove_Should_ReturnTrue_When_KeyExists()
    {
        // Arrange
        var sut = new HashMap<int, string>();
        sut.Add(1, 1.ToString());
        sut.Add(2, 2.ToString());

        // Act
        var removed = sut.Remove(1);

        // ClassicAssert
        sut.Count.Should().Be(1);
        removed.Should().BeTrue();

        var value = sut[2];
        value.Should().Be(2.ToString());
    }

    [Test]
    public void TryGetKey_Should_ReturnTrue_When_ValueExists()
    {
        // Arrange
        var sut = new HashMap<int, string>
        {
            [1] = 1.ToString(),
            [2] = 2.ToString(),
            [3] = 3.ToString(),
        };

        // Act
        var found = sut.TryGetKey(2.ToString(), out var key);

        // ClassicAssert
        found.Should().BeTrue();
        key.Should().Be(2);
    }

    [Test]
    public void TryGetValue_Should_ReturnTrue_When_KeyExists()
    {
        // Arrange
        var sut = new HashMap<int, string>
        {
            [1] = 1.ToString(),
            [2] = 2.ToString(),
            [3] = 3.ToString(),
        };

        // Act
        var found = sut.TryGetValue(2, out var value);

        // ClassicAssert
        found.Should().BeTrue();
        value.Should().Be(2.ToString());
    }
}
