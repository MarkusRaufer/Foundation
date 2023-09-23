using FluentAssertions;
using NUnit.Framework;

namespace Foundation.Collections.Generic;

[TestFixture]
public class SortedSetXTests
{
    [Test]
    public void Add_Should_Append9_When_9IsMaxValue()
    {
        var sut = new SortedSetX<int> { 1, 3, 7 };
        var count = sut.Count;

        sut.Add(9);

        sut.Count.Should().Be(count + 1);

        sut[0].Should().Be(1);
        sut[1].Should().Be(3);
        sut[2].Should().Be(7);
        sut[3].Should().Be(9);
    }

    [Test]
    public void Add_Should_NotInsertItemInTheMiddle_When_ItemExists()
    {
        var sut = new SortedSetX<int> { 1, 3, 5, 7 };
        var count = sut.Count;

        sut.Add(5);

        sut.Count.Should().Be(count);

        sut[0].Should().Be(1);
        sut[1].Should().Be(3);
        sut[2].Should().Be(5);
        sut[3].Should().Be(7);
    }

    [Test]
    public void Add_Should_Insert5AtIndex2_When_5NotExists()
    {
        var sut = new SortedSetX<int> { 1, 3, 7 };
        var count = sut.Count;

        sut.Add(5);

        sut.Count.Should().Be(count + 1);

        sut[0].Should().Be(1);
        sut[1].Should().Be(3);
        sut[2].Should().Be(5);
        sut[3].Should().Be(7);
    }

    [Test]
    public void Add_Should_InsertItemAtTail_When_ItemExists()
    {
        var sut = new SortedSetX<int> { 1, 3, 7, 9 };

        var count = sut.Count;

        sut.Add(9);

        sut.Count.Should().Be(count);

        sut[0].Should().Be(1);
        sut[1].Should().Be(3);
        sut[2].Should().Be(7);
        sut[3].Should().Be(9);
    }

    [Test]
    public void Add_Should_InsertItemOnIndex0_When_ValueIsMinValue()
    {
        var sut = new SortedSetX<int> { 2, 3, 7 };
        var count = sut.Count;

        sut.Add(1);

        sut.Count.Should().Be(count + 1);

        sut[0].Should().Be(1);
        sut[1].Should().Be(2);
        sut[2].Should().Be(3);
        sut[3].Should().Be(7);
    }

    [Test]
    public void Add_Should_InsertItemAtHead_When_ItemExists()
    {
        var sut = new SortedSetX<int> { 2, 3, 7 };
        var count = sut.Count;

        sut.Add(2);

        sut.Count.Should().Be(count);

        sut[0].Should().Be(2);
        sut[1].Should().Be(3);
        sut[2].Should().Be(7);
    }

    [Test]
    public void FindAll_Should_Return7And9_When_LambdaParameterIsGreater3()
    {
        var sut = new SortedSetX<int> { 1, 3, 7, 9 };

        var numbers = sut.FindAll((int x) => x > 3);

        numbers.Count.Should().Be(2);

        numbers[0].Should().Be(7);
        numbers[1].Should().Be(9);
    }

    [Test]
    public void FindAll_Should_Return1And7_When_LambdaParameterIs3Or7()
    {
        var sut = new SortedSetX<int> { 1, 3, 7, 9 };

        var numbers = sut.FindAll((int x) => x == 1 || x == 7);

        numbers.Count.Should().Be(2);

        numbers[0].Should().Be(1);
        numbers[1].Should().Be(7);
    }

    [Test]
    public void GetRange_Should_Return2Elements_When_CountIs2()
    {
        var sut = new SortedSetX<int> { 1, 3, 7, 9 };

        var count = 2;

        var range = sut.GetRange(1, 2);

        range.Count.Should().Be(count);

        range[0].Should().Be(3);
        range[1].Should().Be(7);
    }

    [Test]
    public void GetViewBetween_Should_Return3And7_When_LowerIs2AndUpperIs8()
    {
        var sut = new SortedSetX<int> { 1, 3, 7, 9 };

        var view = sut.GetViewBetween(2, 8);

        view.Count.Should().Be(2);

        view[0].Should().Be(3);
        view[1].Should().Be(7);
    }

    [Test]
    public void GetViewBetween_Should_Return3And7_When_LowerIs3AndUpperIs7()
    {
        var sut = new SortedSetX<int> { 1, 3, 7, 9 };

        var view = sut.GetViewBetween(3, 7);

        view.Count.Should().Be(2);

        view[0].Should().Be(3);
        view[1].Should().Be(7);
    }

    [Test]
    public void GetViewBetween_Should_Return1And3And7_When_LowerIsNegativeAndUpperIs7()
    {
        var sut = new SortedSetX<int> { 1, 3, 7, 9 };

        var view = sut.GetViewBetween(-1, 7);

        view.Count.Should().Be(3);

        view[0].Should().Be(1);
        view[1].Should().Be(3);
        view[2].Should().Be(7);
    }

    [Test]
    public void GetViewBetween_Should_Return7And9_When_LowerIs7AndUpperIs15()
    {
        var sut = new SortedSetX<int> { 1, 3, 7, 9 };

        var view = sut.GetViewBetween(7, 15);

        view.Count.Should().Be(2);

        view[0].Should().Be(7);
        view[1].Should().Be(9);
    }

    [Test]
    public void GetViewBetween_Should_ReturnAllValues_When_LowerIsSmallerThanMinAndUpperIsGreaterThanMax()
    {
        var sut = new SortedSetX<int> { 1, 3, 7, 9 };

        var view = sut.GetViewBetween(-15, 15);

        view.Count.Should().Be(4);

        view[0].Should().Be(1);
        view[1].Should().Be(3);
        view[2].Should().Be(7);
        view[3].Should().Be(9);
    }
}
