using FluentAssertions;
using System.Collections.Immutable;

namespace Foundation.Collections.Immutable.Tests;

public class ImmutableSortedListTests
{
    [Fact]
    public void Add_Should_LetOriginalUnChanged_When_Added_2_Values()
    {
        var numbers = Enumerable.Range(1, 5).ToArray();
        var sut = new ImmutableSortedList<int>(numbers);

        var immutableList = sut.Add(7);

        sut.Should().BeEquivalentTo(numbers);

        var expected = numbers.Append(7).ToArray();
        immutableList.Should().BeEquivalentTo(expected);

        immutableList = immutableList.Add(9);

        sut.Should().BeEquivalentTo(numbers);

        expected = numbers.Append(7).Append(9).ToArray();
        immutableList.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Add_Should_LetOriginalUnChanged_When_Iterated()
    {
        var numbers = Enumerable.Range(1, 5).ToArray();
        var sut = new ImmutableSortedList<int>(numbers);

        IImmutableList<int>? immutableList = null;
        var i = 0;
        foreach(var number in sut)
        {
            if (i == 0) immutableList = sut.Add(7);
            if (i == 1) immutableList = immutableList?.Add(9);
            i++;
        }
        
        var expected = numbers.Append(7).Append(9).ToArray();
        immutableList.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void AddRange_Should_LetOriginalUnChanged_When_Added_2_Values()
    {
        var numbers = Enumerable.Range(1, 5).ToArray();
        var sut = new ImmutableSortedList<int>(numbers);

        var immutableList = sut.AddRange(new[] {7, 9});

        sut.Should().BeEquivalentTo(numbers);

        var expected = numbers.Append(7).Append(9).ToArray();
        immutableList.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Ctor_Should_HaveSortedElements_When_IsSorted_IsFalse()
    {
        var numbers = Enumerable.Range(1, 10).ToArray();
        var reverse = numbers.Reverse();
        var sut = new ImmutableSortedList<int>(reverse);

        sut.Should().BeEquivalentTo(numbers);
    }

    [Fact]
    public void Remove_Should_LetOriginalUnChanged_When_Removed_1_Value()
    {
        var numbers = Enumerable.Range(1, 5).ToArray();
        var sut = new ImmutableSortedList<int>(numbers);

        var immutableList = sut.Remove(3);

        sut.Should().BeEquivalentTo(numbers);

        var expected = numbers.Ignore(x => x == 3).ToArray();
        immutableList.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void RemoveRange_Should_LetOriginalUnChanged_When_Removed_1_Value()
    {
        var numbers = Enumerable.Range(1, 5).ToArray();
        var sut = new ImmutableSortedList<int>(numbers);

        var immutableList = sut.RemoveRange(2, 2);

        sut.Should().BeEquivalentTo(numbers);

        var expected = numbers.Ignore(x => x is 3 or 4).ToArray();
        immutableList.Should().BeEquivalentTo(expected);
    }
}