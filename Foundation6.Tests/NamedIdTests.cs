using FluentAssertions;
using NUnit.Framework;
using System;
using System.Text.Json;

namespace Foundation
{
    [TestFixture]
    public class NamedIdTests
    {
        [Test]
        public void Equals_Should_ReturnsFalse_When_ValuesAreSame_But_NamesAreDifferent()
        {
            var value = 10;

            var sut1 = NamedId.New("name1", value);
            var sut2 = NamedId.New("name2", value);

            sut1.Equals(sut2).Should().BeFalse();
        }

        [Test]
        public void Equals_ReturnsFalse_When_SameNames_And_DifferentValues()
        {
            var sut1 = NamedId.New("x", 10);
            var sut2 = NamedId.New("x", 20);

            sut1.Equals(sut2).Should().BeFalse();
        }

        [Test]
        public void Equals_Should_ReturnsTrue_When_NamesAndValuesAreSame()
        {
            var value = 10;

            var sut1 = NamedId.New("x", value);
            var sut2 = NamedId.New("x", value);

            sut1.Equals(sut2).Should().BeTrue();
        }

        [Test]
        public void Equals_Should_ReturnsTrue_When_NamesAndValuesAreEqual_And_ValuesAreStrings()
        {
            var sut1 = NamedId.New("x", "test");
            var sut2 = NamedId.New("x", "test");

            sut1.Equals(sut2).Should().BeTrue();
        }

        [Test]
        public void EqualsOperator_Should_ReturnsFalse_When_ValuesAreSame_But_NamesAreDifferent()
        {
            var value = 10;

            var sut1 = NamedId.New("name1", value);
            var sut2 = NamedId.New("name2", value);

            var eq = sut1 == sut2;

            eq.Should().BeFalse();
        }

        [Test]
        public void EqualsOperator_Should_ReturnsTrue_When_NamesAndValuesAreEqual_And_ValuesAreStrings()
        {
            var sut1 = NamedId.New("x", "test");
            var sut2 = NamedId.New("x", "test");

            var eq = sut1 == sut2;

            eq.Should().BeTrue();
        }

        [Test]
        public void Compare_Should_ReturnsFalse_When_ValuesAreSameButNamesAreDifferent()
        {
            var value = 10;

            var sut1 = NamedId.New("name1", value);
            var sut2 = NamedId.New("name2", value);

            var cmp = sut1.CompareTo(sut2);
            cmp.Should().Be(-1);
        }

        [Test]
        public void Compare_ReturnsMinus10_When_ValuesAreDifferent()
        {
            var sut1 = NamedId.New("x", 10);
            var sut2 = NamedId.New("x", 20);

            var cmp = sut1.CompareTo(sut2);
            cmp.Should().Be(-10);
        }

        [Test]
        public void Greater_Should_ReturnsFalse_When_ValuesAreSameButNamesAreDifferent()
        {
            var value = 10;

            var sut1 = NamedId.New("name1", value);
            var sut2 = NamedId.New("name2", value);

            var gt = sut1 > sut2;
            gt.Should().BeFalse();
        }

        [Test]
        public void Greater_Should_ReturnsFalse_When_NamesAreSame_And_ValuesAreDifferent()
        {
            var sut1 = NamedId.New("x", 10);
            var sut2 = NamedId.New("x", 20);

            var gt = sut1 > sut2;
            gt.Should().BeFalse();
        }


        [Test]
        public void Greater_Should_ReturnsTrue_When_ValuesAreSameButNamesAreDifferent()
        {
            var value = 10;

            var sut1 = NamedId.New("name2", value);
            var sut2 = NamedId.New("name1", value);

            var gt = sut1 > sut2;
            gt.Should().BeTrue();
        }

        [Test]
        public void Greater_Should_ReturnsTrue_When_NamesAreSame_And_ValuesAreDifferent()
        {
            var sut1 = NamedId.New("x", 20);
            var sut2 = NamedId.New("x", 10);

            var gt = sut1 > sut2;
            gt.Should().BeTrue();
        }

        [Test]
        public void GreaterOrEqual_Should_ReturnsTrue_When_NamesAndValuesAreSame()
        {
            var sut1 = NamedId.New("x", 10);
            var sut2 = NamedId.New("x", 10);

            var gt = sut1 >= sut2;
            gt.Should().BeTrue();
        }

        [Test]
        public void GreaterOrEqual_Should_ReturnsTrue_When_NamesAreSame_And_ValuesAreDifferent()
        {
            var sut1 = NamedId.New("x", 20);
            var sut2 = NamedId.New("x", 10);

            var gt = sut1 >= sut2;
            gt.Should().BeTrue();
        }

        [Test]
        public void Serialize_ReturnsTrue_When_SameTypes_And_SameValues()
        {
            var name = "BirthDay";
            var value = Guid.NewGuid();
            var expected = NamedId.New(name, value);
            var json = JsonSerializer.Serialize(expected);
            
            var sut = JsonSerializer.Deserialize<NamedId>(json);
            
            var eq = sut == expected;
            eq.Should().BeTrue();
        }

        [Test]
        public void Smaller_Should_ReturnsFalse_When_ValuesAreSameButNamesAreDifferent()
        {
            var value = 10;

            var sut1 = NamedId.New("name2", value);
            var sut2 = NamedId.New("name1", value);

            var st = sut1 < sut2;
            st.Should().BeFalse();
        }

        [Test]
        public void Smaller_Should_ReturnsTrue_When_ValuesAreSameButNamesAreDifferent()
        {
            var value = 10;

            var sut1 = NamedId.New("name1", value);
            var sut2 = NamedId.New("name2", value);

            var gt = sut1 < sut2;
            gt.Should().BeTrue();
        }

        [Test]
        public void SmallerOrEqual_Should_ReturnsTrue_When_ValuesAreSameButNamesAreDifferent()
        {
            var value = 10;

            var sut1 = NamedId.New("name1", value);
            var sut2 = NamedId.New("name2", value);

            var gt = sut1 <= sut2;
            gt.Should().BeTrue();
        }

        [Test]
        public void SmallerOrEqual_Should_ReturnsTrue_When_NamesAreSame_And_ValuesAreDifferent()
        {
            var sut1 = NamedId.New("x", 10);
            var sut2 = NamedId.New("x", 20);

            var gt = sut1 <= sut2;
            gt.Should().BeTrue();
        }
    }
}
