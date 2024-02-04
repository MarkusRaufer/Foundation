using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Foundation;

[TestFixture]
public class TypeExtensionsTests
{
    private record MyRecord<T>(T Value);

    private class MutableType1
    {
        public string? Name { get; set; }
        public int Number { get; set; }
    }

    private class ImmutableType1
    {
        private readonly int _number;

        public ImmutableType1(int number)
        {
            _number = number;
        }

        public int Number => _number;
    }

    [Test]
    public void ImplementsInterface_Should_ReturnFalse_When_String_DoesNotImplementInterface()
    {
        var type = typeof(string);

        var result = type.ImplementsInterface(typeof(IActivatable));
        Assert.IsFalse(result);
    }

    [Test]
    public void ImplementsInterface_Should_ReturnTrue_When_String_ImplementsInterface()
    {
        var type = typeof(string);
        {
            var result = type.ImplementsInterface(typeof(IEnumerable<char>));
            Assert.IsTrue(result);
        }
        {
            var result = type.ImplementsInterface(typeof(IComparable));
            Assert.IsTrue(result);
        }
        {
            var result = type.ImplementsInterface(typeof(IComparable<string>));
            Assert.IsTrue(result);
        }
        {
            var result = type.ImplementsInterface(typeof(IComparable<>));
            Assert.IsTrue(result);
        }
    }

    [Test]
    public void IsImmutable_Should_ReturnFalse_When_TypeIsMutable()
    {
        var type = typeof(MutableType1);
        var isImmutable = type.IsImmutable();
        isImmutable.Should().BeFalse();
    }

    [Test]
    public void IsImmutable_Should_ReturnTrue_When_TypeIsImmutableClass()
    {

        var type = typeof(ImmutableType1);
        var isImmutable = type.IsImmutable();
        isImmutable.Should().BeTrue();
    }

    [Test]
    public void IsImmutable_Should_ReturnTrue_When_TypeIsInteger()
    {
        var type = typeof(int);
        var isImmutable = type.IsImmutable();
        isImmutable.Should().BeTrue();
    }

    [Test]
    public void IsImmutable_Should_ReturnTrue_When_TypeIsGenericRecordWithReferenceType()
    {
        var type = typeof(MyRecord<string>);
        var isImmutable = type.IsImmutable();
        isImmutable.Should().BeTrue();
    }

    [Test]
    public void IsImmutable_Should_ReturnTrue_When_TypeIsGenericRecordWithValueType()
    {
        var type = typeof(MyRecord<int>);
        var isImmutable = type.IsImmutable();
        isImmutable.Should().BeTrue();
    }

    [Test]
    public void IsOfGenericType_Should_ReturnFalse_When_TypesAreNotGeneric()
    {
        var type = typeof(MyRecord<int>);
        {
            //right not generic
            Assert.IsFalse(type.IsOfGenericType(typeof(int)));
        }
        {
            //left not generic
            Assert.IsFalse(typeof(int).IsOfGenericType(type));
        }
        {
            //both not generic
            Assert.IsFalse(typeof(int).IsOfGenericType(typeof(int)));
        }
    }

    [Test]
    public void IsOfGenericType_Should_ReturnTrue_When_Other_IsSameTypeWithoutGenericParameter()
    {
        var type = typeof(MyRecord<int>);
        var genType = typeof(MyRecord<>);

        Assert.IsTrue(type.IsOfGenericType(genType));
    }

    [Test]
    public void IsOfGenericType_Should_ReturnTrue_When_Other_IsSameButDifferentGenericParameter()
    {
        var type = typeof(MyRecord<int>);
        var genType = typeof(MyRecord<string>);

        Assert.IsTrue(type.IsOfGenericType(genType));
    }
}
