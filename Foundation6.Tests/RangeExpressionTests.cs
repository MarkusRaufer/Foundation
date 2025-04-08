using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Foundation;

[TestFixture]
public class RangeExpressionTests
{
    // ReSharper disable InconsistentNaming

    [Test]
    public void Between()
    {
        var sut = new Between<int>(3, 5);

        //in range
        sut.IsInRange(3).Should().BeTrue();
        sut.IsInRange(4).Should().BeTrue();
        sut.IsInRange(5).Should().BeTrue();

        //out of range
        sut.IsInRange(2).Should().BeFalse();
        sut.IsInRange(6).Should().BeFalse();
    }

    [Test]
    public void Exactly()
    {
        const int number = 3;
        var sut = new Exactly<int>(number);

        //in range
        sut.IsInRange(number).Should().BeTrue();
        sut.Value.Should().Be(number);

        //out of range
        sut.IsInRange(4).Should().BeFalse();
    }

    [Test]
    public void Generate_CallIsInRangeFirst()
    {
        var number = 1;
        var sut = new BufferdGenerator<int>(() => number++);

        //in range
        sut.IsInRange(1).Should().BeTrue();

        //out of range
        sut.IsInRange(0).Should().BeFalse();
        sut.IsInRange(2).Should().BeFalse();

        //compare values
        var expected = Enumerable.Range(1, 1);
        var values = sut.Values.ToList();
        values.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Generate_CallValuesFirst()
    {
        var number = 1;
        var sut = new BufferdGenerator<int>(() => number++);

        //compare values
        var expected = Enumerable.Range(1, 1);
        var values = sut.Values.ToList();
        values.Should().BeEquivalentTo(expected);

        //in range
        sut.IsInRange(1).Should().BeTrue();

        //out of range
        sut.IsInRange(0).Should().BeFalse();
        sut.IsInRange(2).Should().BeFalse();
    }

    [Test]
    public void Generate_CallValuesLazy()
    {
        var number = 1;
        int numberOfIncrementCalls = 0;

        int increment()
        {
            numberOfIncrementCalls++;
            number++;

            return number;
        }

        var quantity = 5;
        var sut = new BufferdGenerator<int>(increment, quantity);
       
        {
            var values = sut.Values.ToArray();

            values.Length.Should().Be(quantity);
            numberOfIncrementCalls.Should().Be(quantity);
        }
        {
            numberOfIncrementCalls = 0;
            var values = sut.Values.ToArray();

            values.Length.Should().Be(quantity);
            numberOfIncrementCalls.Should().Be(0);
        }
    }


    [Test]
    public void Generate_QuantityOfFive_CallIsInRangeFirst()
    {
        var number = 1;
        var quantity = 5;
        var sut = new BufferdGenerator<int>(() => number++, quantity);

        //in range
        sut.IsInRange(1).Should().BeTrue();
        sut.IsInRange(2).Should().BeTrue();
        sut.IsInRange(3).Should().BeTrue();
        sut.IsInRange(4).Should().BeTrue();
        sut.IsInRange(5).Should().BeTrue();

        //out of range
        sut.IsInRange(0).Should().BeFalse();
        sut.IsInRange(6).Should().BeFalse();

        //compare values
        var expected = Enumerable.Range(1, quantity);
        var values = sut.Values.ToList();

        values.Count.Should().Be(quantity);
    }

    [Test]
    public void Generate_QuantityOfFive_CallValuesFirst()
    {
        var number = 1;
        var sut = new BufferdGenerator<int>(() => number++, 5);

        //compare values
        var expected = Enumerable.Range(1, 5).ToList();

        //take(2) is just for inner test.
        var values = sut.Values.Take(2).ToList();
        values.Should().BeEquivalentTo(expected.Take(2));

        values = sut.Values.ToList();
        values.Should().BeEquivalentTo(expected);

        //in range
        sut.IsInRange(1).Should().BeTrue();
        sut.IsInRange(2).Should().BeTrue();
        sut.IsInRange(3).Should().BeTrue();
        sut.IsInRange(4).Should().BeTrue();
        sut.IsInRange(5).Should().BeTrue();

        //out of range
        sut.IsInRange(0).Should().BeFalse();
        sut.IsInRange(6).Should().BeFalse();
    }

    [Test]
    public void Matches()
    {
        var sut = new Matching<int>(n => n > 3);

        //in range
        sut.IsInRange(4).Should().BeTrue();
        sut.IsInRange(5).Should().BeTrue();

        //out of range
        sut.IsInRange(1).Should().BeFalse();
        sut.IsInRange(2).Should().BeFalse();
        sut.IsInRange(3).Should().BeFalse();
    }

    [Test]
    public void NumericBetween()
    {
        var sut = new NumericBetween<int>(3, 5);

        //in range
        sut.IsInRange(3).Should().BeTrue();
        sut.IsInRange(4).Should().BeTrue();
        sut.IsInRange(5).Should().BeTrue();

        //out of range
        sut.IsInRange(2).Should().BeFalse();
        sut.IsInRange(6).Should().BeFalse();

        //compare values
        var expected = new List<int> {3, 4, 5};
        var values = sut.Values.ToList();

        values.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void OneOf_InRange()
    {
        var expected = new List<int> {1, 3, 5};
        var sut = new OneOf<int>(expected.ToArray());
        
        //in range
        sut.IsInRange(1).Should().BeTrue();
        sut.IsInRange(3).Should().BeTrue();
        sut.IsInRange(5).Should().BeTrue();

        //out of range
        sut.IsInRange(2).Should().BeFalse();
        sut.IsInRange(4).Should().BeFalse();
        sut.IsInRange(6).Should().BeFalse();

        //compare values
        var values = sut.Values.ToList();

        values.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void OfType_InRange()
    {
        var sut = new OfType<int>();

        //in range
        sut.IsInRange(-1).Should().BeTrue();
        sut.Value.Should().Be(-1);

        sut.IsInRange(0).Should().BeTrue();
        sut.Value.Should().Be(0);

        sut.IsInRange(1).Should().BeTrue();
        sut.Value.Should().Be(1);

        //out of range
        sut.IsInRange(2.5).Should().BeFalse();
        sut.IsInRange("4").Should().BeFalse();
        sut.IsInRange('A').Should().BeFalse();
    }
    // ReSharper restore InconsistentNaming
}
