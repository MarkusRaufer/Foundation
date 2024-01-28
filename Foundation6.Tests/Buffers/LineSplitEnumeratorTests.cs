using FluentAssertions;
using NUnit.Framework;
using System;

namespace Foundation.Buffers
{
    [TestFixture]
    public class LineSplitEnumeratorTests
    {
        [Test]
        public void MoveNext_Should_Return4Lines_When_StringIncludes3Linebreaks()
        {
            var str = "this\nis\na\ntest";
            var sut = new LineSplitEnumerator(str.AsSpan());
            var enumerator = sut.GetEnumerator();
            {
                enumerator.MoveNext().Should().BeTrue();
                var line = enumerator.Current;
                "this".AsSpan().IsSameAs(line).Should().BeTrue();
            }
            {
                enumerator.MoveNext().Should().BeTrue();
                var line = enumerator.Current;
                "is".AsSpan().IsSameAs(line).Should().BeTrue();
            }
            {
                enumerator.MoveNext().Should().BeTrue();
                var line = enumerator.Current;
                "a".AsSpan().IsSameAs(line).Should().BeTrue();
            }
            {
                enumerator.MoveNext().Should().BeTrue();
                var line = enumerator.Current;
                "test".AsSpan().IsSameAs(line).Should().BeTrue();
            }
            enumerator.MoveNext().Should().BeFalse();
        }
    }
}
