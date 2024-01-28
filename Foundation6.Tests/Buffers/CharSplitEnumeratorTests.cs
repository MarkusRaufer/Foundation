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

            var sut = new CharSplitEnumerator(span, '.');

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
            var sut = new CharSplitEnumerator(span, '.');

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
            var sut = new CharSplitEnumerator(span, '_');

            sut.MoveNext().Should().BeFalse();
        }

        [Test]
        public void MoveNext_Should_Not_Find_Separator_When_Used_With_Foreach()
        {
            var span = "123.4567.89".AsSpan();
            var sut = new CharSplitEnumerator(span, '_');

            var passed = false;
            foreach (var _ in sut)
            {
                passed = true;
            }

            passed.Should().BeFalse();
        }
    }
}
