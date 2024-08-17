#if NET6_0_OR_GREATER
using FluentAssertions;
using Foundation.Text.Json.Serialization;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Foundation.TestUtil.Text.Json.Serialization;

[TestFixture]
public class ObjectConverterTests
{
    [Test]
    public void Read_Should_Transform_ToRightType_When_ListOfObjectsIsUsed()
    {
        var dateTimeValue = new DateTime(2020, 1, 2, 3, 4, 5);
        var dateOnlyValue = dateTimeValue.ToDateOnly();
        var doubleValue = 12.345;
        var intValue = 12345;
        var strValue = "12345";
        var timeOnlyValue = dateTimeValue.ToTimeOnly();
        var timeSpan = TimeSpan.FromSeconds(1.234);
        
        List<object?> values = [dateTimeValue, dateOnlyValue, doubleValue, intValue, strValue, timeOnlyValue, timeSpan];

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
            desirialized.Count.Should().Be(7);
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
                    if (value is string str) str.Should().Be(strValue);
                    else Assert.Fail($"{nameof(value)} is not of type {nameof(String)}.");
                }
            }
            {
                var value = desirialized[5];
                value.Should().NotBeNull();
                if (value != null)
                {
                    if (value is TimeOnly timeOnly) timeOnly.Should().Be(timeOnlyValue);
                    else Assert.Fail($"{nameof(value)} is not of type {nameof(TimeOnly)}.");

                }
            }
            {
                var value = desirialized[6];
                value.Should().NotBeNull();
                if (value != null)
                {
                    if (value is TimeSpan ts) ts.Should().Be(timeSpan);
                    else Assert.Fail($"{nameof(value)} is not of type {nameof(TimeSpan)}.");

                }
            }
        }
    }
}
#endif