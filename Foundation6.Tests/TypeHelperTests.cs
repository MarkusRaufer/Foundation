using NUnit.Framework;
using Shouldly;
using System;

namespace Foundation;

[TestFixture]
public class TypeHelperTests
{
    [Test]
    public void AreAllDifferent_Should_ReturnFalse_When_CollectionContainsSameType()
    {
        // Arrange
        Type[] types = [typeof(string), typeof(int), typeof(int)];

        // Act
        var areDifferent = TypeHelper.AreAllDifferent(types);

        // Assert
        areDifferent.ShouldBeFalse();
    }

    [Test]
    public void AreAllDifferent_Should_ReturnTrue_When_CollectionContainsNoSameType()
    {
        // Arrange
        Type[] types = [typeof(string), typeof(int), typeof(DateTime)];

        // Act
        var areDifferent = TypeHelper.AreAllDifferent(types);

        // Assert
        areDifferent.ShouldBeTrue();
    }

    [Test]
    public void AreAllEqual_Should_ReturnFalse_When_CollectionContainsDifferentType()
    {
        // Arrange
        Type[] types = [typeof(string), typeof(int), typeof(int)];

        // Act
        var areEqual = TypeHelper.AreAllEqual(types);

        // Assert
        areEqual.ShouldBeFalse();
    }

    [Test]
    public void AreAllEqual_Should_ReturnTrue_When_CollectionContainsOnlySameTypes()
    {
        // Arrange
        Type[] types = [typeof(int), typeof(int), typeof(int)];

        // Act
        var areEqual = TypeHelper.AreAllEqual(types);

        // Assert
        areEqual.ShouldBeTrue();
    }

    [Test]
    public void GetPrimitiveType_Should_ReturnAValidType_When_UsingValidShortName()
    {
        var expected = typeof(int);
        
        var actual = TypeHelper.GetPrimitveType("int");
        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetScalarType_Should_ReturnAValidType_When_UsingValidShortName()
    {
        {
            var expected = typeof(string);
            var actual = TypeHelper.GetScalarType("string");
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}
