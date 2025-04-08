using Foundation.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;

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
            lhs.EqualsArray(rhs).ShouldBeFalse();
        }

        [Test]
        [TestCase(new int[] { }, new int[] { })]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
        public void EqualsArray_Should_ReturnTrue_When_ElementsAreSame(int[] lhs, int[] rhs)
        {
            lhs.EqualsArray(rhs).ShouldBeTrue();
        }

        [Test]
        public void GetEnumerator_Should_ReturnEnumerator_When_CalledGetEnumerator()
        {
            var sut = new[] { 1, 2, 3 };
            var enumerator = sut.GetEnumerator();

            enumerator.MoveNext().ShouldBeTrue();
            sut[0].ShouldBeEquivalentTo(enumerator.Current);

            enumerator.MoveNext().ShouldBeTrue();
            sut[1].ShouldBeEquivalentTo(enumerator.Current);

            enumerator.MoveNext().ShouldBeTrue();
            sut[2].ShouldBeEquivalentTo(enumerator.Current);

            enumerator.MoveNext().ShouldBeFalse();
        }

        [Test]
        public void OfTypes_Should_ReturnOnlyIntAndDoubleValue_When_TypeIsIntAndDouble()
        {
            var sut = new object[] { DateTime.Now, 2, "3", 4.5D };
            
            var comparables = sut.OfTypes(typeof(int), typeof(double));
            comparables.ShouldContain(2);
            comparables.ShouldContain(4.5);
        }

        [Test]
        public void Shuffle_Should_ReturnOnlyIntAndDoubleValue_When_TypeIsIntAndDouble()
        {
            // Arrange
            var sut = Enumerable.Range(5, 3).ToArray();
            var expected = new int[sut.Length];
            Array.Copy(sut, expected, sut.Length);

            var random = new Random();

            var shuffled = sut.Shuffle(random);
            shuffled.All(x => expected.Contains(x)).ShouldBeTrue();
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
