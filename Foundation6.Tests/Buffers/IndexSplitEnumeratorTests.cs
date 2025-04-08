using NUnit.Framework;
using Shouldly;
using System;

namespace Foundation.Buffers;

[TestFixture]
public class IndexSplitEnumeratorTests
{
    [Test]
    public void MoveNext_Should_Find_Separator_When_Calling_MoveNext()
    {
        var span = "123.4567.89".AsSpan();

        var sut = new IndexSplitEnumerator<char>(span, new[] {(0, 3), (4, 4), (9, 2)});

        sut.MoveNext().ShouldBeTrue();
        sut.Current.ToString().ShouldBeEquivalentTo("123");

        sut.MoveNext().ShouldBeTrue();
        sut.Current.ToString().ShouldBeEquivalentTo("4567");

        sut.MoveNext().ShouldBeTrue();
        sut.Current.ToString().ShouldBeEquivalentTo("89");

        sut.MoveNext().ShouldBeFalse();
    }
}
