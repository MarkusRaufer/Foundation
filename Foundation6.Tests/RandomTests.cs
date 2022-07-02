using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation;

[TestFixture]
public class RandomTests
{
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

            CollectionAssert.AreEqual(dateTimes1, dateTimes2);
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

        Assert.IsTrue(guids.AllEqual());
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

            CollectionAssert.AreEqual(guids1, guids2);
        }
    }
}
