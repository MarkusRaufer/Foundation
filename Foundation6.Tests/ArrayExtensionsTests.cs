using FluentAssertions;
using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation
{
    [TestFixture]
    public class ArrayExtensionsTests
    {
        [Test]
        [TestCase(new int[] {}, null)]
        [TestCase(new [] { 1, 2, 3 }, new [] { 1, 4, 3 })]
        public void EqualsArray_Should_ReturnFalse_When_ElementsAreNotSame(int[] lhs, int[] rhs)
        {
            Assert.False(lhs.EqualsArray(rhs));
        }

        [Test]
        [TestCase(new int[] { }, new int[] { })]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
        public void EqualsArray_Should_ReturnTrue_When_ElementsAreSame(int[] lhs, int[] rhs)
        {
            Assert.True(lhs.EqualsArray(rhs));
        }

        [Test]
        public void GetEnumerator_Should_ReturnEnumerator_When_CalledGetEnumerator()
        {
            var sut = new[] { 1, 2, 3 };
            var enumerator = sut.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(sut[0], enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(sut[1], enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(sut[2], enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext());
        }

        [Test]
        public void OfTypes_Should_()
        {
            var sut = new object[] { DateTime.Now, 2, "3", 4.5D };
            
            var comparables = sut.OfTypes(typeof(int), typeof(double));
            comparables.Should().Contain(2);
            comparables.Should().Contain(4.5);
        }

        [Test]
        [TestCase(new int [0])]
        public void ThrowIfNullOrEmpty_Should_ThrowArgumentException_When_Empty(int [] sut)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.ThrowIfNullOrEmpty());
        }


        [Test]
        [TestCase(null)]
        public void ThrowIfNullOrEmpty_Should_ThrowArgumentNullException_When_Null(int[] sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.ThrowIfNullOrEmpty());
        }
    }
}
