using NUnit.Framework;
using Shouldly;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;

[TestFixture]
public class FactoryMethodConverterTests
{
    [Test]
    public void Deserialize_Should_ReturnAnObjectWithInitializedFields_When_ConstructorIsMarkedWithJsonConstructor()
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            Converters = { new FactoryMethodConverterFactory(typeof(MyClassWithFactoryMethod)) }
        };

        int[] array = [1, 2, 3];
        var name = "John";
        var number = 123;
        object? obj = MyClassWithFactoryMethod.New(array, name, number);

        var json = JsonSerializer.Serialize(obj, options);

        var deserialized = JsonSerializer.Deserialize<MyClassWithFactoryMethod>(json, options);
        deserialized.ShouldNotBeNull();

        deserialized.Array.Length.ShouldBe(array.Length);
        deserialized.Array[0].ShouldBe(array[0]);
        deserialized.Array[1].ShouldBe(array[1]);
        deserialized.Array[2].ShouldBe(array[2]);

        deserialized.Name.ShouldBe(name);
        deserialized.Number.ShouldBe(number);
    }

    [Test]
    public void Deserialize_Should_ReturnAnObjectWithInitializedFields_When_ObjectHasDefaultConstructor()
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            Converters = { new FactoryMethodConverterFactory() }
        };

        int[] array = [1, 2, 3];
        var name = "John";
        var number = 123;
        object? obj = MyClassWithFactoryMethod.New(array, name, number);

        var json = JsonSerializer.Serialize(obj, options);

        var deserialized = JsonSerializer.Deserialize<MyClassWithFactoryMethod>(json, options);
        deserialized.ShouldNotBeNull();

        deserialized.Array.Length.ShouldBe(array.Length);
        deserialized.Array[0].ShouldBe(array[0]);
        deserialized.Array[1].ShouldBe(array[1]);
        deserialized.Array[2].ShouldBe(array[2]);

        deserialized.Name.ShouldBe(name);
        deserialized.Number.ShouldBe(number);
    }
    [Test]
    public void Serialize_Should_ReturnAJsonStringWithPrivateFieldsProperty_When_ObjectHasConstructorMarkedWithJsonConstructor()
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new FactoryMethodConverterFactory(typeof(MyClassWithFactoryMethod)) }
        };

        int[] array = [1, 2, 3];
        var name = "John";
        var number = 123;
        object? obj = MyClassWithFactoryMethod.New(array, name, number);

        var json = JsonSerializer.Serialize(obj, options);
        json.ShouldBe("""{"__factory_method_parameters__":{"array":[1,2,3],"name":"John","number":123}}""");
    }
}

public class MyClassWithFactoryMethod
{
    [JsonFactoryMethodParameter("array")]
    private int[] _array;

    [JsonFactoryMethodParameter("name")]
    private string _name;

    [JsonFactoryMethodParameter("number")]
    private int _number;

    internal MyClassWithFactoryMethod(int[] array, string name, int number)
    {
        _array = array;
        _name = name;
        _number = number;
    }

    [JsonIgnore]
    public ReadOnlySpan<int> Array => _array;

    [JsonIgnore]
    public string Name => _name;

    [JsonFactoryMethod]
    public static MyClassWithFactoryMethod New(int[] array, string name, int number)
    {
        array.ThrowIfNull();
        name.ThrowIfNullOrEmpty();
        return new(array, name, number);
    }

    [JsonIgnore]
    public int Number => _number;
}
