using FluentAssertions;
using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation;

[TestFixture]
public class RandomTests
{
    [Test]
    public void GetItem_Should_ReturnOneValue_When_ArrayHasValues()
    {
        var rnd = new Random(1);
        var numbers = Enumerable.Range(1, 5).ToArray();

        var randomSelected = rnd.GetItem(numbers);

        numbers.Should().Contain(randomSelected);
    }

    [Test]
    public void GetItem_Should_ThrowException_When_ArrayIsEmpty()
    {
        var rnd = new Random(1);

        Action act = () => rnd.GetItem(new int[0]);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void GetItems_Should_Return10Values_When_LengthIs10AndArrayHasSizeOf5()
    {
        var rnd = new Random(123);
        var max = 10;
        var numbers = Enumerable.Range(1, max).ToArray();

        var randomSelected = rnd.GetItems(numbers, max).ToArray();

        randomSelected.Length.Should().Be(max);
    }

    [Test]
    public void GetItems_Should_Return5Values_When_LengthIs5AndArrayHasSizeOf10()
    {
        var rnd = new Random(1);
        var count = 10;

        var numbers = Enumerable.Range(1, count).ToArray();

        var randomSelected = rnd.GetItems(numbers, count / 2).ToArray();

        randomSelected.Length.Should().Be(count / 2);
    }


    [Test]
    public void GetItemsLazy_Should_Return6Values_When_LeftIndexRandomlySelected6OutOf20()
    {
        // Arrange
        var sut = new Random(123);
        var left = 5;
        var right = 10;
        var max = 20;
        var numbers = Enumerable.Range(1, max).ToArray();

        // Act
        var randomSelected = sut.GetItemsLazy(numbers, left, right).ToArray();

        // Assert
        var length = right - left + 1;
        randomSelected.Length.Should().Be(length);
        randomSelected.All(numbers.Contains).Should().BeTrue();
    }

    [Test]
    public void IntegersWithoutDuplicates()
    {
        // Arrange
        var random = new Random();
        var min = 5;
        var max = 10;

        // Act
        var numbers = random.IntegersWithoutDuplicates(min, max).ToArray();

        // Assert
        var length = max - min + 1;
        numbers.Length.Should().Be(length);

        var expectedOrderedNumbers = Enumerable.Range(min, numbers.Length).ToArray(); // same numbers but ordered.
        expectedOrderedNumbers.All(x => numbers.Contains(x)).Should().BeTrue();
    }

    [Test]
    public void NextAlphaChar_Should_ReturnACharBetweenAandz_When_CalledOnce()
    {
        var random = new Random(1);

        for (var i = 0; i<100; i++)
        {
            var c = random.NextAlphaChar();
            var inRange = c >= 'A' && c <= 'z';
            inRange.Should().BeTrue();
        }
    }

    [Test]
    public void NextDateOnly_Should_ReturnDateOnlyWithinMinAndMax_When_Called()
    {
        var seed = 1;
        var random = new Random(seed);
        var min = new DateOnly(2020, 6, 15);
        var max = new DateOnly(2020, 7, 15);

        var dates = For.Collect(() => random.NextDateOnly(min, max)).Take(10).ToArray();

        dates.Should().OnlyContain(x => x >= min && x <= max);
    }

    [Test]
    public void NextDateTime_Should_ReturnTheSameDateTime_When_CalledOnce()
    {
        var seed = 1;
        var seeds = Enumerable.Repeat(seed, 10);

        var randoms = seeds.Select(x => new Random(x)).ToArray();

        var dateTimes = randoms.Select(rnd => rnd.NextDateTime(new (2022, 6, 3), new (2022, 6, 5)))
                               .ToArray();

        Assert.IsTrue(dateTimes.AllEqual());
    }

    [Test]
    public void NextDateTime_Should_ReturnTheSameDateTime_When_CalledMultipleTimes()
    {
        var maxSeed = 5;
        var quantity = 5;
        var seeds = Enumerable.Range(1, maxSeed);
        var minDateTime = new DateTime(2022, 6, 3);
        var maxDateTime = new DateTime(2022, 6, 25);


        foreach (var seed in seeds)
        {
            var random1 = new Random(seed);
            var random2 = new Random(seed);

            var dateTimes1 = For.Collect(() => random1.NextDateTime(minDateTime, maxDateTime))
                                .Take(quantity)
                                .ToArray();

            var dateTimes2 = For.Collect(() => random2.NextDateTime(minDateTime, maxDateTime))
                                .Take(quantity)
                                .ToArray();

            dateTimes1.Should().BeEquivalentTo(dateTimes2);
        }
    }

    [Test]
    public void NextGuid_Should_ReturnTheSameGuid_When_CalledOnce()
    {
        var seed = 1;
        var seeds = Enumerable.Repeat(seed, 10);
        var buffer = new byte[16];

        var randoms = seeds.Select(x => new Random(x)).ToArray();

        var guids = randoms.Select(rnd => rnd.NextGuid(buffer)).ToArray();

        guids.AllEqual().Should().BeTrue();
     }

    [Test]
    public void NextGuid_Should_ReturnTheSameGuid_When_CalledMultipleTimes()
    {
        var maxSeed = 5;
        var numberOfGuids = 5;
        var seeds = Enumerable.Range(1, maxSeed);
        var buffer = new byte[16];

        foreach (var seed in seeds)
        {
            var random1 = new Random(seed);
            var random2 = new Random(seed);
            
            var guids1 = For.Collect(() => random1.NextGuid()).Take(numberOfGuids).ToArray();
            var guids2 = For.Collect(() => random2.NextGuid()).Take(numberOfGuids).ToArray();

            guids1.Should().BeEquivalentTo(guids2);
        }
    }

    [Test]
    public void NextDateOnly_Should_ReturnTimeOnlyWithinMinAndMax_When_Called()
    {
        var seed = 1;
        var random = new Random(seed);
        var min = new TimeOnly(8, 0, 0);
        var max = new TimeOnly(10, 0, 0);

        var dates = For.Collect(() => random.NextTimeOnly(min, max)).Take(10).ToArray();

        dates.Should().OnlyContain(x => x >= min && x <= max);
    }
}
