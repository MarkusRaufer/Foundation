using NUnit.Framework;
using System.Linq;

namespace Foundation
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void CompareRegardingNumbers()
        {
            {
                var result = "90.5".CompareRegardingNumbers("100.3");
                Assert.AreEqual(-1, result);
            }
            {
                var result = "90".CompareRegardingNumbers("100");
                Assert.AreEqual(-1, result);
            }
            {
                var result = "10".CompareRegardingNumbers("9");
                Assert.AreEqual(1, result);
            }
            {
                var result = "10".CompareRegardingNumbers("10");
                Assert.AreEqual(0, result);
            }
            {
                var result = "ABC".CompareRegardingNumbers("ABc");
                var r2 = string.Compare("ABC", "ABc", true);
                Assert.AreEqual(1, result);
            }
            {
                var result = "AbC".CompareRegardingNumbers("ABC");
                Assert.AreEqual(-1, result);
            }
            {
                var result = "ABC".CompareRegardingNumbers("abc", true);
                Assert.AreEqual(0, result);
            }
        }

        [Test]
        public void IndexFromEnd_ShouldReturnAPositiveInt_When_UsingChar()
        {
            var str = "Invoice<TId> {";
            var index = str.IndexFromEnd('<');
            Assert.AreEqual(7, index);
        }

        [Test]
        public void IndexFromEnd_ShouldReturnAPositiveInt_When_UsingString()
        {
            var str = "Invoice <<ID>> {";
            var index = str.IndexFromEnd("<<");
            Assert.AreEqual(8, index);
        }

        [Test]
        public void IndexesOf_ShouldReturn2Indices_When_Using2Strings()
        {
            var str = "class Invoice <<aggregateroot>> {";
            var actual = str.IndexesOf("<<", ">>").ToArray();
            Assert.AreEqual(14, actual[0]);
            Assert.AreEqual(29, actual[1]);
        }

        [Test]
        public void ReduceSpaces()
        {
            var str = "1 2  3   4     5";
            var result = str.ReduceSpaces();
            Assert.AreEqual("1 2 3 4 5", result);
        }

        [Test]
        public void RemoveApostrophes_double_quoted_apostrophes()
        {
            var value = "Test";
            var str = $"\"{value}\"";
            var result = str.TrimApostrophes();
            Assert.AreEqual(value, result);
        }

        [Test]
        public void RemoveApostrophes_no_apostrophes()
        {
            var value = "Test";
            var result = value.TrimApostrophes();
            Assert.AreEqual(value, result);
        }

        [Test]
        public void RemoveApostrophes_single_quoted_apostrophes()
        {
            var value = "Test";
            var str = $"'{value}'";
            var result = str.TrimApostrophes();
            Assert.AreEqual(value, result);
        }

        [Test]
        public void SplitAtIndex()
        {
            {
                var tokens = "onetwo".SplitAtIndex(3).ToList();
                Assert.AreEqual(2, tokens.Count);

                Assert.AreEqual("one", tokens[0]);
                Assert.AreEqual("two", tokens[1]);
            }

            var sut = "123456789";
            {
                var tokens = sut.SplitAtIndex(0).ToList();
                Assert.AreEqual(1, tokens.Count);
                Assert.AreEqual("123456789", tokens[0]);
            }
            {
                var tokens = sut.SplitAtIndex(8).ToList();
                Assert.AreEqual(2, tokens.Count);

                Assert.AreEqual("12345678", tokens[0]);
                Assert.AreEqual("9", tokens[1]);
            }
            {
                var tokens = sut.SplitAtIndex(4).ToList();
                Assert.AreEqual(2, tokens.Count);

                Assert.AreEqual("1234", tokens[0]);
                Assert.AreEqual("56789", tokens[1]);
            }
            {
                var tokens = sut.SplitAtIndex(2, 5).ToList();
                Assert.AreEqual(3, tokens.Count);

                Assert.AreEqual("12", tokens[0]);
                Assert.AreEqual("345", tokens[1]);
                Assert.AreEqual("6789", tokens[2]);
            }
            {
                var tokens = sut.SplitAtIndex(1, 2, 4, 6).ToList();
                Assert.AreEqual(5, tokens.Count);

                Assert.AreEqual("1", tokens[0]);
                Assert.AreEqual("2", tokens[1]);
                Assert.AreEqual("34", tokens[2]);
                Assert.AreEqual("56", tokens[3]);
                Assert.AreEqual("789", tokens[4]);
            }
        }

        [Test]
        public void SubstringBetween_Should_ReturnSubstringWithLeftAndRightLimits_When_Using2CharsAndInclusiveIsFalse()
        {
            {
                var str = "class Invoice \"aggregateroot\" {";
                var actual = str.SubstringBetween("\"", "\"", false);
                Assert.AreEqual("aggregateroot", actual);
            }
            {
                var str = "\"\"";
                var actual = str.SubstringBetween("\"", "\"", false);
                Assert.AreEqual("", actual);
            }
            {
                var str = "\"1\"";
                var actual = str.SubstringBetween("\"", "\"", false);
                Assert.AreEqual("1", actual);
            }
        }

        [Test]
        public void SubstringBetween_ShouldReturnSubstringWithLeftAndRightLimits_When_Using2CharsAndInclusiveIsTrue()
        {
            {
                var str = "class Invoice \"aggregateroot\" {";
                var actual = str.SubstringBetween("\"", "\"");
                Assert.AreEqual("\"aggregateroot\"", actual);
            }
            {
                var str = "\"\"";
                var actual = str.SubstringBetween("\"", "\"");
                Assert.AreEqual("\"\"", actual);
            }
            {
                var str = "\"1\"";
                var actual = str.SubstringBetween("\"", "\"");
                Assert.AreEqual("\"1\"", actual);
            }
        }

        [Test]
        public void SubstringBetween_ShouldReturnSubstringWithLeftAndRightLimits_When_Using2StringsAndInclusiveIsFalse()
        {
            {
                var str = "class Invoice <<aggregateroot>> {";
                var actual = str.SubstringBetween("<<", ">>", false);
                Assert.AreEqual("aggregateroot", actual);
            }
            {
                var str = "<<>>";
                var actual = str.SubstringBetween("<<", ">>", false);
                Assert.AreEqual("", actual);
            }
            {
                var str = "<<1>>";
                var actual = str.SubstringBetween("<<", ">>", false);
                Assert.AreEqual("1", actual);
            }
        }

        [Test]
        public void SubstringBetween_ShouldReturnSubstringWithLeftAndRightLimits_When_Using2StringsAndInclusiveIsTrue()
        {
            {
                var str = "class Invoice <<aggregateroot>> {";
                var actual = str.SubstringBetween("<<", ">>");
                Assert.AreEqual("<<aggregateroot>>", actual);
            }
            {
                var str = "<<>>";
                var actual = str.SubstringBetween("<<", ">>");
                Assert.AreEqual("<<>>", actual);
            }
            {
                var str = "<<1>>";
                var actual = str.SubstringBetween("<<", ">>");
                Assert.AreEqual("<<1>>", actual);
            }
        }

        [Test]
        public void SubstringFromIndex()
        {
            var sut = "123456789";
            {
                var actual = sut.SubstringFromIndex(1, 2);
                Assert.AreEqual("23", actual);
            }
            {
                var actual = sut.SubstringFromIndex(3, 4);
                Assert.AreEqual("45", actual);
            }
            {
                var actual = sut.SubstringFromIndex(4, 4);
                Assert.AreEqual("5", actual);
            }
            {
                var actual = sut.SubstringFromIndex(1, 5);
                Assert.AreEqual("23456", actual);
            }
        }
    }
}
