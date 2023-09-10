using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation;

[TestFixture]
public class TypeExtensionsTests
{
    private record MyType<T>(T Value);

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
    public void IsOfGenericType_Should_ReturnFalse_When_TypesAreNotGeneric()
    {
        var type = typeof(MyType<int>);
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
        var type = typeof(MyType<int>);
        var genType = typeof(MyType<>);

        Assert.IsTrue(type.IsOfGenericType(genType));
    }

    [Test]
    public void IsOfGenericType_Should_ReturnTrue_When_Other_IsSameButDifferentGenericParameter()
    {
        var type = typeof(MyType<int>);
        var genType = typeof(MyType<string>);

        Assert.IsTrue(type.IsOfGenericType(genType));
    }
}
