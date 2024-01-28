using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
