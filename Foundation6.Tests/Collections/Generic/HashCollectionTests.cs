using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation.Collections.Generic;

[TestFixture]
public class HashCollectionTests
{
    [Test]
    public void Contains_Should_ReturnFalse_When_NonUniqueValueNotExists()
    {
        HashCollection<int> sut = [1, 2, 3, 2, 4];

        sut.Contains(5).Should().BeFalse();
    }

    [Test]
    public void Contains_Should_ReturnTrue_When_UniqueValueNotExists()
    {
        HashCollection<int> sut = [1, 2, 3, 4];

        sut.Contains(5).Should().BeFalse();

    }
    [Test]
    public void Contains_Should_ReturnTrue_When_NonUniqueValueExists()
    {
        HashCollection<int> sut = [1, 2, 3, 2, 4];

        sut.Contains(2).Should().BeTrue();
    }

    [Test]
    public void Contains_Should_ReturnTrue_When_UniqueValueExists()
    {
        HashCollection<int> sut = [1, 2, 3, 4];

        sut.Contains(2).Should().BeTrue();
    }

    [Test]
    public void Ctor_Should_Have4Items_When_Assigning4UniqueValues()
    {
        HashCollection<int> sut = [1, 2, 3, 4];

        sut.Should().NotBeNull();
        sut.Count().Should().Be(4);

        var values = sut.ToArray();
        values.Contains(1).Should().BeTrue();
        values.Contains(2).Should().BeTrue();
        values.Contains(3).Should().BeTrue();
        values.Contains(4).Should().BeTrue();
    }

    [Test]
    public void Ctor_Should_Have5Items_When_Assigning5NonUniqueValues()
    {
        HashCollection<int> sut = [1, 2, 3, 2, 4];

        sut.Should().NotBeNull();
        sut.Count().Should().Be(5);

        var values = sut.ToArray();
        values.Contains(1).Should().BeTrue();
        values.Contains(2).Should().BeTrue();
        values.Contains(3).Should().BeTrue();
        values.Contains(4).Should().BeTrue();
    }

    [Test]
    public void Count_Should_Return2_When_Added2UniqueValues()
    {
        HashCollection<int> sut = [];
        sut.Add(1);
        sut.Add(2);

        sut.Count().Should().Be(2);
    }

    [Test]
    public void Count_Should_Return4_When_Removed2NonUniqueValues()
    {
        HashCollection<int> sut = [];
        sut.Add(1);
        sut.Add(2);
        sut.Add(3);
        sut.Add(2);
        sut.Add(4);
        sut.Add(3);

        sut.Remove(2);
        sut.Remove(3);

        sut.Count().Should().Be(4);
    }
    [Test]
    public void Count_Should_Return2_When_Removed2UniqueValues()
    {
        HashCollection<int> sut = [];
        sut.Add(1);
        sut.Add(2);
        sut.Add(3);
        sut.Add(4);
        sut.Remove(2);
        sut.Remove(3);

        sut.Count().Should().Be(2);
    }

    [Test]
    public void Count_Should_Return4_When_Added4NonUniqueValues()
    {
        HashCollection<int> sut = [];
        sut.Add(1);
        sut.Add(2);
        sut.Add(2);
        sut.Add(3);

        sut.Count().Should().Be(4);
    }

    [Test]
    public void Count_Should_Return4_When_Assigned4UniqueValues()
    {
        HashCollection<int> sut = [1, 2, 3, 4];

        sut.Count().Should().Be(4);
    }

    [Test]
    public void Count_Should_Return5_When_Assigned5NonUniqueValues()
    {
        HashCollection<int> sut = [1, 2, 3, 2, 4];

        sut.Count().Should().Be(5);
    }

    [Test]
    public void GetEnumerator_Should_Return4Values_When_Assigning4UniqueValues()
    {
        HashCollection<int> sut = [1, 2, 3, 4];

        var it = sut.GetEnumerator();

        it.MoveNext().Should().BeTrue();
        it.Current.Should().Be(1);

        it.MoveNext().Should().BeTrue();
        it.Current.Should().Be(2);

        it.MoveNext().Should().BeTrue();
        it.Current.Should().Be(3);

        it.MoveNext().Should().BeTrue();
        it.Current.Should().Be(4);

        it.MoveNext().Should().BeFalse();
    }

    [Test]
    public void GetEnumerator_Should_Return5Values_When_Assigning5NonUniqueValues()
    {
        HashCollection<int> sut = [1, 2, 3, 2, 4];

        var it = sut.GetEnumerator();

        it.MoveNext().Should().BeTrue();
        it.Current.Should().Be(1);

        it.MoveNext().Should().BeTrue();
        it.Current.Should().Be(2);

        it.MoveNext().Should().BeTrue();
        it.Current.Should().Be(2);

        it.MoveNext().Should().BeTrue();
        it.Current.Should().Be(3);

        it.MoveNext().Should().BeTrue();
        it.Current.Should().Be(4);

        it.MoveNext().Should().BeFalse();
    }

    [Test]
    public void GetValues_Should_Return2Values_When_2NonUniqueValuesExist()
    {
        HashCollection<int> sut = [1, 2, 3, 2, 4];

        var values = sut.GetValues(2).ToArray();
        values.Length.Should().Be(2);
        values[0].Should().Be(2);
        values[1].Should().Be(2);
    }

    [Test]
    public void TryGetValue_Should_ReturnValueWithTrue_When_NonUniqueValueExists()
    {
        HashCollection<int> sut = [1, 2, 3, 2, 4];

        sut.TryGetValue(2, out var value).Should().BeTrue();
        value.Should().Be(2);
    }

    [Test]
    public void TryGetValues_Should_Return1Item_When_Assigning4UniqueValues()
    {
        HashCollection<int> sut = [1, 2, 3, 4];

        sut.TryGetValues(2, out var values);

        var result = values.ToArray();
        result.Length.Should().Be(1);
        result[0].Should().Be(2);
    }

    [Test]
    public void TryGetValues_Should_Return2Items_When_Assigning4NonUniqueValues()
    {
        HashCollection<int> sut = [1, 2, 3, 2, 4];

        sut.TryGetValues(2, out var values);

        var result = values.ToArray();
        result.Length.Should().Be(2);
        result[0].Should().Be(2);
        result[1].Should().Be(2);
    }
}
