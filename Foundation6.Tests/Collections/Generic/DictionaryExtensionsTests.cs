using NUnit.Framework;
using System.Collections.Generic;

namespace Foundation.Collections.Generic
{
    [TestFixture]
    public class DictionaryExtensionsTests
    {
        [Test]
        public void IsEqualTo_Should_ReturnTrue_When_Elements_Size_AreEqual()
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

            Assert.IsTrue(map1.IsEqualTo(map2));
        }

        [Test]
        public void IsEqualTo_Should_ReturnTrue_When_Elements_Size_Positions_AreEqual()
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
                { 3, "three" }
            };

            Assert.IsFalse(map1.Equals(map2));
            Assert.IsTrue(map1.IsEqualTo(map2));
        }
    }
}
