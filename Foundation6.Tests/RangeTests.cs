using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Foundation
{
    [TestFixture]
    public class RangeTests
    {
        // ReSharper disable InconsistentNaming

        [Test]
        public void Between_One_IsInRange()
        {
            var min = 3;
            var max = 5;
            var sut = Range.Create(Is.Between(min, max));
            Assert.IsTrue(sut.IsInRange(min));
            Assert.IsTrue(sut.IsInRange(4));
            Assert.IsTrue(sut.IsInRange(max));

            Assert.IsFalse(sut.IsInRange(1));
            Assert.IsFalse(sut.IsInRange(2));
            Assert.IsFalse(sut.IsInRange(6));

            var between = sut.RangeExpressions.Between().Single();
            Assert.AreEqual(min, between.Min);
            Assert.AreEqual(max, between.Max);
        }

        [Test]
        public void Between_One_Values()
        {
            var sut = Range.Create(Is.Between(3, 5));
            Assert.IsFalse(sut.ContainsOnlyValueExpressions);
            Assert.AreEqual(0, sut.Values.Count());
        }

        [Test]
        public void Between_Three_IsInRange()
        {
            var sut = Range.Create(Is.Between(2, 3), 
                                   Is.Between(5, 6), 
                                   Is.Between(8, 9));

            Assert.IsTrue(sut.IsInRange(2));
            Assert.IsTrue(sut.IsInRange(3));

            Assert.IsTrue(sut.IsInRange(5));
            Assert.IsTrue(sut.IsInRange(6));

            Assert.IsTrue(sut.IsInRange(8));
            Assert.IsTrue(sut.IsInRange(9));

            Assert.IsFalse(sut.IsInRange(1));
            Assert.IsFalse(sut.IsInRange(4));
            Assert.IsFalse(sut.IsInRange(7));
            Assert.IsFalse(sut.IsInRange(10));
        }

        [Test]
        public void Exactly_One_IsInRange()
        {
            var value = 3;
            var sut = Range.Create(Is.Exactly(value));
            Assert.IsTrue(sut.IsInRange(value));

            Assert.IsFalse(sut.IsInRange(2));
            Assert.IsFalse(sut.IsInRange(4));

            var singleValue = sut.RangeExpressions.SingleValue().Single();
            Assert.AreEqual(value, singleValue.Value);
        }

        [Test]
        public void Exactly_One_Values()
        {
            var sut = Range.Create(Is.Exactly(3));
            Assert.IsTrue(sut.ContainsOnlyValueExpressions);

            var values = sut.Values.ToList();
            Assert.AreEqual(3, values.First());
        }

        [Test]
        public void Exactly_Three_IsInRange()
        {
            var sut = Range.Create(Is.Exactly(1), Is.Exactly(3), Is.Exactly(5));
            Assert.IsTrue(sut.IsInRange(1));
            Assert.IsTrue(sut.IsInRange(3));
            Assert.IsTrue(sut.IsInRange(5));

            Assert.IsFalse(sut.IsInRange(0));
            Assert.IsFalse(sut.IsInRange(2));
            Assert.IsFalse(sut.IsInRange(4));
            Assert.IsFalse(sut.IsInRange(6));

            var singleValues = sut.RangeExpressions.SingleValue().ToList();
            Assert.AreEqual(3, singleValues.Count);
            Assert.AreEqual(1, singleValues[0].Value);
            Assert.AreEqual(3, singleValues[1].Value);
            Assert.AreEqual(5, singleValues[2].Value);
        }

        [Test]
        public void Matching_One()
        {
            var sut = Range.Create(Is.Matching<int>(n => n > 3));
            Assert.IsFalse(sut.IsInRange(2));
            Assert.IsFalse(sut.IsInRange(3));

            Assert.IsTrue(sut.IsInRange(4));
            Assert.IsTrue(sut.IsInRange(5));

            Assert.AreEqual(0, sut.Values.Count());
        }

        [Test]
        public void Matching_Three()
        {
            var sut = Range.Create(Is.Matching<int>(n => n == 2), 
                                   Is.Matching<int>(n => n == 5), 
                                   Is.Matching<int>(n => n == 7));

            Assert.IsFalse(sut.IsInRange(1));
            Assert.IsFalse(sut.IsInRange(3));
            Assert.IsFalse(sut.IsInRange(4));
            Assert.IsFalse(sut.IsInRange(6));
            Assert.IsFalse(sut.IsInRange(8));

            Assert.IsTrue(sut.IsInRange(2));
            Assert.IsTrue(sut.IsInRange(5));
            Assert.IsTrue(sut.IsInRange(7));

            Assert.AreEqual(0, sut.Values.Count());
        }

        [Test]
        public void Mixed_IsInRange()
        {
            var sut = Range.Create(Is.OneOf(2, 4, 7), 
                                   Is.Exactly(6), 
                                   Is.Between(8, 10),
                                   Is.Matching<int>(n => n > 23 && n < 25));

            Assert.IsTrue(sut.IsInRange(2));
            Assert.IsTrue(sut.IsInRange(4));
            Assert.IsTrue(sut.IsInRange(6));
            Assert.IsTrue(sut.IsInRange(7));
            Assert.IsTrue(sut.IsInRange(8));
            Assert.IsTrue(sut.IsInRange(9));
            Assert.IsTrue(sut.IsInRange(10));
            Assert.IsTrue(sut.IsInRange(24));

            Assert.IsFalse(sut.IsInRange(1));
            Assert.IsFalse(sut.IsInRange(3));
            Assert.IsFalse(sut.IsInRange(5));
            Assert.IsFalse(sut.IsInRange(11));
            Assert.IsFalse(sut.IsInRange(23));
            Assert.IsFalse(sut.IsInRange(25));

            
        }
        
        [Test]
        public void OneOf_One_IsInRange()
        {
            var sut = Range.Create(Is.OneOf(1, 3, 5));
            Assert.IsTrue(sut.IsInRange(1));
            Assert.IsTrue(sut.IsInRange(3));
            Assert.IsTrue(sut.IsInRange(5));

            Assert.IsFalse(sut.IsInRange(2));
            Assert.IsFalse(sut.IsInRange(4));
            Assert.IsFalse(sut.IsInRange(6));
        }

        [Test]
        public void OneOf_WithStrings_One_IsInRange()
        {
            var sut = Range.Create(Is.OneOf("1", "3", "5"));
            Assert.IsTrue(sut.IsInRange("1"));
            Assert.IsTrue(sut.IsInRange("3"));
            Assert.IsTrue(sut.IsInRange("5"));

            Assert.IsFalse(sut.IsInRange("2"));
            Assert.IsFalse(sut.IsInRange("4"));
            Assert.IsFalse(sut.IsInRange("6"));

            var multiValue = sut.RangeExpressions.MultiValue().Single();
            var values = multiValue.Values.ToList();
            Assert.AreEqual(3, values.Count);

            Assert.AreEqual("1", values[0]);
            Assert.AreEqual("3", values[1]);
            Assert.AreEqual("5", values[2]);
        }

        [Test]
        public void OneOf_Three_IsInRange()
        {
            var sut = Range.Create(Is.OneOf(2, 4), Is.OneOf(6, 8), Is.OneOf(10, 12));
            Assert.IsTrue(sut.IsInRange(2));
            Assert.IsTrue(sut.IsInRange(4));
            Assert.IsTrue(sut.IsInRange(6));
            Assert.IsTrue(sut.IsInRange(8));
            Assert.IsTrue(sut.IsInRange(10));
            Assert.IsTrue(sut.IsInRange(12));

            Assert.IsFalse(sut.IsInRange(1));
            Assert.IsFalse(sut.IsInRange(3));
            Assert.IsFalse(sut.IsInRange(5));
            Assert.IsFalse(sut.IsInRange(7));
            Assert.IsFalse(sut.IsInRange(9));
            Assert.IsFalse(sut.IsInRange(11));
            Assert.IsFalse(sut.IsInRange(13));

            var multiValues = sut.RangeExpressions.MultiValue().ToList();
            Assert.AreEqual(3, multiValues.Count);

            var values = multiValues.SelectMany(mv => mv.Values).ToList();
            Assert.AreEqual(6, values.Count);
            Assert.AreEqual(2, values[0]);
            Assert.AreEqual(4, values[1]);
            Assert.AreEqual(6, values[2]);
            Assert.AreEqual(8, values[3]);
            Assert.AreEqual(10, values[4]);
            Assert.AreEqual(12, values[5]);

        }

        [Test]
        public void TypeOf()
        {
            var sut = Range.Create<object, int>(Is.OfType<int>());
            Assert.IsTrue(sut.IsInRange(-1));
            Assert.AreEqual(-1, sut.Values.Single());

            Assert.IsTrue(sut.IsInRange(0));
            Assert.AreEqual(0, sut.Values.Single());

            Assert.IsTrue(sut.IsInRange(1));
            Assert.AreEqual(1, sut.Values.Single());

            Assert.IsFalse(sut.IsInRange(2.5));
            Assert.IsFalse(sut.IsInRange("4"));
            Assert.IsFalse(sut.IsInRange('A'));
        }

        [Test]
        public void Values_Mixed()
        {
            var sut = Range.Create(Is.OneOf(2, 4), Is.Exactly(6), Is.Between(8, 10));
            Assert.IsFalse(sut.ContainsOnlyValueExpressions);
            Assert.AreEqual(0, sut.Values.Count());
        }

        [Test]
        public void Values_Mixed_ValueRangeExpressionsOnly()
        {
            var sut = Range.Create(Is.OneOf(2, 4), Is.Exactly(6), Is.NumericBetween(8, 10));
            Assert.IsTrue(sut.ContainsOnlyValueExpressions);

            var expected = new List<int> { 2, 4, 6, 8, 9, 10 };
            var values = sut.Values.ToList();
            Assert.AreEqual(expected.Count, values.Count);
            CollectionAssert.AreEqual(expected, values);
        }
        
        [Test]
        public void Values_One_NumericBetween()
        {
            var sut = Range.Create(Is.NumericBetween(3, 5));
            Assert.IsTrue(sut.ContainsOnlyValueExpressions);

            var expected = new List<int> { 3, 4, 5 };
            var values = sut.Values.ToList();
            Assert.AreEqual(expected.Count, values.Count);
            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void Values_One_OneOf()
        {
            var sut = Range.Create(Is.OneOf(1, 3, 5));
            Assert.IsTrue(sut.ContainsOnlyValueExpressions);

            var expected = new List<int> { 1, 3, 5 };
            var values = sut.Values.ToList();
            Assert.AreEqual(expected.Count, values.Count);
            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void Values_Three_Between()
        {
            var sut = Range.Create(Is.Between(2, 3), Is.Between(5, 6), Is.Between(8, 9));
            Assert.IsFalse(sut.ContainsOnlyValueExpressions);
            Assert.AreEqual(0, sut.Values.Count());
        }

        [Test]
        public void Values_Three_Exactly()
        {
            var sut = Range.Create(Is.Exactly(1), Is.Exactly(3), Is.Exactly(5));
            Assert.IsTrue(sut.ContainsOnlyValueExpressions);

            var expected = new List<int> { 1, 3, 5 };
            var values = sut.Values.ToList();
            CollectionAssert.AreEqual(expected, values);
        }
        
        [Test]
        public void Values_Three_NumericBetween()
        {
            var sut = Range.Create(Is.NumericBetween(2, 3), Is.NumericBetween(5, 6), Is.NumericBetween(8, 9));
            Assert.IsTrue(sut.ContainsOnlyValueExpressions);
            
            var expected = new List<int> { 2, 3, 5, 6, 8, 9 };
            var values = sut.Values.ToList();
            Assert.AreEqual(expected.Count, values.Count);
            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void Values_Three_OneOf()
        {
            var sut = Range.Create(Is.OneOf(1, 3), Is.OneOf(5, 7), Is.OneOf(9, 11));
            Assert.IsTrue(sut.ContainsOnlyValueExpressions);

            var expected = new List<int> { 1, 3, 5, 7, 9, 11 };
            var values = sut.Values.ToList();
            Assert.AreEqual(expected.Count, values.Count);
            CollectionAssert.AreEqual(expected, values);
        }

        // ReSharper restore InconsistentNaming
    }
}
