using NUnit.Framework;
using System.Collections.Generic;

namespace Foundation.Collections.Generic
{
    [TestFixture]
    public class DictionaryExtensionsTests
    {

        [Test]
        public void IsEqualToSet_Should_ReturnFalse_When_Size_IsDifferent()
        {
            var map1 = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" }
            };

            var map2 = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
                { 4, "four" }
            };

            Assert.IsFalse(map1.IsEqualToSet(map2));
        }

        [Test]
        public void IsEqualToSet_Should_ReturnFalse_When_Size_IsEqual_ButElementsKeyIsDifferent()
        {
            var map1 = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" }
            };

            var map2 = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "four" }
            };

            Assert.IsFalse(map1.IsEqualToSet(map2));
        }

        [Test]
        public void IsEqualToSet_Should_ReturnFalse_When_Size_IsEqual_ButElementsValueIsDifferent()
        {
            var map1 = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" }
            };

            var map2 = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" },
                { 4, "three" }
            };

            Assert.IsFalse(map1.IsEqualToSet(map2));
        }

        [Test]
        public void IsEqualToSet_Should_ReturnTrue_When_Elements_And_Size_AreEqual()
        {
            var map1 = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
            };

            var map2 = new Dictionary<int, string>
            {
                { 3, "three" },
                { 1, "one" },
                { 2, "two" },
            };

            Assert.IsTrue(map1.IsEqualToSet(map2));
        }
    }
}
