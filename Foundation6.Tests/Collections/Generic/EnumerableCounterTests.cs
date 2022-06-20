using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Collections.Generic
{
    [TestFixture]
    public class EnumerableCounterTests
    {
        [Test]
        public void Ctor_Should_HaveCountOf_0_When_NotIterated()
        {
            var numbers = Enumerable.Range(1, 10);

            var sut = new EnumerableCounter<int>(numbers);
            Assert.AreEqual(0, sut.Count);
        }

        [Test]
        public void Ctor_Should_HaveCountOf_10_When_Iterated()
        {
            var numbers = Enumerable.Range(1, 10);

            var sut = new EnumerableCounter<int>(numbers);

            foreach (var _ in sut)
            {
            }
            Assert.AreEqual(10, sut.Count);
        }

        [Test]
        public void Cast_Should_HaveCountOf_10_When_LeftEnumerableCounter_RightArray_Iterated()
        {
            var numbers = Enumerable.Range(1, 10).ToArray();

            EnumerableCounter<int> sut = numbers;

            foreach (var _ in sut)
            {
            }
            Assert.AreEqual(10, sut.Count);
        }

        [Test]
        public void Cast_Should_HaveCountOf_10_When_MethodArgumentIsArray_Iterated()
        {
            var numbers = Enumerable.Range(1, 10).ToArray();

            int method(EnumerableCounter<int> sut)
            {
                foreach (var _ in sut)
                {
                }
                return sut.Count;
            }
            
            Assert.AreEqual(10, method(numbers));
        }
    }
}
