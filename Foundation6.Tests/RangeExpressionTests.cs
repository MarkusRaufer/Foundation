using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Foundation
{
    [TestFixture]
    public class RangeExpressionTests
    {
        // ReSharper disable InconsistentNaming

        [Test]
        public void Between()
        {
            var sut = new Between<int>(3, 5);

            //in range
            Assert.IsTrue(sut.IsInRange(3));
            Assert.IsTrue(sut.IsInRange(4));
            Assert.IsTrue(sut.IsInRange(5));

            //out of range
            Assert.IsFalse(sut.IsInRange(2));
            Assert.IsFalse(sut.IsInRange(6));
        }

        [Test]
        public void Exactly()
        {
            const int number = 3;
            var sut = new Exactly<int>(number);

            //in range
            Assert.IsTrue(sut.IsInRange(number));
            Assert.AreEqual(number, sut.Value);

            //out of range
            Assert.IsFalse(sut.IsInRange(4));
        }

        [Test]
        public void Generate_CallIsInRangeFirst()
        {
            var number = 1;
            var sut = new BufferdGenerator<int>(() => number++);

            //in range
            Assert.IsTrue(sut.IsInRange(1));

            //out of range
            Assert.IsFalse(sut.IsInRange(0));
            Assert.IsFalse(sut.IsInRange(2));
            
            //compare values
            var expected = Enumerable.Range(1, 1);
            var values = sut.Values.ToList();
            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void Generate_CallValuesFirst()
        {
            var number = 1;
            var sut = new BufferdGenerator<int>(() => number++);

            //compare values
            var expected = Enumerable.Range(1, 1);
            var values = sut.Values.ToList();
            CollectionAssert.AreEqual(expected, values);
            
            //in range
            Assert.IsTrue(sut.IsInRange(1));

            //out of range
            Assert.IsFalse(sut.IsInRange(0));
            Assert.IsFalse(sut.IsInRange(2));
        }

        [Test]
        public void Generate_CallValuesLazy()
        {
            var number = 1;
            int numberOfIncrementCalls = 0;

            int increment()
            {
                numberOfIncrementCalls++;
                number++;

                return number;
            }

            var quantity = 5;
            var sut = new BufferdGenerator<int>(increment, quantity);
           
            {
                var values = sut.Values.ToArray();

                Assert.AreEqual(quantity, values.Length);
                Assert.AreEqual(quantity, numberOfIncrementCalls);
            }
            {
                numberOfIncrementCalls = 0;
                var values = sut.Values.ToArray();

                Assert.AreEqual(quantity, values.Length);
                Assert.AreEqual(0, numberOfIncrementCalls);
            }
        }


        [Test]
        public void Generate_QuantityOfFive_CallIsInRangeFirst()
        {
            var number = 1;
            var sut = new BufferdGenerator<int>(() => number++, 5);

            //in range
            Assert.IsTrue(sut.IsInRange(1));
            Assert.IsTrue(sut.IsInRange(2));
            Assert.IsTrue(sut.IsInRange(3));
            Assert.IsTrue(sut.IsInRange(4));
            Assert.IsTrue(sut.IsInRange(5));

            //out of range
            Assert.IsFalse(sut.IsInRange(0));
            Assert.IsFalse(sut.IsInRange(6));

            //compare values
            var expected = Enumerable.Range(1, 5);
            var values = sut.Values.ToList();
            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void Generate_QuantityOfFive_CallValuesFirst()
        {
            var number = 1;
            var sut = new BufferdGenerator<int>(() => number++, 5);

            //compare values
            var expected = Enumerable.Range(1, 5).ToList();

            //take(2) is just for inner test.
            var values = sut.Values.Take(2).ToList();
            CollectionAssert.AreEqual(expected.Take(2), values);

            values = sut.Values.ToList();
            CollectionAssert.AreEqual(expected, values);

            //in range
            Assert.IsTrue(sut.IsInRange(1));
            Assert.IsTrue(sut.IsInRange(2));
            Assert.IsTrue(sut.IsInRange(3));
            Assert.IsTrue(sut.IsInRange(4));
            Assert.IsTrue(sut.IsInRange(5));

            //out of range
            Assert.IsFalse(sut.IsInRange(0));
            Assert.IsFalse(sut.IsInRange(6));
        }

        [Test]
        public void Matches()
        {
            var sut = new Matching<int>(n => n > 3);

            //in range
            Assert.IsTrue(sut.IsInRange(4));
            Assert.IsTrue(sut.IsInRange(5));

            //out of range
            Assert.IsFalse(sut.IsInRange(1));
            Assert.IsFalse(sut.IsInRange(2));
            Assert.IsFalse(sut.IsInRange(3));
        }

        [Test]
        public void NumericBetween()
        {
            var sut = new NumericBetween<int>(3, 5);

            //in range
            Assert.IsTrue(sut.IsInRange(3));
            Assert.IsTrue(sut.IsInRange(4));
            Assert.IsTrue(sut.IsInRange(5));

            //out of range
            Assert.IsFalse(sut.IsInRange(2));
            Assert.IsFalse(sut.IsInRange(6));

            //compare values
            var expected = new List<int> {3, 4, 5};
            var values = sut.Values.ToList();
            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void OneOf_InRange()
        {
            var expected = new List<int> {1, 3, 5};
            var sut = new OneOf<int>(expected.ToArray());
            
            //in range
            Assert.IsTrue(sut.IsInRange(1));
            Assert.IsTrue(sut.IsInRange(3));
            Assert.IsTrue(sut.IsInRange(5));

            //out of range
            Assert.IsFalse(sut.IsInRange(2));
            Assert.IsFalse(sut.IsInRange(4));
            Assert.IsFalse(sut.IsInRange(6));

            //compare values
            var values = sut.Values.ToList();
            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void OfType_InRange()
        {
            var sut = new OfType<int>();

            //in range
            Assert.IsTrue(sut.IsInRange(-1));
            Assert.AreEqual(-1, sut.Value);

            Assert.IsTrue(sut.IsInRange(0));
            Assert.AreEqual(0, sut.Value);

            Assert.IsTrue(sut.IsInRange(1));
            Assert.AreEqual(1, sut.Value);

            //out of range
            Assert.IsFalse(sut.IsInRange(2.5));
            Assert.IsFalse(sut.IsInRange("4"));
            Assert.IsFalse(sut.IsInRange('A'));
        }
        // ReSharper restore InconsistentNaming
    }
}
