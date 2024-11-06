using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Foundation.Collections.Generic;

[TestFixture]
public class HashMapValueTests
{
    [Test]
    public void Ctor_Should_HaveNoElements_When_Created()
    {           
        // Arrange
        var keyValues = new Dictionary<int, string> {
            { 1, 1.ToString() },
            { 2, 2.ToString() }
        };

        // Act
        var sut = new HashMapValue<int, string>(keyValues);

        // Assert
        sut.Count.Should().Be(keyValues.Count);
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_KeysAndValuesAreDifferent()
    {
        // Arrange
        var sut1 = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 1, 1.ToString() },
            { 2, 2.ToString() },
        });

        var sut2 = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 3, 3.ToString() },
            { 4, 4.ToString() },
        });

        // Act

        var equals = sut1.Equals(sut2);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_KeysAreDifferentButValuesAreSame()
    {
        // Arrange
        var sut1 = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 1, 1.ToString() },
            { 2, 2.ToString() },
        });

        var sut2 = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 3, 1.ToString() },
            { 4, 2.ToString() },
        });

        // Act

        var equals = sut1.Equals(sut2);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_KeysAreSameButValuesAreDifferent()
    {
        // Arrange
        var sut1 = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 1, 1.ToString() },
            { 2, 2.ToString() },
        });

        var sut2 = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 1, 3.ToString() },
            { 2, 2.ToString() },
        });

        // Act

        var equals = sut1.Equals(sut2);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_AllKeysAndValuesAreEqual()
    {
        var sut1 = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 1, 1.ToString() },
            { 2, 2.ToString() },
        });

        var sut2 = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 1, 1.ToString() },
            { 2, 2.ToString() },
        });

        // Act
        var equals = sut1.Equals(sut2);

        // Assert
        equals.Should().BeTrue();
    }

    [Test]
    public void Count_Should_Be2_When_2ElementsWithDifferentKeysAndValuesAdded()
    {
        // Act
        var sut = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 1, 1.ToString() },
            { 2, 2.ToString() },
        });

        // Assert
        sut.Count.Should().Be(2);
    }

    [Test]
    public void Indexer_get_Should_AddAnElements_When_KeyAndValue_DoNotExist()
    {
        // Arrange
        var sut = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 1, 1.ToString() },
            { 2, 2.ToString() },
            { 3, 3.ToString() },
        });


        // Act
        var value = sut[3];

        // Assert
        value.Should().Be(3.ToString());
    }

    [Test]
    public void TryGetKey_Should_ReturnTrue_When_ValueExists()
    {
        // Arrange
        var sut = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 1, 1.ToString() },
            { 2, 2.ToString() },
            { 3, 3.ToString() },
        });

        // Act
        var found = sut.TryGetKey(2.ToString(), out var key);

        // Assert
        found.Should().BeTrue();
        key.Should().Be(2);
    }

    [Test]
    public void TryGetValue_Should_ReturnTrue_When_KeyExists()
    {
        // Arrange
        var sut = new HashMapValue<int, string>(new Dictionary<int, string>
        {
            { 1, 1.ToString() },
            { 2, 2.ToString() },
            { 3, 3.ToString() },
        });

        // Act
        var found = sut.TryGetValue(2, out var value);

        // Assert
        found.Should().BeTrue();
        value.Should().Be(2.ToString());
    }
}
