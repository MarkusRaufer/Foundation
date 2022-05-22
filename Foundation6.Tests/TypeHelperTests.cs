using NUnit.Framework;

namespace Foundation
{
    [TestFixture]
    public class TypeHelperTests
    {
        [Test]
        public void GetPrimitiveType_Should_ReturnAValidType_When_UsingValidShortName()
        {
            var expected = typeof(int);
            
            var actual = TypeHelper.GetPrimitveType("int");
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetScalarType_Should_ReturnAValidType_When_UsingValidShortName()
        {
            {
                var expected = typeof(string);
                var actual = TypeHelper.GetScalarType("string");
                Assert.IsNotNull(actual);
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
