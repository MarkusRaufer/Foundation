//using NUnit.Framework;
//using System;
//using System.Buffers;
//using System.Linq;

//namespace Foundation.Buffers
//{
//    [TestFixture]
//    public class ReadOnlyMemoryExtensionsTests
//    {
//        [Test]
//        public void IndexesOf_Should_Return_Indexes_When_Selector_Matches()
//        {
//            var numbers = "A,B,C,D".AsMemory().Split(',').ToArray();
//            var indexes = numbers.IndexesOf("B".AsMemory(), "D".AsMemory()).ToArray();
//            Assert.AreEqual(2, indexes.Length);
//            Assert.AreEqual(1, indexes[0]);
//            Assert.AreEqual(3, indexes[1]);
//        }

//        [Test]
//        public void SequenceEqual_Should_Return_False_When_Sequences_Are_Not_Equal()
//        {
//            var a = new[] { "A", "B", "C" }.Select(x => x.AsMemory());
//            var b = new[] { "A", "B", "B" }.Select(x => x.AsMemory());

//            Assert.IsFalse(a.SequenceEqual(b));
//        }

//        [Test]
//        public void SequenceEqual_Should_Return_True_When_Sequences_Are_Equal()
//        {
//            var a = new[] { "A", "B", "C" }.Select(x => x.AsMemory());
//            var b = new[] { "A", "B", "C" }.Select(x => x.AsMemory());
//            Assert.IsTrue(a.SequenceEqual(b));
//        }

//        [Test]
//        public void Split_Should_Return_A_List_Of_ReadOnlyMemory_When_Using_Value_As_Separator()
//        {
//            var sut = "123.4567.89".AsMemory();

//            var splitted = sut.Split('.').ToArray();
//            Assert.AreEqual("123", splitted[0].Span.ToString());
//            Assert.AreEqual("4567", splitted[1].Span.ToString());
//            Assert.AreEqual("89", splitted[2].Span.ToString());
//        }

//        [Test]
//        public void Split_Should_Return_A_List_Of_ReadOnlyMemory_When_Using_Values_As_Separator()
//        {
//            var sut = "123_4567.89".AsMemory();
//            var splitted = sut.Split('.', '_').ToArray();
//            Assert.AreEqual("123", splitted[0].Span.ToString());
//            Assert.AreEqual("4567", splitted[1].Span.ToString());
//            Assert.AreEqual("89", splitted[2].Span.ToString());
//        }
//    }
//}
