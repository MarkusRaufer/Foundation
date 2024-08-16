using FluentAssertions;
using NUnit.Framework;
using System;

namespace Foundation.Buffers
{
    [TestFixture]
    public class CharSplitEnumeratorTests
    {
        [Test]
        public void MoveNext_Should_Find_Separator_When_Calling_MoveNext()
        {
            var span = "123.4567.89".AsSpan();

            var sut = new CharSplitEnumerator(span, true, '.');

            sut.MoveNext().Should().BeTrue();
            sut.Current.ToString().Should().Be("123");

            sut.MoveNext().Should().BeTrue();
            sut.Current.ToString().Should().Be("4567");

            sut.MoveNext().Should().BeTrue();
            sut.Current.ToString().Should().Be("89");

            sut.MoveNext().Should().BeFalse();
        }

        [Test]
        public void MoveNext_Should_Find_Separator_When_Used_With_Foreach()
        {
            var span = "123.4567.89".AsSpan();
            var sut = new CharSplitEnumerator(span, true, '.');

            var i = 0;
            foreach (var entry in sut)
            {
                if (0 == i) entry.ToString().Should().Be("123");
                if (1 == i) entry.ToString().Should().Be("4567");
                if (2 == i) entry.ToString().Should().Be("89");
                i++;
            }
        }

        [Test]
        public void MoveNext_Should_Not_Find_Separator_When_Calling_MoveNext()
        {
            var span = "123.4567.89".AsSpan();
            var sut = new CharSplitEnumerator(span, true, '_');

            sut.MoveNext().Should().BeFalse();
        }

        [Test]
        public void MoveNext_Should_Not_Find_Separator_When_Used_With_Foreach()
        {
            var span = "123.4567.89".AsSpan();
            var sut = new CharSplitEnumerator(span, true, '_');

            var passed = false;
            foreach (var _ in sut)
            {
                passed = true;
            }

            passed.Should().BeFalse();
        }

        [Test]
        public void MoveNext_Should_Find_Separators_When_Calling_MoveNext()
        {
            var span = "123.456_78-9".AsSpan();

            var sut = new CharSplitEnumerator(span, true, '.', '_', '-');

            sut.MoveNext().Should().BeTrue();
            sut.Current.ToString().Should().Be("123");

            sut.MoveNext().Should().BeTrue();
            sut.Current.ToString().Should().Be("456");

            sut.MoveNext().Should().BeTrue();
            sut.Current.ToString().Should().Be("78");

            sut.MoveNext().Should().BeTrue();
            sut.Current.ToString().Should().Be("9");

            sut.MoveNext().Should().BeFalse();
        }

    }
}
