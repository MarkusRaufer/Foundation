using NUnit.Framework;

namespace Foundation.Collections.Generic;

[TestFixture]
public class EquatableListTests
{
    [Test]
    public void Equals_Should_ReturnFalse_When_CollectionsHaveDifferentSizes()
    {
        var sut1 = new EquatableList<int> { 1, 2, 3 };
        var sut2 = new EquatableList<int> { 1, 2, 3, 4 };
        Assert.False(sut1.Equals(sut2));
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_CollectionsHaveDifferentValues_SameSizes()
    {
        var sut1 = new EquatableList<int> { 1, 2, 3 };
        var sut2 = new EquatableList<int> { 1, 2, 4 };
        Assert.False(sut1.Equals(sut2));
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_CollectionsHaveSameValues_SameSizes_DifferentPositions()
    {
        var sut1 = new EquatableList<int> { 1, 2, 3 };
        var sut2 = new EquatableList<int> { 3, 1, 2 };
        Assert.False(sut1.Equals(sut2));
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_CollectionsHaveSameValues_SameSizes_SamePositions()
    {
        var sut1 = new EquatableList<int> { 1, 2, 3 };
        var sut2 = new EquatableList<int> { 1, 2, 3 };
        Assert.True(sut1.Equals(sut2));
    }

    [Test]
    public void GetHashCode_Should_ReturnDifferentHashCodes_When_CollectionsHaveDifferentSizes()
    {
        var sut1 = new EquatableList<int> { 1, 2, 3 };
        var sut2 = new EquatableList<int> { 1, 2, 3, 4 };

        Assert.False(sut1.GetHashCode() == sut2.GetHashCode());
    }

    [Test]
    public void GetHashCode_Should_ReturnDifferentHashCodes_When_CollectionsHaveDifferentValues_SameSizes()
    {
        var sut1 = new EquatableList<int> { 1, 2, 3 };
        var sut2 = new EquatableList<int> { 1, 2, 4 };

        Assert.False(sut1.GetHashCode() == sut2.GetHashCode());
    }

    [Test]
    public void GetHashCode_Should_ReturnDifferentHashCodes_When_CollectionsHaveSameValues_SameSizes_DifferentPositions()
    {
        var sut1 = new EquatableList<int> { 1, 2, 3 };
        var sut2 = new EquatableList<int> { 3, 1, 2 };

        Assert.False(sut1.GetHashCode() == sut2.GetHashCode());
    }

    [Test]
    public void GetHashCode_Should_ReturnSameHashCode_When_CollectionsHaveSameValues_SameSizes_SamePositions()
    {
        var sut1 = new EquatableList<int> { 1, 2, 3 };
        var sut2 = new EquatableList<int> { 1, 2, 3 };

        Assert.True(sut1.GetHashCode() == sut2.GetHashCode());
    }
}
