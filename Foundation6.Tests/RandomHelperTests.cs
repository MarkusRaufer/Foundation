﻿using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation;

[TestFixture]
public class RandomHelperTests
{

    [Test]
    public void GetRandomOrdinalGuid_ShouldReturnSameValue_When_UsingSameIndex()
    {
        var guid1 = RandomHelper.GetRandomOrdinalGuid(5);
        var guid2 = RandomHelper.GetRandomOrdinalGuid(5);

        Assert.AreEqual(guid1, guid2);
    }

    [Test]
    public void GetRandomOrdinalGuid_ShouldReturnSameValues_When_UsingSameIndices()
    {
        var guids1 = RandomHelper.GetRandomOrdinalGuids(new[] { 3, 5, 8 }).ToArray();
        var guids2 = RandomHelper.GetRandomOrdinalGuids(new[] { 3, 5, 8 }).ToArray();

        CollectionAssert.AreEqual(guids1, guids2);
    }

    [Test]
    public void GetRandomOrdinalValues_ShouldReturnSameDateTimeValues_When_UsingSameIndices()
    {
        var indices = new int[] { 1, 5, 10 };

        var min = new DateTime(2022, 6,  1);
        var max = new DateTime(2022, 6, 10);

        var dateTimes1 = RandomHelper.GetRandomOrdinalDateTimes(indices, min, max).ToArray();
        var dateTimes2 = RandomHelper.GetRandomOrdinalDateTimes(indices, min, max).ToArray();

        CollectionAssert.AreEqual(dateTimes1, dateTimes2);
    }

    [Test]
    public void GetRandomOrdinalValues_ShouldReturnSameDoubleValues_When_UsingSameIndices()
    {
        var numbers1 = RandomHelper.GetRandomOrdinalDoubles(new int[] { 1, 5, 10 }, 0D, 1.5).ToArray();
        var numbers2 = RandomHelper.GetRandomOrdinalDoubles(new int[] { 1, 5, 10 }, 0D, 1.5).ToArray();

        CollectionAssert.AreEqual(numbers1, numbers2);
    }

    [Test]
    public void GetRandomOrdinalValues_ShouldReturnSameInt32Values_When_UsingSameIndices()
    {
        var numbers1 = RandomHelper.GetRandomOrdinalInts(new int[] { 1, 5, 10 }, 0, 10).ToArray();
        var numbers2 = RandomHelper.GetRandomOrdinalInts(new int[] { 1, 5, 10 }, 0, 10).ToArray();

        CollectionAssert.AreEqual(numbers1, numbers2);
    }

    [Test]
    public void GetRandomOrdinalValues_ShouldReturnSameInt64Values_When_UsingSameIndices()
    {
        var numbers1 = RandomHelper.GetRandomOrdinalLongs(new int[] { 1, 5, 10 }, 0L, 10L).ToArray();
        var numbers2 = RandomHelper.GetRandomOrdinalLongs(new int[] { 1, 5, 10 }, 0L, 10L).ToArray();

        CollectionAssert.AreEqual(numbers1, numbers2);
    }

    [Test]
    public void GetRandomValues_ShouldReturnDifferntDoubleValues_When_UsingDefaultSeed()
    {
        var deviation = .1;

        var numbers1 = RandomHelper.GetRandomDoubles(new double[] { .5, 1.0, 1.5 }, deviation).Take(10).ToArray();
        var numbers2 = RandomHelper.GetRandomDoubles(new double[] { .5, 1.0, 1.5 }, deviation).Take(10).ToArray();

        CollectionAssert.AreNotEqual(numbers1, numbers2);
    }

    [Test]
    public void GetRandomValues_ShouldReturnSameDoubleValues_When_UsingSameSeed()
    {
        var seed = 1;
        var deviation = .1;

        var numbers1 = RandomHelper.GetRandomDoubles(new double[] { .5, 1.0, 1.5 }, deviation, seed).Take(10).ToArray();
        var numbers2 = RandomHelper.GetRandomDoubles(new double[] { .5, 1.0, 1.5 }, deviation, seed).Take(10).ToArray();

        CollectionAssert.AreEqual(numbers1, numbers2);
    }

    [Test]
    public void GetRandomValues_ShouldReturnDifferentInt32Values_When_UsingDefaultSeed()
    {
        var numbers1 = RandomHelper.GetRandomInts(new int[] { 3, 6, 9 }).Take(10).ToArray();
        var numbers2 = RandomHelper.GetRandomInts(new int[] { 3, 6, 9 }).Take(10).ToArray();

        CollectionAssert.AreNotEqual(numbers1, numbers2);
    }

    [Test]
    public void GetRandomValues_ShouldReturnSameInt32Values_When_UsingSameSeed()
    {
        var seed = 0;

        var numbers1 = RandomHelper.GetRandomInts(new int[] { 3, 6, 9 }, seed).Take(10).ToArray();
        var numbers2 = RandomHelper.GetRandomInts(new int[] { 3, 6, 9 }, seed).Take(10).ToArray();

        CollectionAssert.AreEqual(numbers1, numbers2);
    }

    [Test]
    public void GetRandomValues_ShouldReturnDifferentInt64Values_When_UsingDefaultSeed()
    {
        var numbers1 = RandomHelper.GetRandomLong(new long[] { 3, 6, 9 }).Take(10).ToArray();
        var numbers2 = RandomHelper.GetRandomLong(new long[] { 3, 6, 9 }).Take(10).ToArray();

        CollectionAssert.AreNotEqual(numbers1, numbers2);
    }

    [Test]
    public void GetRandomValues_ShouldReturnSameInt64Values_When_UsingSameSeed()
    {
        var seed = 0;

        var numbers1 = RandomHelper.GetRandomLong(new long[] { 3, 6, 9 }, seed).Take(10).ToArray();
        var numbers2 = RandomHelper.GetRandomLong(new long[] { 3, 6, 9 }, seed).Take(10).ToArray();

        CollectionAssert.AreEqual(numbers1, numbers2);
    }
}
