using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Foundation.Text.Json.Serialization;

[TestFixture]
public class ObjectConverterTests
{
    [Test]
    public void Deserialize_Should_Transform_ToRightType_When_ListOfObjectsIsUsed()
    {
        //Arrange
        var dateTimeValue = new DateTime(2020, 1, 2, 3, 4, 5);
        var dateOnlyValue = dateTimeValue.ToDateOnly();
        var doubleValue = 12.345;
        var intValue = 12345;
        var quantity = new Quantity("kg", 12.34M);
        var strValue = "12345";
        var timeOnlyValue = dateTimeValue.ToTimeOnly();
        var timeSpan = TimeSpan.FromSeconds(1.234);

        List<object?> values = [dateTimeValue, dateOnlyValue, doubleValue, intValue, quantity, strValue, timeOnlyValue, timeSpan];

        var serializeOptions = new JsonSerializerOptions
        {
            Converters =
            {
                new ObjectJsonConverter()
            }
        };

        var json = JsonSerializer.Serialize(values, serializeOptions);

        var desirialized = JsonSerializer.Deserialize<List<object?>>(json, serializeOptions);
        desirialized.Should().NotBeNull();

        if (null != desirialized)
        {
            desirialized.Count.Should().Be(8);
            {
                var value = desirialized[0];
                value.Should().NotBeNull();
                if (value != null)
                {
                    if (value is DateTime dateTime) dateTime.Should().Be(dateTimeValue);
                    else Assert.Fail($"{nameof(value)} is not of type {nameof(DateTime)}.");
                }
            }
            {
                var value = desirialized[1];
                value.Should().NotBeNull();
                if (value != null)
                {
                    if (value is DateOnly dateOnly) dateOnly.Should().Be(dateOnlyValue);
                    else Assert.Fail($"{nameof(value)} is not of type {nameof(DateOnly)}.");
                }
            }
            {
                var value = desirialized[2];
                value.Should().NotBeNull();
                if (value != null)
                {
                    if (value is double d) d.Should().Be(doubleValue);
                    else Assert.Fail($"{nameof(value)} is not of type {nameof(Double)}.");
                }
            }
            {
                var value = desirialized[3];
                value.Should().NotBeNull();
                if (value != null)
                {
                    if (value is int i) i.Should().Be(intValue);
                    else Assert.Fail($"{nameof(value)} is not of type {nameof(Int32)}.");
                }
            }
            {
                var value = desirialized[4];
                value.Should().NotBeNull();
                if (value != null)
                {
                    if (value is Quantity q) q.Should().Be(quantity);
                    else Assert.Fail($"{nameof(value)} is not of type {nameof(Quantity)}.");
                }
            }
            {
                var value = desirialized[5];
                value.Should().NotBeNull();
                if (value != null)
                {
                    if (value is string str) str.Should().Be(strValue);
                    else Assert.Fail($"{nameof(value)} is not of type {nameof(String)}.");
                }
            }
            {
                var value = desirialized[6];
                value.Should().NotBeNull();
                if (value != null)
                {
                    if (value is TimeOnly timeOnly) timeOnly.Should().Be(timeOnlyValue);
                    else Assert.Fail($"{nameof(value)} is not of type {nameof(TimeOnly)}.");
                }
            }
            {
                var value = desirialized[7];
                value.Should().NotBeNull();
                if (value != null)
                {
                    if (value is TimeSpan ts) ts.Should().Be(timeSpan);
                    else Assert.Fail($"{nameof(value)} is not of type {nameof(TimeSpan)}.");
                }
            }
        }
    }

    [Test]
    public void Serialize_Should_Transform_ToRightType_When_ListOfObjectsIsUsed()
    {
        //Arrange
        var dateTimeValue = new DateTime(2020, 1, 2, 3, 4, 5);
        var dateOnlyValue = dateTimeValue.ToDateOnly();
        var doubleValue = 12.345;
        var intValue = 12345;
        var quantity = new Quantity("kg", 12.34M);
        var strValue = "12345";
        var timeOnlyValue = dateTimeValue.ToTimeOnly();
        var timeSpan = TimeSpan.FromSeconds(1.234);
        
        List<object?> values = [dateTimeValue, dateOnlyValue, doubleValue, intValue, quantity, strValue, timeOnlyValue, timeSpan];

        var serializeOptions = new JsonSerializerOptions
        {
            Converters =
            {
                new ObjectJsonConverter()
            }
        };

        //Act
        var json = JsonSerializer.Serialize(values, serializeOptions);
        
        var expected = """["2020-01-02T03:04:05","2020-01-02",12.345,12345,{"Unit":"kg","Value":12.34},"12345","03:04:05","PT1S234F"]""";
        json.Should().Be(expected);
    }
}
