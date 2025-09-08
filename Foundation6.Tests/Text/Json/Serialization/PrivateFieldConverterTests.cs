using NUnit.Framework;
using Shouldly;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;

[TestFixture]
public class PrivateFieldConverterTests
{
    [Test]
    public void Deserialize_Should_ReturnAnObjectWithInitializedFields_When_ConstructorIsMarkedWithJsonConstructor()
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            Converters = { new PrivateFieldConverterFactory(typeof(JsonTest1<int>)) }
        };

        int[] array = [1, 2, 3];
        var name = "John";
        var number = 123;
        object? jsonTest = new JsonTest1<int>(array, name, number);

        var json = JsonSerializer.Serialize(jsonTest, options);

        var deserialized = JsonSerializer.Deserialize<JsonTest1<int>>(json, options);
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
            Converters = { new PrivateFieldConverterFactory(typeof(JsonTest2<int>)) }
        };

        int[] array = [1, 2, 3];
        var name = "John";
        var number = 123;
        object? jsonTest = new JsonTest2<int>(array, name, number);

        var json = JsonSerializer.Serialize(jsonTest, options);

        var deserialized = JsonSerializer.Deserialize<JsonTest2<int>>(json, options);
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
            IncludeFields = true,
            Converters = { new PrivateFieldConverterFactory(typeof(JsonTest1<int>)) }
        };

        int[] array = [1, 2, 3];
        var name = "John";
        var number = 123;
        object? jsonTest = new JsonTest1<int>(array, name, number);

        var json = JsonSerializer.Serialize(jsonTest, options);
        json.ShouldBe("""{"__private_fields__":{"_array":[1,2,3],"_name":"John","_number":123}}""");
    }

    [Test]
    public void Serialize_Should_ReturnAJsonStringWithPrivateFieldsProperty_When_ObjectHasAPrivateDefaultConstructor()    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            Converters = { new PrivateFieldConverterFactory(typeof(JsonTest2<int>)) }
        };

        int[] array = [1, 2, 3];
        var name = "John";
        var number = 123;
        object? jsonTest = JsonTest2.New(array, name, number);

        var json = JsonSerializer.Serialize(jsonTest, options);
        json.ShouldBe("""{"__private_fields__":{"_array":[1,2,3],"_name":"John","_number":123}}""");
    }
}

public class JsonTest1<T>
{
    [JsonPrivateField]
    private T[] _array;

    [JsonPrivateField]
    private string _name;

    [JsonPrivateField]
    private int _number;

    [JsonConstructor]
    public JsonTest1(T[] array, string name, int number)
    {
        _array = array;
        _name = name;
        _number = number;
    }

    [JsonIgnore]
    public ReadOnlySpan<T> Array => _array;

    public string Name => _name;
    public int Number => _number;
}

public static class JsonTest2
{
    public static JsonTest2<T> New<T>(T[] array, string name, int number)
    {
        array.ThrowIfNull();
        name.ThrowIfNullOrEmpty();
        return new(array, name, number);
    }
}

public class JsonTest2<T>
{
    [JsonPrivateField]
    private T[] _array;

    [JsonPrivateField]
    private string _name;

    [JsonPrivateField]
    private int _number;


    internal JsonTest2()
    {
    }

    internal JsonTest2(T[] array, string name, int number)
    {
        _array = array;
        _name = name;
        _number = number;
    }

    [JsonIgnore]
    public ReadOnlySpan<T> Array => _array;

    public string Name => _name;


    public int Number => _number;
}