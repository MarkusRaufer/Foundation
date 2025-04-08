using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Foundation.Collections.Generic;

[TestFixture]
public class ImmutableKeysDictionaryTests
{
    [Test]
    public void Count_Should_Return3_When_Added3Items()
    {
        // Arrange
        var keyValues = new Dictionary<string, object?>
        {
            ["one"] =  1,
            ["two"] =  2,
            ["three"]= 3,
        };

        // Act
        var sut = new ImmutableKeysDictionary<string, object?>(keyValues);

        // Assert
        sut.Count.ShouldBe(keyValues.Count);
    }

    [Test]
    public void Indexer_Get_Should_ReturnTwo_When_KeyExists()
    {
        // Arrange
        var expectedValue = 2;
        var keyValues = new Dictionary<string, object?>
        {
            ["one"] = 1,
            ["two"] = expectedValue,
            ["three"] = 3,
        };
        var sut = new ImmutableKeysDictionary<string, object?>(keyValues);

        // Act
        var value = sut["two"];

        // Assert
        value.ShouldBe(expectedValue);
    }

    [Test]
    public void Indexer_Get_Should_ThrowException_When_KeyDoesNotExist()
    {
        // Arrange
        var expectedValue = 2;
        var keyValues = new Dictionary<string, object?>
        {
            ["one"] = 1,
            ["two"] = expectedValue,
            ["three"] = 3,
        };
        var sut = new ImmutableKeysDictionary<string, object?>(keyValues);

        var key = "four";

        // Act
        var exception = Should.Throw<KeyNotFoundException>(() => sut[key]);

        // Assert
        exception.ShouldNotBeNull();
        exception.Message.ShouldBe($"The given key '{key}' was not present in the dictionary.");
    }

    [Test]
    public void Indexer_Set_Should_ChangeNothing_When_KeyDoesNotExist()
    {
        // Arrange
        var expectedValue = 2;
        var keyValues = new Dictionary<string, object?>
        {
            ["one"] = 1,
            ["two"] = expectedValue,
            ["three"] = 3,
        };
        var sut = new ImmutableKeysDictionary<string, object?>(keyValues);

        // Act
        sut["four"] = 4;

        // Assert
        sut.IsDirty.ShouldBeFalse();
    }

    [Test]
    public void Indexer_Set_Should_ChangeValue_When_KeyExists()
    {
        // Arrange
        var keyValues = new Dictionary<string, object?>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
        };
        var sut = new ImmutableKeysDictionary<string, object?>(keyValues);

        // Act
        var expectedValue = 22;

        sut["two"] = expectedValue;

        // Assert
        sut.IsDirty.ShouldBeTrue();
        var value = sut["two"];
        value.ShouldBe(expectedValue);
    }

    [Test]
    public void IsDirty_Should_ReturnFalse_When_CollectionWasInitialized()
    {
        // Arrange
        var expectedValue = 2;
        var keyValues = new Dictionary<string, object?>
        {
            ["one"] = 1,
            ["two"] = expectedValue,
            ["three"] = 3,
        };

        // Act
        var sut = new ImmutableKeysDictionary<string, object?>(keyValues);

        // Assert
        sut.IsDirty.ShouldBeFalse();
    }

}
