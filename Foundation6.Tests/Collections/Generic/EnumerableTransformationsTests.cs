using Foundation.ComponentModel;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Foundation.Collections.Generic;

[TestFixture]
public class EnumerableTransformationsTests
{
    private record Entity(Id Id, string Type);
    private record Entities(Entity[] Objects);
    private record EntityIds(Id[] Ids);

    [Test]
    public void SplitInto_Should_Return4StreamsWithSameNumbers_When_4PredicatesAreUsed()
    {
        var tuples = Enumerable.Range(1, 10).Select(x => (lhs: $"{x}", rhs: x)).ToArray();

        var streams = tuples.SplitInto(x => (object)x.lhs, x => (object)x.rhs).ToArray();

        streams.Length.ShouldBe(2);
        {
            var stream = streams[0].ObjectOfType<string>();
            var expected = Enumerable.Range(1, 10).Select(x => $"{x}");
            stream.SequenceEqual(expected).ShouldBeTrue();
        }
        {
            var stream = streams[1].ObjectOfType<int>();
            stream.SequenceEqual(Enumerable.Range(1, 10)).ShouldBeTrue();
        }
    }

    [Test]
    public void SplitIntoStreams_Should_Return4StreamsWithSameNumbers_When_4PredicatesAreUsed()
    {
        var numbers = Enumerable.Range(1, 20);

        var streams = numbers.SplitIntoStreams(new Func<int, bool>[]
        {
            x => x % 2 == 0,
            x => x % 5 == 0,
            x => x % 3 == 0,
            x => x % 10 == 0
        }).ToArray();

        streams.Length.ShouldBe(4);
        {
            var stream = streams[0];
            stream.SequenceEqual(Enumerable.Range(1, 20).Where(x => x % 2 == 0)).ShouldBeTrue();
        }
        {
            var stream = streams[1];
            stream.SequenceEqual(new[] {5, 10, 15, 20 }).ShouldBeTrue();
        }
        {
            var stream = streams[2];
            stream.SequenceEqual(new[] { 3, 6, 9, 12, 15, 18 }).ShouldBeTrue();
        }
        {
            var stream = streams[3];
            stream.SequenceEqual(new[] { 10, 20 }).ShouldBeTrue();
        }
    }

    [Test]
    public void SplitIntoStreams_Should_Return3StreamsWithDifferentNumbers_When_4PredicatesAreUsed_AllowSameElementsIsFalse_RemoveEmptyStreamsIsTrue()
    {
        var numbers = Enumerable.Range(1, 20);

        var streams = numbers.SplitIntoStreams(new Func<int, bool>[]
        {
            x => x % 2 == 0,
            x => x % 5 == 0,
            x => x % 3 == 0,
            x => x % 10 == 0
        },
        allowSameElements: false,
        removeEmptyStreams: true).ToArray();

        streams.Length.ShouldBe(3);
        {
            var stream = streams[0];
            stream.SequenceEqual(Enumerable.Range(1, 20).Where(x => x % 2 == 0)).ShouldBeTrue();
        }
        {
            var stream = streams[1];
            stream.SequenceEqual(new[] { 5, 15 }).ShouldBeTrue();
        }
        {
            var stream = streams[2];
            stream.SequenceEqual(new[] { 3, 9 }).ShouldBeTrue();
        }
    }

    [Test]
    public void SplitIntoStreams_Should_Return4StreamsWithDifferentNumbers_When_4PredicatesAreUsed_AllowSameElementsIsFalse()
    {
        var numbers = Enumerable.Range(1, 20);

        var streams = numbers.SplitIntoStreams(new Func<int, bool>[]
        {
            x => x % 2 == 0,
            x => x % 5 == 0,
            x => x % 3 == 0,
            x => x % 10 == 0
        }, allowSameElements: false).ToArray();

        streams.Length.ShouldBe(4);
        {
            var stream = streams[0];
            stream.SequenceEqual(Enumerable.Range(1, 20).Where(x => x % 2 == 0)).ShouldBeTrue();
        }
        {
            var stream = streams[1];
            stream.SequenceEqual(new[] { 5, 15 }).ShouldBeTrue();
        }
        {
            var stream = streams[2];
            stream.SequenceEqual(new[] { 3, 9 }).ShouldBeTrue();
        }
        {
            var stream = streams[3];
            stream.SequenceEqual(Array.Empty<int>()).ShouldBeTrue();
        }
    }

