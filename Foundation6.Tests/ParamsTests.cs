using Foundation.Collections.Generic;
using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation
{
    [TestFixture]
    public class ParamsTests
    {
        [Test]
        public void New_ShouldReturnTheCtorArguments_When_Using_Numbers()
        {
            var expected = Enumerable.Range(1, 3).ToArray();

            var parameters = Params.New(expected).ToArray();
            Assert.AreEqual(3, parameters.Length);
            Assert.AreEqual(expected[0], parameters[0]);
            Assert.AreEqual(expected[1], parameters[1]);
            Assert.AreEqual(expected[2], parameters[2]);
        }

        [Test]
        public void New_ShouldReturnTheCtorArguments_When_Using_Objects()
        {
            var number = 5;
            var name = "test";
            var dt = new DateTime(2018, 5, 17);

            var parameters = new Params(number, name, dt).ToArray();
            Assert.AreEqual(3, parameters.Length);
            Assert.AreEqual(number, parameters[0]);
            Assert.AreEqual(name, parameters[1]);
            Assert.AreEqual(dt, parameters[2]);
        }
    }
}
