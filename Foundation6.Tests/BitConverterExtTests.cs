using NUnit.Framework;
using System.Linq;

namespace Foundation
{
    [TestFixture]
    public class BitConverterExtTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void GetBytes_Should_Return16Bytes_When_Called(decimal number)
        {
            var bytes = BitConverterExt.GetBytes(number);
            Assert.AreEqual(16, bytes.Length);
            var value = (byte)number;
            Assert.AreEqual(value, bytes[0]);
        }

        [Test]
        [TestCase(12324.56)]
        [TestCase(12.3456)]
        public void GetBytes_Should_Return16Bytes_When_Called_GetBytesAndToDecimal(decimal number)
        {
            var bytes = BitConverterExt.GetBytes(number);
            var value = BitConverterExt.ToDecimal(bytes);
            Assert.AreEqual(number, value);
        }

        [Test]
        [TestCase(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [TestCase(new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
        public void ToDecimal_Should_ReturnDecimal_When_Called(byte[] bytes)
        {
            var value = BitConverterExt.ToDecimal(bytes);
            var expected = (decimal)bytes[0];
            Assert.AreEqual(expected, value);
        }
    }
}
