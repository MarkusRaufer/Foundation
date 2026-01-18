using Foundation.Reflection;
using NUnit.Framework;
using Shouldly;
using System.Collections.ObjectModel;

namespace Foundation.ComponentModel;

[TestFixture]
public class DuckTypeTests
{
    public interface IDuckType
    {
        void Clear();
        int Count { get; }
        bool Remove(int item);
        bool TryGetValue(int key, Out value);
    }

    public record Person(string FirstName, string LastName, DateOnly DateOfBirth)
    {
        public bool TryGetNames(out string firstName, out string lastName)
        {
            firstName = FirstName;
            lastName = LastName;
            return true;
        }
    }

    [Test]
    public void Action_Should_CallMethodOfObject_When_MethodExistsAndHasNoParameter()
    {
        // Arrange
        var list = Enumerable.Range(1, 3).ToList();

        // Act
        var result = DuckType.Action((IDuckType x) => x.Clear(), list);

        // Assert
        result.IsOk.ShouldBeTrue();
        list.Count.ShouldBe(0);
    }

    [Test]
    public void Property_Should_ReturnValue_When_PropertyExists()
    {
        // Arrange
        var list = Enumerable.Range(1, 3).ToList();

        // Act
        var result = DuckType.Property((IDuckType x) => x.Count, list);

        // Assert
        result.TryGetOk(out var value).ShouldBeTrue();
        value.ShouldBe(list.Count);
    }

    [Test]
    public void Method_Should_ReturnTheMethodsReturnValue_When_MethodExistsAndHasOneParameter()
    {
        // Arrange
        var dictionary = Enumerable.Range(1, 3).ToDictionary(i => i, i => $"{i}");
        var count = dictionary.Count;

        // Act
        var result = DuckType.Method((IDuckType x) => x.Remove(2), dictionary);

        // Assert
        result.TryGetOk(out var value).ShouldBeTrue();
        value.ShouldBe(true);
        dictionary.Count.ShouldBe(count - 1);
    }

    [Test]
    public void MethodWithOutParameters_Should_InvokeMethod_When_MethodExists()
    {
        // Arrange
        IDictionary<int, string> dictionary = Enumerable.Range(1, 3).ToDictionary(i => i, i => $"{i}");

        // Act
        object[] values = new object[1];
        var result = DuckType.OutParameterMethod(nameof(dictionary.TryGetValue), dictionary, values, 2);

        // Assert
        result.TryGetOk(out var ok).ShouldBeTrue();
        ok.ShouldBe(true);
        values[0].ShouldBe("2");
    }

    [Test]
    public void TryMethod3_Should_InvokeMethod_When_MethodExists()
    {
        // Arrange
        var person = new Person("John", "Doe", new DateOnly(1991, 2, 3));

        // Act
        object[] values = new object[2];
        var result = DuckType.OutParameterMethod(nameof(Person.TryGetNames), person, values, []);

        // Assert
        result.TryGetOk(out var ok).ShouldBeTrue();
        ok.ShouldBe(true);
        values[0].ShouldBe("John");
        values[1].ShouldBe("Doe");
    }

    [Test]
    public void TryMethod2_Should_InvokeMethod_When_MethodExists()
    {
        // Arrange

        IDictionary<int, string> dictionary = Enumerable.Range(1, 3).ToDictionary(i => i, i => $"{i}");
        IReadOnlyDictionary<int, string> readOnlyDictionary = new ReadOnlyDictionary<int, string>(dictionary);

        object dictObj = dictionary;
        object readOnlyDictObj = readOnlyDictionary;

        // Act
        var result = DuckType.OutParameterMethod((IDuckType x) => x.TryGetValue(2,  new Out()), dictionary, out var values);

        // Assert
        result.TryGetOk(out var ok).ShouldBeTrue();
        ok.ShouldBe(true);

        values.ShouldNotBeNull();
        values.Length.ShouldBe(1);
        values[0].ShouldBe("2");
    }
}
