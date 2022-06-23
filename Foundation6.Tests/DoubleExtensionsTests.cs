using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation
{
    [TestFixture]
    public class DoubleExtensionsTests
    {
        [Test]
        public void Equal_Should_ReturnTrue_When_ValuesAreDifferent_And_EpsilonIsUsed()
        {
            var lhs = 1.253D;
            var rhs = 1.258D;

            Assert.IsTrue(lhs.Equal(rhs, .01));
        }

        [Test]
        public void Equal_Should_ReturnTrue_When_ValuesAreEqual_And_DefaultEpsilonIsUsed()
        {
            var lhs = 1.25D;
            var rhs = 1.25D;

            Assert.IsTrue(lhs.Equal(rhs));
        }

        [Test]
        public void GreaterThan_Should_ReturnFalse_When_Difference_IsSmaller_ThanEpsilon()
        {
            var lhs = 2.123D;
            var rhs = 2.122D;

            Assert.IsTrue(lhs > rhs);
            Assert.IsFalse(lhs.GreaterThan(rhs, .01));
        }

        [Test]
        public void GreaterThanOrEqual_Should_ReturnTrue_When_Difference_IsSmaller_ThanEpsilon()
        {
            var lhs = 2.123D;
            var rhs = 2.122D;

            Assert.IsTrue(lhs > rhs);
            Assert.IsTrue(lhs.GreaterThanOrEqual(rhs, .01));
        }

        [Test]
        public void GreaterThan_Should_ReturnTrue_When_LhsIsGreaterThanRhs_And_DefaultEpsilonIsUsed()
        {
            var lhs = 2.0D;
            var rhs = 1.0D;

            Assert.IsTrue(lhs > rhs);
            Assert.IsTrue(lhs.GreaterThan(rhs));
        }

        [Test]
        public void LessThan_Should_ReturnFalse_When_Difference_IsSmaller_ThanEpsilon()
        {
            var lhs = 2.122D;
            var rhs = 2.123D;

            Assert.IsTrue(lhs < rhs);
            Assert.IsFalse(lhs.LessThan(rhs, .01));
        }

        [Test]
        public void LessThan_Should_ReturnTrue_When_LhsIsGreaterThanRhs_And_DefaultEpsilonIsUsed()
        {
            var lhs = 1.0D;
            var rhs = 2.0D;

            Assert.IsTrue(lhs < rhs);
            Assert.IsTrue(lhs.LessThan(rhs));
        }

        [Test]
        public void LessThanOrEqual_Should_ReturnTrue_When_Difference_IsSmaller_ThanEpsilon()
        {
            var lhs = 2.122D;
            var rhs = 2.123D;

            Assert.IsTrue(lhs < rhs);
            Assert.IsTrue(lhs.LessThanOrEqual(rhs, .01));
        }
    }
}
