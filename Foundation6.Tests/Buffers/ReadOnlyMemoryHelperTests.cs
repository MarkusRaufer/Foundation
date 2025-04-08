using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;

namespace Foundation.Buffers
{
    [TestFixture]
    public class ReadOnlyMemoryHelperTests
    {
        [Test]
        public void ToString_Should_Return_Character_Separated_String_When_Separator_Is_A_Character()
        {
            var items = Enumerable.Range(1, 5).Select(x => x.ToString().AsMemory());
            var str = ReadOnlyMemoryHelper.ToString(items, ',');

            const string expected = "1,2,3,4,5";
            expected.ShouldBeEquivalentTo(str);
        }

        [Test]
        public void ToString_Should_Return_String_Separated_String_When_Separator_Is_A_String()
        {
            var items = Enumerable.Range(1, 5).Select(x => x.ToString().AsMemory());
            var str = ReadOnlyMemoryHelper.ToString(items, ", ".AsMemory());

            const string expected = "1, 2, 3, 4, 5";
            expected.ShouldBeEquivalentTo(str);
        }
    }
}