    [Test]
    public void ToBreakable()
    {
        {
            var items1 = Enumerable.Range(1, 3);
            var items2 = Enumerable.Range(1, 3);

            var i1 = 0;
            var i2 = 0;
            var stop = ObservableValue.New(false);
            foreach (var item1 in items1.ToBreakable(ref stop))
            {
                i1++;
                foreach (var item2 in items2.ToBreakable(ref stop))
                {
                    i2++;

                    if (i2 == 2) stop.Value = true;
                }
            }

            i1.ShouldBe(1);
            i2.ShouldBe(2);
        }

        {
            var items1 = Enumerable.Range(1, 3);
            var items2 = Enumerable.Range(1, 3);
            var items3 = Enumerable.Range(1, 3);

            var i1 = 0;
            var i2 = 0;
            var i3 = 0;

            foreach (var item1 in items1)
            {
                var stop = ObservableValue.New(false);

                i1++;
                foreach (var item2 in items2.ToBreakable(ref stop))
                {
                    i2++;
                    foreach (var item3 in items3.ToBreakable(ref stop))
                    {
                        i3++;
                        if (item3 == 2) stop.Value = true;
                    }
                }
            }

            i1.ShouldBe(3);
            i2.ShouldBe(3);
            i3.ShouldBe(6);
        }
    }

    [Test]
    public void ToBreakable_Cascaded()
    {
        var items1 = Enumerable.Range(0, 3);
        var items2 = Enumerable.Range(0, 3);
        var items3 = Enumerable.Range(0, 3);

        var i1 = 0;
        var i2 = 0;
        var i3 = 0;
        var stop = ObservableValue.New(false);
        var stopAll = ObservableValue.New(false);
        foreach (var item1 in items1.ToBreakable(ref stopAll))
        {
            i1++;
            foreach (var item2 in items2.ToBreakable(ref stop)
                                        .ToBreakable(ref stopAll))
            {
                i2++;
                foreach (var item3 in items3.ToBreakable(ref stop)
                                            .ToBreakable(ref stopAll))
                {
                    i3++;

                    if (item1 == 0 && item3 == 1)
                        stop.Value = true;

                    if (item2 == 1)
                        stopAll.Value = true;
                }
            }
        }

        i1.ShouldBe(2);
        i2.ShouldBe(3);
        i3.ShouldBe(6);
    }

    [Test]
    public void ToDictionaryValue_Should_ReturnDictionaryValue_When_UsedEnumerableWithKeyValuePairsAndNoSelector()
    {
        // Arrange
        var numberOfItems = 3;
        var items = Enumerable.Range(1, numberOfItems).Select(x => new KeyValuePair<int, string>(x, x.ToString()));

        // Act
        var dictionary = items.ToDictionaryValue();

        // Assert
        dictionary.Count.ShouldBe(numberOfItems);
        dictionary[1].ShouldBe("1");
        dictionary[2].ShouldBe("2");
        dictionary[3].ShouldBe("3");
    }

    [Test]
    public void ToDictionaryValue_Should_ReturnDictionaryValueWithTransformedValues_When_SelectorIsUsed()
    {
        var items = new[]
        {
            (Key: "one",   Value: 1),
            (Key: "two",   Value: 2),
            (Key: "three", Value: 3),
        };
        var dictionary = items.ToDictionaryValue(x => x.Key, x => x.Value * 10);
        dictionary.Count.ShouldBe(3);
        dictionary["one"].ShouldBe(10);
        dictionary["two"].ShouldBe(20);
        dictionary["three"].ShouldBe(30);
    }

    [Test]
    public void ToDualStreams_Should_ReturnDualStreams_When_PredicateIsFizzBuzz_And_IsExhaustiveIsTrue()
    {
        var numbers = Enumerable.Range(1, 50);

        var re = new Regex("([0-9]+)");

        var all = numbers.ToDualStreams(n => 0 == n % 2, n => n)
                         .SelectLeft(_ => true, n => $"odd({n})")
                         .SelectRight(_ => true, n => $"even({n})")
                         .MergeAndSort(x => x, x => int.Parse(re.Match(x).Value))
                         .ToArray();

        foreach (var (counter, item) in all.Enumerate(seed: 1))
        {
            var expected = (0 == counter % 2) ? $"even({counter})" : $"odd({counter})";
            item.ShouldBe(expected);
        }
    }

