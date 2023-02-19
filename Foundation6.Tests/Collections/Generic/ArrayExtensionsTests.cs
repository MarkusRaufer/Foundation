using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation.Collections.Generic
{
    [TestFixture]
    public class ArrayExtensionsTests
    {
        [Test]
        public void AverageMedian_ShouldReturnMedian_WhenUsingConverter()
        {
            //odd number of elements
            {
                var items = new[] { "a", "ab", "abc"};
                var median = items.AverageMedian(x => x.Length);
                Assert.AreEqual(2M, median);
            }
            //even number of elements
            {
                var items = new[] { "a", "ab", "abc", "abcd" };
                var median = items.AverageMedian(x => x.Length);
                Assert.AreEqual(2.5M, median);
            }
        }

        [Test]
        public void AverageMedian_ShouldReturnMedian_WhenUsingNumbers()
        {
            //odd number of elements
            {
                var numbers = Enumerable.Range(1, 7).ToArray();
                var median = numbers.AverageMedian();
                Assert.AreEqual(4, median);
            }
            //even number of elements
            {
                var numbers = Enumerable.Range(1, 8).ToArray();
                var median = numbers.AverageMedian();
                Assert.AreEqual(4.5, median);
            }
        }

        [Test]
        public void AverageMedian_ShouldThrowException_WhenUsingValuesNotConvertibleToDecimal()
        {
            var items = new[] { "one", "two", "three" };
            Assert.Throws<FormatException>(() => items.AverageMedian());
        }

        [Test]
        public void AverageTrueMedian_ShouldReturnTheMedianPositioned()
        {
            {
                var numbers = Enumerable.Range(1, 7).ToArray();
                var (opt1, opt2) = numbers.AverageMedianPosition();
                Assert.IsFalse(opt2.IsSome);
                Assert.AreEqual(4, opt1.OrThrow());
            }
            {
                var numbers = Enumerable.Range(1, 8).ToArray();
                var (opt1, opt2) = numbers.AverageMedianPosition();
                Assert.IsTrue(opt2.IsSome);
                Assert.AreEqual(4, opt1.OrThrow());
                Assert.AreEqual(5, opt2.OrThrow());
            }
            {
                var items = Enumerable.Range(1, 7).Select(x => x.ToString()).ToArray();
                var (opt1, opt2) = items.AverageMedianPosition();
                Assert.IsFalse(opt2.IsSome);
                Assert.AreEqual("4", opt1.OrThrow());
            }
            {
                var items = Enumerable.Range(1, 8).Select(x => x.ToString()).ToArray();
                var (opt1, opt2) = items.AverageMedianPosition();
                Assert.IsTrue(opt2.IsSome);
                Assert.AreEqual("4", opt1.OrThrow());
                Assert.AreEqual("5", opt2.OrThrow());
            }
        }

        [Test]
        public void IndexesOf_Should_Return_Found_Indexes_When_Array_Includes_Value()
        {
            var numbers = Enumerable.Range(1, 7).ToArray();
            var indexes = numbers.IndexesOf(2, 5).ToArray();
            Assert.AreEqual(2, indexes.Length);

            Assert.AreEqual(1, indexes[0]);
            Assert.AreEqual(4, indexes[1]);
        }
    }
}

