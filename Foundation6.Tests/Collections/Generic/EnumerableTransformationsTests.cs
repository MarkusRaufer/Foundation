using FluentAssertions;
using Foundation.ComponentModel;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Foundation.Collections.Generic;

[TestFixture]
public class EnumerableTransformationsTests
{
    [Test]
    public void SplitInto_Should_Return4StreamsWithSameNumbers_When_4PredicatesAreUsed()
    {
        var tuples = Enumerable.Range(1, 10).Select(x => (lhs: $"{x}", rhs: x)).ToArray();

        var streams = tuples.SplitInto(x => (object)x.lhs, x => (object)x.rhs).ToArray();

        streams.Length.Should().Be(2);
        {
            var stream = streams[0].OfType<string>();
            var expected = Enumerable.Range(1, 10).Select(x => $"{x}");
            stream.Should().ContainInOrder(expected);
        }
        {
            var stream = streams[1].OfType<int>();
            stream.Should().ContainInOrder(Enumerable.Range(1, 10));
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

        streams.Length.Should().Be(4);
        {
            var stream = streams[0];
            stream.Should().ContainInOrder(Enumerable.Range(1, 20).Where(x => x % 2 == 0));
        }
        {
            var stream = streams[1];
            stream.Should().ContainInOrder(new[] {5, 10, 15, 20 });
        }
        {
            var stream = streams[2];
            stream.Should().ContainInOrder(new[] { 3, 6, 9, 12, 15, 18 });
        }
        {
            var stream = streams[3];
            stream.Should().ContainInOrder(new[] { 10, 20 });
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

        streams.Length.Should().Be(3);
        {
            var stream = streams[0];
            stream.Should().ContainInOrder(Enumerable.Range(1, 20).Where(x => x % 2 == 0));
        }
        {
            var stream = streams[1];
            stream.Should().ContainInOrder(new[] { 5, 15, });
        }
        {
            var stream = streams[2];
            stream.Should().ContainInOrder(new[] { 3, 9 });
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

        streams.Length.Should().Be(4);
        {
            var stream = streams[0];
            stream.Should().ContainInOrder(Enumerable.Range(1, 20).Where(x => x % 2 == 0));
        }
        {
            var stream = streams[1];
            stream.Should().ContainInOrder(new[] { 5, 15, });
        }
        {
            var stream = streams[2];
            stream.Should().ContainInOrder(new[] { 3, 9 });
        }
        {
            var stream = streams[3];
            stream.Should().ContainInOrder(Array.Empty<int>());
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

            i1.Should().Be(1);
            i2.Should().Be(2);
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

            i1.Should().Be(3);
            i2.Should().Be(3);
            i3.Should().Be(6);
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

        i1.Should().Be(2);
        i2.Should().Be(3);
        i3.Should().Be(6);
    }


    [Test]
    public void ToDualStreams_Should_ReturnDualStreams_When_PredicateIsFizzBuzz_And_IsExhaustiveIsTrue()
    {
        var numbers = Enumerable.Range(1, 50);

        var re = new Regex("([0-9]+)");

        var all = numbers.ToDualStreams(n => 0 == n % 2, n => n, true)
                         .SelectLeft(_ => true, n => $"odd({n})")
                         .SelectRight(_ => true, n => $"even({n})")
                         .MergeAndSort(x => x, x => int.Parse(re.Match(x).Value))
                         .ToArray();

        foreach (var (counter, item) in all.Enumerate(seed: 1))
        {
            var expected = (0 == counter % 2) ? $"even({counter})" : $"odd({counter})";
            item.Should().Be(expected);
        }
    }

    [Test]
    public void ToDualOrdinalStreams_Should_ReturnDualOrdinalStreams_When_PredicateIsFizzBuzz_And_IsExhaustiveIsTrue()
    {
        var numbers = Enumerable.Range(1, 50);

        var fizzBuzz = "FizzBuzz";
        var fizz = "Fizz";
        var buzz = "Buzz";

        var all = numbers.ToDualOrdinalStreams(n => 0 == n % 15, _ => fizzBuzz, true)
                         .AddToRight(n => 0 == n % 3, _ => fizz, true)
                         .AddToRight(n => 0 == n % 5, _ => buzz, true)
                         .MergeStreams(n => n.ToString())
                         .ToArray();

        foreach (var (counter, item) in all.Enumerate(seed: 1))
        {
            if (0 == counter % 15)
            {
                fizzBuzz.Should().Be(item);
                continue;
            }
            if (0 == counter % 3)
            {
                fizz.Should().Be(item);
                continue;
            }

            if (0 == counter % 5)
            {
                buzz.Should().Be(item);
                continue;
            }

            item.Should().Be(counter.ToString());
        }
    }

}