    [Test]
    public void ToDualOrdinalStreams_Should_ReturnDualOrdinalStreams_When_PredicateIsFizzBuzz_And_IsExhaustiveIsTrue()
    {
        var numbers = Enumerable.Range(1, 50);

        var fizzBuzz = "FizzBuzz";
        var fizz = "Fizz";
        var buzz = "Buzz";

        var all = numbers.ToDualOrdinalStreams(n => 0 == n % 15, _ => fizzBuzz)
                         .AddToRight(n => 0 == n % 3, _ => fizz)
                         .AddToRight(n => 0 == n % 5, _ => buzz)
                         .MergeStreams(n => n.ToString())
                         .ToArray();

        foreach (var (counter, item) in all.Enumerate(seed: 1))
        {
            if (0 == counter % 15)
            {
                fizzBuzz.ShouldBe(item);
                continue;
            }
            if (0 == counter % 3)
            {
                fizz.ShouldBe(item);
                continue;
            }

            if (0 == counter % 5)
            {
                buzz.ShouldBe(item);
                continue;
            }

            item.ShouldBe(counter.ToString());
        }
    }

    [Test]
    public void ToDualStreams_LeftToRightMany_Should_ReturnDualStreams_Left0Right6ELems_When_LeftToRightMany_UsedWithPredicate()
    {
        var entity = new Entity(Id.New(1), "1");
        var entities = Enumerable.Range(2, 3).Select(x => new Entity(Id.New(x), x.ToString())).ToArray();
        var entityIds = Enumerable.Range(5, 2).Select(x => Id.New(x)).ToArray();
        object[] objects = [entity, new Entities(entities), new EntityIds(entityIds)];

        var streams = objects.ToDualStreams(x => x is Entity, x => ((Entity)x).Id)
                             .LeftToRightMany(x => x is Entities, x => ((Entities)x).Objects.Select(x => x.Id))
                             .LeftToRightMany(x => x is EntityIds, x => ((EntityIds)x).Ids);

        var left  = streams.Left.ToArray();
        left.Length.ShouldBe(0);

        var right = streams.Right.ToArray();
        var expected = Enumerable.Range(1, 6).Select(Id.New);
        right.ShouldBe(expected);
    }

    [Test]
    public void ToDualStreams_LeftToRightMany_Should_ReturnDualStreams_Left0Right6ELems_When_LeftToRightMany_UsedWithGenericTypeArgument()
    {
        var entity = new Entity(Id.New(1), "1");
        var entities = Enumerable.Range(2, 3).Select(x => new Entity(Id.New(x), x.ToString())).ToArray();
        var entityIds = Enumerable.Range(5, 2).Select(x => Id.New(x)).ToArray();
        object[] objects = [entity, new Entities(entities), new EntityIds(entityIds)];

        var streams = objects.ToDualStreams<object, Entity, Id>(x => x.Id)
                             .LeftToRightMany<object, Entities, Id>(x => x.Objects.Select(x => x.Id))
                             .LeftToRightMany<object, EntityIds, Id>(x => x.Ids);

        var left = streams.Left.ToArray();
        left.Length.ShouldBe(0);

        var right = streams.Right.ToArray();
        var expected = Enumerable.Range(1, 6).Select(Id.New);
        right.ShouldBe(expected);
    }

    [Test]
    public void ToDualStreams_LeftToRightMany_Should_ReturnDualStreams_Left0Right6ELems_When_ToDualStreams_CopiesAllElementsToLeft()
    {
        var entity = new Entity(Id.New(1), "1");
        var entities = Enumerable.Range(2, 3).Select(x => new Entity(Id.New(x), x.ToString())).ToArray();
        var entityIds = Enumerable.Range(5, 2).Select(x => Id.New(x)).ToArray();
        object[] objects = [entity, new Entities(entities), new EntityIds(entityIds)];

        var streams = objects.ToDualStreams<object, Id>()
                             .LeftToRight<object, Entity, Id>(x => x.Id)
                             .LeftToRightMany<object, Entities, Id>(x => x.Objects.Select(x => x.Id))
                             .LeftToRightMany<object, EntityIds, Id>(x => x.Ids);

        var left = streams.Left.ToArray();
        left.Length.ShouldBe(0);

        var right = streams.Right.ToArray();
        var expected = Enumerable.Range(1, 6).Select(Id.New);
        right.ShouldBe(expected);
    }

    [Test]
    public void ZipAll_ShouldReturn8Tuples_When_SidesHaveSameAndDifferentValues()
    {
        int[] left = [1, 2, 3, 4, 5, 7];
        int[] right = [2, 4, 6, 8];

        var zipped = left.ZipAll(right, x => x, x => x).ToArray();

    }
}
